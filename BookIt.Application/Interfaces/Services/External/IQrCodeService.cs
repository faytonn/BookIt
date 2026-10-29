namespace BookIt.Application.Interfaces.Services.External;

public interface IQrCodeService
{
    /// <summary>PNG image of a QR code holding <paramref name="content"/>.</summary>
    byte[] Png(string content, int pixelsPerModule = 10);
}
