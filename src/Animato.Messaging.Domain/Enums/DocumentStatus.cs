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

    public static readonly DocumentStatus Send = new(4, nameof(BeingSend));

    public static readonly DocumentStatus Failed = new(5, nameof(BeingSend));

    public static readonly DocumentStatus WaitingForSchedule = new(6, nameof(BeingSend));

    /// <summary>
    /// Type of target document generated from template and data
    /// </summary>
    /// <param name="value">Numeric value</param>
    /// <param name="name">Text description</param>
    public DocumentStatus(int value, string name) : base(name, value)
    {
    }
}
