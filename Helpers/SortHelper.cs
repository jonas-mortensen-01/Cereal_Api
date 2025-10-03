using System.Reflection;
using Microsoft.EntityFrameworkCore;

public static class SortHelper
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, List<SortOrder> sortOrders)
    {
        IOrderedQueryable<T>? sorted = null;

        for (int i = 0; i < sortOrders.Count; i++)
        {
            var sort = sortOrders[i];
            if (i == 0)
            {
                sorted = sort.Direction == Order.Asc
                    ? source.OrderBy(x => EF.Property<object>(x, sort.Field))
                    : source.OrderByDescending(x => EF.Property<object>(x, sort.Field));
            }
            else
            {
                sorted = sort.Direction == Order.Asc
                    ? sorted.ThenBy(x => EF.Property<object>(x, sort.Field))
                    : sorted.ThenByDescending(x => EF.Property<object>(x, sort.Field));
            }
        }

        var result = sorted ?? source;

        return result;
    }
}
