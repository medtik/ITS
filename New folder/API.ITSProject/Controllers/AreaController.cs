namespace API.ITSProject.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Pagination;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using API.ITSProject.ViewModels;

    public class AreaController : _BaseController
    {
        private readonly IAreaService _areaService;

        public AreaController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService, IAreaService areaService) : base(loggingService, paggingService, identityService)
        {
            this._areaService = areaService;
        }

        #region Get
        [HttpGet]
        [Route("api/Area/Details")]
        public IHttpActionResult GetDetails(int id)
        {
            try
            {
                Area area = _areaService.Search(_ => _.Id == id, 
                    _ => _.Locations.Select(__ => __.Reviews),
                    _ => _.Locations.Select(__ => __.Photos.Select(___ => ___.Photo)),
                    _ => _.Plans.Select(__ => __.Voters),
                    _ => _.Plans.Select(__ => __.PlanLocations.Select(___ => ___.Location).Select(____ => ____.Photos.Select(_____ => _____.Photo))),
                    _ => _.Photos.Select(__ => __.Photo)

                    ).FirstOrDefault();
                if (area == null)
                {
                    return BadRequest("Not found");
                } else
                {
                    return Ok(ModelBuilder.ConvertToAreaDetailsViewModels(area));
                }
            }
            catch (Exception ex)
            { 
                _loggingService.Write(GetType().Name, nameof(GetDetails), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult Get(string searchValue = "", string orderBy = "", int? pageIndex = 1, int? pageSize = 10)
        {
            IEnumerable<AreaViewModels> currentList = Enumerable.Empty<AreaViewModels>();

            try
            {
                Pager<Area> pager = null;
                IQueryable<Area> listAreas = _areaService.Search(_ => string.IsNullOrEmpty(searchValue)
                                    || _.Name.Contains(searchValue), _ => _.Locations, _ => _.Questions);

                orderBy = orderBy?.ToLower();
                switch (orderBy)
                {
                    case "locationcount_desc":
                        pager = _paggingService.ToPagedList(listAreas.
                            OrderByDescending(_ => _.Locations.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "locationcount_asc":
                        pager = _paggingService.ToPagedList(listAreas.
                            OrderBy(_ => _.Locations.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "questioncount_asc":
                        pager = _paggingService.ToPagedList(listAreas.
                            OrderBy(_ => _.Questions.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "questioncount_desc":
                        pager = _paggingService.ToPagedList(listAreas.
                            OrderByDescending(_ => _.Questions.Count), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    default:
                        pager = _paggingService.ToPagedList(listAreas.
                            OrderBy(_ => _.Name), pageIndex ?? 1, pageSize ?? 10);
                        break;
                }
                currentList = ModelBuilder.ConvertToAreaViewModels(pager.CurrentList);

                return Ok(new
                {
                    meta = new
                    {
                        pageIndex = pager.PageIndex,
                        pageSize = pager.PageSize,
                        totalElement = pager.TotalElement,
                        totalPage = pager.TotalPage,
                        orderBy,
                        searchValue
                    },
                    currentList
                });
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Get), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/GetFeaturedArea")]
        public IHttpActionResult GetFeaturedArea()
        {
            IList<AreaFeatureViewModels> currentList = new List<AreaFeatureViewModels>();
            try
            {
                IQueryable<Area> listAreas = _areaService.Search(_ => true, _ => _.Locations
                            .Select(__ => __.PlanLocations.Select(___ => ___.Plan)),
                            _ => _.Photos.Select(__ => __.Photo)).Take(5);

                foreach (var ele in listAreas)
                {
                    currentList.Add(ModelBuilder.ConvertToAreaFeaturedViewModels(ele));
                }

                return Ok(currentList.OrderByDescending(_ => (_.LocationCount + _.PlanCount)));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetFeaturedArea), ex);

                return InternalServerError(ex);
            }
        }
        #endregion
    }
}