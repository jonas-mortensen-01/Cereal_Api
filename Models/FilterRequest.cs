public class FilterRequest
{
    public List<Filter> Filters { get; set; } = new();
    public List<SortOrder> SortOrders { get; set; } = new();
}