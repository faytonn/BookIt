namespace BookIt.Infrastracture.Messaging;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    /// <summary>Empty host = messaging disabled (publisher becomes a no-op, consumer does not start).</summary>
    public string? Host { get; set; }
    public int Port { get; set; } = 5672;
    public string User { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "bookit";
    public string SeatSoldEmailQueue { get; set; } = "bookit.seat-sold.email";

    public bool Enabled => !string.IsNullOrWhiteSpace(Host);
}
