namespace Core.ObjectModels.Pagination
{
    using System.Collections.Generic;

    public class Pager<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPage { get; set; }

        public IEnumerable<T> CurrentList { get; set; }

        public int TotalElement { get; set; }
    }
}