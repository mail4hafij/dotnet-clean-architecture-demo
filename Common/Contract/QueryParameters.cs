using System.ComponentModel.DataAnnotations;

namespace Common.Contract
{
    public class QueryParameters
    {
        public string? Where { get; set; }      // E.g., "Deleted == false AND Nameplate == \"FCKISRL\""
        public string? OrderBy { get; set; }    // E.g., "Nameplate asc"
        public string? Include { get; set; }    // E.g., "Cars"

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than zero")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100")]
        public int Limit { get; set; } = 25;
    }
}
