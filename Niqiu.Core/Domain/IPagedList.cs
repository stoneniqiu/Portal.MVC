using System.Collections.Generic;

namespace Niqiu.Core.Domain
{
    public interface IPagedList<T>:IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
