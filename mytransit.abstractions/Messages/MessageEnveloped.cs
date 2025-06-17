
namespace mytransit.abstractions.Messages;

public class EMessage<T>
{

    public required string MessageId { get; set; }
    public required string CorrelationId { get; set; }
    public required T payload { get; set; }
    public DateTime TimesStamp { get; init; } = DateTime.UtcNow;

}
