using BookIt.Application.Interfaces.Services.External;
using QRCoder;

namespace BookIt.Infrastracture.External;

/// <summary>QR codes via QRCoder's PNG writer (no System.Drawing, so it runs in the Linux container too).</summary>
public class QrCodeService : IQrCodeService
{
    public byte[] Png(string content, int pixelsPerModule = 10)
    {
        using var generator = new QRCodeGenerator();
        // level M survives about 15% damage: a scratched phone screen or a creased printout still scans
        using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
        return new PngByteQRCode(data).GetGraphic(pixelsPerModule);
    }
}
