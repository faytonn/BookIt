using BookIt.Application.DTOs.DashboardDTO;

namespace BookIt.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<GetDashboardDTO> GetAsync(int salesDays = 14);
}
