public class RequestContext
{
    public List<Filter> Filters { get; set; } = new();
    public List<SortOrder> SortOrders { get; set; } = new();
}