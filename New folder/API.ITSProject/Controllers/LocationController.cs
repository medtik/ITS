using System.Data.Entity;
using Core.ObjectModels.Algorithm;

namespace API.ITSProject.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Pagination;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.Algorithm;
    using API.ITSProject.ViewModels;
    using System.Device.Location;

    public class LocationController : _BaseController
    {
        private readonly ILocationService _locationService;
        private readonly ITagService _tagService;

        private readonly ISearchTreeService _searchTreeService;

        public LocationController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService, ILocationService locationService, ISearchTreeService searchTreeService,
            ITagService tagService)
            : base(loggingService, paggingService, identityService)
        {
            this._locationService = locationService;
            this._tagService = tagService;
            this._searchTreeService = searchTreeService;
        }

        #region Get
        [HttpGet]
        [Route("api/Location/NearbyLocation")]
        public IHttpActionResult GetNearbyLocation(double longitude, double latitude, double radius)
        {
            IList<LocationViewModels> result = new List<LocationViewModels>();
            var searchLoation = new GeoCoordinate(latitude, longitude);

            var listLocation = _locationService.GetAll();

            foreach (var ele in listLocation)
            {
                var tmpLocation = new GeoCoordinate(ele.Latitude, ele.Longitude);

                if (searchLoation.GetDistanceTo(tmpLocation) <= radius)
                {
                    result.Add(ModelBuilder.ConvertToViewModels(ele));
                }// end if check is in radius
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/location/categories")]
        public IHttpActionResult GetCategories()
        {
            return Ok(_locationService.GetCategories());
        }


        [HttpGet]
        [Route("api/Details")]
        public IHttpActionResult GetDetails([FromUri] int id)
        {
            try
            {
                LocationDetailViewModels locationDetail;
                Location location = _locationService.Find(id, _ => _.BusinessHours, _ => _.Reviews,
                    _ => _.BusinessHours, _ => _.Tags, _ => _.Photos.Select(__ => __.Photo));

                if (location == null)
                {
                    return BadRequest();
                }

                locationDetail = ModelBuilder.ConvertToLocationDetailViewModels(location);
                return Ok(locationDetail);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetDetails), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Test")]
        public IHttpActionResult Algorithm([FromUri] int[] list, int? areaId)
        {
            WrireTree();
            if (list == null)
            {
                return BadRequest();
            }

            List<Tag> tags = _tagService.GetAll()
                .Where(tag => tag.Answer
                    .Any(answer => list.Contains(answer.Id))
                )
                .ToList();

            var path = CurrentContext.Server.MapPath("/Tree/tree.json");
            Tree tree = _searchTreeService.ReadTree(path);
            if (tree == null)
            {
                tree = _searchTreeService.BuildTree(_locationService.GetAll().Include(location => location.Tags).ToList());
                _searchTreeService.WriteTree(path, tree);
            }

            var resultIds = _searchTreeService.SearchTree(tags, tree);
            var locationsResult = _locationService.GetAll()
                .Include(location => location.Reviews)
                .Include(location => location.Tags)
                .Include(location => location.Photos.Select(__ => __.Photo))
                .Where(location => !areaId.HasValue || location.AreaId == areaId)
                .Where(location => resultIds.Contains(location.Id));

            List<TreeViewModels> resultList = new List<TreeViewModels>();
            foreach (Location location in locationsResult)
            {
                var locationTags = location.Tags;
                var commonTags = locationTags.Intersect(tags);
                int ratingCount = location.Reviews.Count;
                var rating = location.Reviews.Sum(_ => _.Rating) / ratingCount;
                rating = float.IsNaN(rating) ? 0 : rating;

                foreach (Review review in location.Reviews)
                {
                    rating += review.Rating;
                }

                TreeViewModels result = new TreeViewModels
                {
                    Id = location.Id,
                    Address = location.Address,
                    Location = location.Name,
                    Percent = (tags.Count / commonTags.Count()).ToString(),
                    PrimaryPhoto = location.Photos.FirstOrDefault(_ => _.IsPrimary)?.Photo.Path,
                    Rating = rating,
                    Reasons = commonTags.Select(tag => tag.Name).ToList(),
                    ReviewCount = location.Reviews.Count,
                    Categories = location.Category
                };

                resultList.Add(result);
            }


            return Ok(resultList.OrderByDescending(_ => _.Reasons.Count));
        }

        [HttpGet]
        [Route("api/Location/SearchClient")]
        public IHttpActionResult SearchClient(string searchValue = "", string type = "", int? pageIndex = 1,
            int? pageSize = 10, int? areaId = null)
        {
            IEnumerable<LocationViewModels> currentList = Enumerable.Empty<LocationViewModels>();
            try
            {
                Pager<Location> pager = null;
                IQueryable<Location> listLocations = null;
                if (areaId.HasValue)
                {
                    listLocations = _locationService
                    .Search(_ => !_.IsDelete && (string.IsNullOrEmpty(searchValue) ||
                                                 _.Name.Contains(searchValue) ||
                                                 _.Address.Contains(searchValue)) && 
                                                 _.AreaId == areaId.Value, _ => _.Area, _ => _.Reviews, _ => _.Photos.Select(__ => __.Photo));
                } else
                {
                    listLocations = _locationService
                    .Search(_ => !_.IsDelete && (string.IsNullOrEmpty(searchValue) ||
                                                 _.Name.Contains(searchValue) ||
                                                 _.Address.Contains(searchValue)), _ => _.Area, _ => _.Reviews, _ => _.Photos.Select(__ => __.Photo));
                }
                if (string.IsNullOrWhiteSpace(type))
                {
                    pager = _paggingService.ToPagedList(listLocations.OrderByDescending(e => e.Name),
                    pageIndex ?? 1, pageSize ?? 10);
                } else
                {
                    pager = _paggingService.ToPagedList(listLocations.Where(_ => _.Category == type).OrderByDescending(e => e.Name),
                    pageIndex ?? 1, pageSize ?? 10);
                }
                
                currentList = ModelBuilder.ConvertToViewModels(pager.CurrentList);

                return Ok(new
                {
                    meta = new
                    {
                        pageIndex = pager.PageIndex,
                        pageSize = pager.PageSize,
                        totalElement = pager.TotalElement,
                        totalPage = pager.TotalPage,
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
        public IHttpActionResult Get(string searchValue = "", string orderBy = "", int? pageIndex = 1,
            int? pageSize = 10)
        {
            IEnumerable<LocationViewModels> currentList = Enumerable.Empty<LocationViewModels>();
            try
            {
                Pager<Location> pager = null;
                IQueryable<Location> listLocations = _locationService
                    .Search(_ => !_.IsDelete && (string.IsNullOrEmpty(searchValue) ||
                                                 _.Name.Contains(searchValue) ||
                                                 _.Address.Contains(searchValue) ||
                                                 _.EmailAddress.Contains(searchValue) ||
                                                 _.PhoneNumber.Contains(searchValue) ||
                                                 _.Website.Contains(searchValue)), _ => _.Area, _ => _.Reviews, _ => _.Photos.Select(__ => __.Photo));

                orderBy = orderBy?.ToLower();
                switch (orderBy)
                {
                    case "areaname_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(_ => _.Area.Name), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "areaname_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(_ => _.Area.Name),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "isclosed_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(_ => _.IsClosed), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "isclosed_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(_ => _.IsClosed),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "isverified_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(_ => _.IsVerified), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "isverified_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(_ => _.IsVerified),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "address_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(_ => _.Address), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "address_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(_ => _.Address),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "website_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(_ => _.Website), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "website_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(_ => _.Website),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "phonenumber_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(e => e.PhoneNumber), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "phonenumber_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(e => e.PhoneNumber),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "emailaddress_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(e => e.EmailAddress), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    case "emailaddress_desc":
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(e => e.EmailAddress),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "name_asc":
                        pager = _paggingService.ToPagedList(listLocations.OrderBy(e => e.Name), pageIndex ?? 1,
                            pageSize ?? 10);
                        break;
                    default:
                        //"name_desc"
                        pager = _paggingService.ToPagedList(listLocations.OrderByDescending(e => e.Name),
                            pageIndex ?? 1, pageSize ?? 10);
                        break;
                }

                currentList = ModelBuilder.ConvertToViewModels(pager.CurrentList);

                CurrentContext.Response.Headers.Add("Paging-Header", JsonConvert.SerializeObject(new
                {
                    pageIndex = pager.PageIndex,
                    pageSize = pager.PageSize,
                    totalElement = pager.TotalElement,
                    totalPage = pager.TotalPage,
                    orderBy,
                })); //add meta-data to current response's header

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
        #endregion

        #region Post
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> Post(CreateLocationViewModels data)
        {
            try
            {
                int userId = (await CurrentUser()).Id;

                Photo primaryPhoto = ModelBuilder.ConvertToModels(data.PrimaryPhoto, userId);
                IEnumerable<Photo> otherPhoto = ModelBuilder.ConvertToModels(data.OtherPhotos.AsEnumerable(), userId);

                Location location = ModelBuilder.ConvertToModels(data);

                IEnumerable<BusinessHour> businessHours = ModelBuilder.ConvertToModels(data.Days);

                bool result = _locationService.Create(location, primaryPhoto, otherPhoto, businessHours, data.Tags);

                if (result)
                {
                    WrireTree();
                    return Ok();
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Post), ex);

                return InternalServerError(ex);
            }
        }

        #endregion

        #region Put
        [HttpPut]
        [Authorize, Route("api/Location/ApproveSuggestion")]
        public IHttpActionResult AppectLocationSuggestion(int suggestionId)
        {
            try
            {
                bool result = _locationService.AcceptStatusLocationSuggestion(suggestionId);
                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AppectLocationSuggestion), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize, Route("api/Location/RejectSuggestion")]
        public IHttpActionResult RejectLocationSuggestion(int suggestionId)
        {
            try
            {
                bool result = _locationService.RejectStatusLocationSuggestion(suggestionId);
                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AppectLocationSuggestion), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Delete

        [HttpDelete]
        public IHttpActionResult Delete(int locationId)
        {
            try
            {
                bool isSuccess = _locationService.Delete(locationId);

                if (isSuccess)
                {
                    WrireTree();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Delete), ex);

                return InternalServerError(ex);
            }
        }

        #endregion

        #region Other

        private void WrireTree()
        {
            Tree tree = _searchTreeService.BuildTree(_locationService.GetAll(_ => _.Tags).ToList());
            var path = CurrentContext.Server.MapPath("/Tree/tree.json");
            _searchTreeService.WriteTree(path, tree);
            //            var tree = _treeBuilder.BuildTree(_locationService.GetAll(_ => _.Tags).ToList());
            //            System.IO.File.WriteAllText(CurrentContext.Server.MapPath($"/Tree/tree.json"),
            //                JsonConvert.SerializeObject(tree), Encoding.UTF8);
        }

        #endregion
    }
}