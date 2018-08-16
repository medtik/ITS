namespace Core.ApplicationService.Business.PagingService
{
    using System.Linq;
    using Core.ObjectModels.Pagination;

    public interface IPagingService
    {
        Pager<T> ToPagedList<T>(IOrderedQueryable<T> data, int pageIndex, int pageSize) where T : class;
    }
}