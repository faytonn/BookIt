namespace BookIt.Domain.AppSettingModels;

public class MailKitOptions
{
    public string Mail { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public string Port { get; set; } = null!;
}
