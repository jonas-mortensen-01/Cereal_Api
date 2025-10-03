using System.Text.Json.Serialization;

public class SortOrder
{
    public required string Field { get; set; }   // e.g. "Calories", "Cups"
    public Order Direction { get; set; } = Order.Asc; // Your enum Order
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Order
{
    Asc,
    Desc
}