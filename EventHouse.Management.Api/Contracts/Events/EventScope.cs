namespace EventHouse.Management.Api.Contracts.Events;

/// <summary>
/// Geographical scope of the event.
/// Serialized as string in JSON.
/// </summary>
public enum EventScope : byte
{
    /// <summary>
    /// Event takes place in a single city or region.
    /// </summary>
    Local = 1,

    /// <summary>
    /// Event takes place in multiple cities within the same country.
    /// </summary>
    National = 2,

    /// <summary>
    /// Event takes place in multiple countries at the same time.
    /// </summary>
    International = 3
}
