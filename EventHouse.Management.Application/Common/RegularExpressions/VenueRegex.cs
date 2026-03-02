using System.Text.RegularExpressions;

namespace EventHouse.Management.Application.Common.RegularExpressions;

public partial class VenueRegex
{
    [GeneratedRegex("^[A-Z]{2}$")]
    public static partial Regex CountryCode();
}
