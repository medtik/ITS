namespace Service.Implement.Pagination
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Core.ObjectModels.Pagination;
    using Core.ApplicationService.Business.PagingService;

    public class PagingService : IPagingService
    {
        public Pager<T> ToPagedList<T>(IOrderedQueryable<T> data, int pageIndex, int pageSize) where T : class
        {
            pageSize = pageSize == -1 ? int.MaxValue : pageSize;
            int totalElement = data.Count();
            int finalPage = (int)Math.Ceiling(totalElement / (double)pageSize);
            IList<T> currentList = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new Pager<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                CurrentList = currentList,
                TotalElement = totalElement,
                TotalPage = finalPage
            };
        }
    }
}