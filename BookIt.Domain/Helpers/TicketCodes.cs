using System.Security.Cryptography;

namespace BookIt.Domain.Helpers;

/// <summary>
/// Codes printed on bookings and tickets. The alphabet leaves out 0/O and 1/I/L so a code read out
/// over the phone or typed from paper can't be misread.
/// </summary>
public static class TicketCodes
{
    private const string _alphabet = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";

    /// <summary>Booking reference the customer quotes, e.g. "BK-7F3K2Q" (31^6 ≈ 887 million).</summary>
    public static string NewBookingCode() => "BK-" + _random(6);

    /// <summary>
    /// Secret inside the QR code: whoever holds it gets in, so it must not be guessable.
    /// 16 characters ≈ 79 bits from a cryptographic RNG.
    /// </summary>
    public static string NewTicketCode() => _random(16);

    private static string _random(int length) => RandomNumberGenerator.GetString(_alphabet, length);
}
