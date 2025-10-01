public class Filter
{
    public required string Field { get; set; }   // e.g. "Calories", "Cups"
    public required string Operator { get; set; } // e.g. "=", ">", "<", ">=", "contains"
    public required string Value { get; set; }   // keep it string, convert later
}