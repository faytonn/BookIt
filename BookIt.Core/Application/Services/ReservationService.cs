using BookIt.Core.Application.DTOs;
using BookIt.Core.Application.Interfaces;
using BookIt.Core.Domain.Entities;



namespace BookIt.Core.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDBContext _context;

        public ReservationService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ReservationResponseDTO> CreateReservationAsync(int userId, CreateReservationDTO reservationDto)
        {
            var @event = await _context.Events
                .Include(e => e.Pricing)
                .FirstOrDefaultAsync(e => e.Id == reservationDto.EventId)
                ?? throw new InvalidOperationException("Event not found");

            var pricing = @event.Pricing
                .FirstOrDefault(p => p.Id == reservationDto.PricingId)
                ?? throw new InvalidOperationException("Invalid ticket category");

            if (pricing.AvailableSeats < reservationDto.Quantity)
            {
                throw new InvalidOperationException("Not enough seats available");
            }

            var reservation = new Reservation
            {
                ReservationNumber = GenerateReservationNumber(),
                EventId = reservationDto.EventId,
                UserId = userId,
                PricingId = reservationDto.PricingId,
                Quantity = reservationDto.Quantity,
                TotalAmount = pricing.Price * reservationDto.Quantity,
                Status = "Pending",
                FirstName = reservationDto.FirstName,
                LastName = reservationDto.LastName,
                Email = reservationDto.Email,
                Phone = reservationDto.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Reservations.Add(reservation);

                // Update only available seats using raw SQL to prevent unintended updates
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE EventPricing SET AvailableSeats = AvailableSeats - {0} WHERE Id = {1}",
                    reservationDto.Quantity, reservationDto.PricingId
                );

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return await GetReservationByIdAsync(reservation.Id);
        }

        public async Task<ReservationResponseDTO> GetReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Event)
                .Include(r => r.Pricing)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new InvalidOperationException("Reservation not found");

            return MapToReservationResponse(reservation);
        }

        public async Task<List<ReservationResponseDTO>> GetUserReservationsAsync(string email)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Event)
                .Include(r => r.Pricing)
                .Where(r => r.Email == email && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reservations.Select(MapToReservationResponse).ToList();
        }

        public async Task<bool> DeleteReservationAsync(int id, string email)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Pricing)
                .FirstOrDefaultAsync(r => r.Id == id && r.Email == email);

            if (reservation == null)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update available seats
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE EventPricing SET AvailableSeats = AvailableSeats + {0} WHERE Id = {1}",
                    reservation.Quantity, reservation.PricingId
                );

                // Soft delete the reservation
                DbContextExtension.SoftDelete(_context, reservation);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static string GenerateReservationNumber()
        {
            return $"RES-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private static ReservationResponseDTO MapToReservationResponse(Reservation reservation)
        {
            return new ReservationResponseDTO
            {
                Id = reservation.Id,
                ReservationNumber = reservation.ReservationNumber,
                EventId = reservation.EventId,
                EventTitle = reservation.Event.Title,
                TicketCategory = reservation.Pricing.Category,
                TicketPrice = reservation.Pricing.Price,
                Quantity = reservation.Quantity,
                TotalAmount = reservation.TotalAmount,
                Status = reservation.Status,
                FirstName = reservation.FirstName,
                LastName = reservation.LastName,
                Email = reservation.Email,
                Phone = reservation.Phone,
                CreatedAt = reservation.CreatedAt
            };
        }
    }
}
