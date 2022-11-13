namespace Animato.Messaging.Domain.Enums;

using Ardalis.SmartEnum;

/// <summary>
/// Type of target document generated from template and data
/// </summary>
public sealed class DocumentStatus : SmartEnum<DocumentStatus>
{
    public static readonly DocumentStatus WaitingToProcess = new(0, nameof(WaitingToProcess));

    public static readonly DocumentStatus BeingProcessed = new(1, nameof(BeingProcessed));

    public static readonly DocumentStatus WaitingToSend = new(2, nameof(WaitingToSend));

    public static readonly DocumentStatus BeingSend = new(3, nameof(BeingSend));

    public static readonly DocumentStatus Send = new(4, nameof(Send));

    public static readonly DocumentStatus Failed = new(5, nameof(Failed));

    public static readonly DocumentStatus WaitingForSchedule = new(6, nameof(WaitingForSchedule));

    /// <summary>
    /// Type of target document generated from template and data
    /// </summary>
    /// <param name="value">Numeric value</param>
    /// <param name="name">Text description</param>
    public DocumentStatus(int value, string name) : base(name, value)
    {
    }
}

public sealed class QueueType : SmartEnum<QueueType>
{
    public static readonly QueueType Received = new(0, nameof(Received));
    public static readonly QueueType Processed = new(1, nameof(Processed));
    public static readonly QueueType Send = new(2, nameof(Send));
    public static readonly QueueType Failed = new(3, nameof(Failed));

    /// <summary>
    /// Type of document queues
    /// </summary>
    /// <param name="value">Numeric value</param>
    /// <param name="name">Text description</param>
    public QueueType(int value, string name) : base(name, value)
    {
    }
}
