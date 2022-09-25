namespace Animato.Messaging.Domain.Enums;

using Ardalis.SmartEnum;

/// <summary>
/// Type of target document generated from template and data
/// </summary>
public sealed class TargetType : SmartEnum<TargetType>
{
    /// <summary>
    /// Default brand for new partners
    /// </summary>
    public static readonly TargetType Email = new(0, nameof(Email));

    /// <summary>
    /// Inherited brand from other partner
    /// </summary>
    public static readonly TargetType Sms = new(1, nameof(Sms));

    /// <summary>
    /// Custom defined brand
    /// </summary>
    public static readonly TargetType Push = new(2, nameof(Push));

    /// <summary>
    /// Custom defined brand
    /// </summary>
    public static readonly TargetType Pdf = new(2, nameof(Pdf));

    /// <summary>
    /// Type of target document generated from template and data
    /// </summary>
    /// <param name="value">Numeric value</param>
    /// <param name="name">Text description</param>
    public TargetType(int value, string name) : base(name, value)
    {
    }
}
