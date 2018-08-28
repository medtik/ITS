using System.Data.Entity;
using Core.ObjectModels.Algorithm;

namespace API.ITSProject_2.Controllers
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
    using API.ITSProject_2.ViewModels;
    using System.Device.Location;
    using System.Net.Http;
    using System.Net;
    using System.IO;
    using System.Net.Http.Headers;
    using System.ComponentModel.DataAnnotations;
    using Core.ObjectModels.Entities.EnumType;

    public class LocationController : _BaseController
    {
        private readonly ILocationService _locationService;
        private readonly ITagService _tagService;
        private readonly ISearchTreeService _searchTreeService;
        private readonly IPlanService _planService;

        public LocationController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService, ILocationService locationService, ISearchTreeService searchTreeService,
            ITagService tagService, IPhotoService photoService, IPlanService planService) : base(loggingService, paggingService, identityService, photoService)
        {
            this._locationService = locationService;
            this._tagService = tagService;
            this._searchTreeService = searchTreeService;
            _planService = planService;
        }

        #region Get
        [HttpGet]
        [Route("api/Location/GetChangeRequests")]
        public IHttpActionResult GetRequest(int locationId)
        {
            try
            {
                var location = _locationService.Find(locationId, _ => _.ChangeRequests, _ => _.ChangeRequests.Select(__ => __.User));

                List<object> temp = new List<object>();

                foreach (var item in location.ChangeRequests)
                {
                    temp.Add(new
                    {
                        RequestId = item.Id,
                        User = new
                        {
                            Photo = item.User.Avatar,
                            Name = item.User.FullName
                        },
                        item.Status,
                        Detail = new
                        {
                            item.Name,
                            item.Address,
                            item.Description,
                            item.Website,
                            Phone = item.PhoneNumber,
                            Email = item.EmailAddress
                        }
                    });
                }
                return Ok(temp);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetRequest), ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/photo/converPhotoBase64")]
        public HttpResponseMessage ConvertToImage(int id)
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);

                MemoryStream memoryStream = new MemoryStream(ConvertToStream(id));

                result.Content = new ByteArrayContent(memoryStream.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                return result;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(ConvertToImage), ex);
                throw;
            }
        }

        [HttpGet]
        [Route("api/Location/NearbyLocation")]
        public IHttpActionResult GetNearbyLocation(double longitude, double latitude, double? radius = null)
        {
            try
            {
                if (!radius.HasValue)
                    radius = double.MaxValue;
                IList<LocationViewModels> result = new List<LocationViewModels>();
                var searchLoation = new GeoCoordinate(latitude, longitude);

                var listLocation = _locationService.GetAll(_ => _.Photos.Select(__ => __.Photo), __ => __.Area, _ => _.Reviews, _ => _.Photos.Select(__ => __.Photo));
                listLocation = listLocation.Where(_ => !_.IsDelete);
                foreach (var ele in listLocation)
                {
                    var tmpLocation = new GeoCoordinate(ele.Latitude, ele.Longitude);
                    double range = Math.Abs(searchLoation.GetDistanceTo(tmpLocation) - radius.Value);
                    if (searchLoation.GetDistanceTo(tmpLocation) <= radius.Value)
                    {
                        result.Add(ModelBuilder.ConvertToViewModels(ele, range));
                    }// end if check is in radius
                }

                return Ok(result.OrderByDescending(_ => _.Range));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetNearbyLocation), ex);
                return InternalServerError();
            }

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
                Location location = _locationService.Find(id, _ => _.BusinessHours, _ => _.Reviews.Select(__ => __.Creator),
                    _ => _.Reviews.Select(__ => __.Photos),
                    _ => _.BusinessHours, _ => _.Tags, _ => _.Photos.Select(__ => __.Photo),
                    _ => _.Area);

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
            locationsResult = locationsResult.Where(_ => !_.IsDelete);
            List<TreeViewModels> resultList = new List<TreeViewModels>();
            foreach (Location location in locationsResult)
            {
                var locationTags = location.Tags;
                var commonTags = locationTags.Intersect(tags);
                int ratingCount = location.Reviews.Count;
                var rating = location.Reviews.Sum(_ => _.Rating) / ratingCount;
                rating = float.IsNaN(rating) ? 0 : rating;

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
                    Categories = location.Category,
                    TotalTimeStay = location.TotalTimeStay
                };

                resultList.Add(result);
            }


            return Ok(resultList.OrderByDescending(_ => _.Reasons.Count).ThenByDescending(_ => _.Rating));
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
                }
                else
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
                }
                else
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
        public class ReviewViewModel
        {
            public int ReviewId { get; set; }

            public string Message { get; set; }
        }

        [HttpPost]
        [Route("api/Location/CreateReview")]
        public async Task<IHttpActionResult> CreateReview(ReviewViewModel review)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                Report cr = new Report
                {
                    ReviewId = review.ReviewId,
                    Content = review.Message,
                    UserId = (await CurrentUser()).Id
                };
                _locationService.CreateReport(cr);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(CreateChangeRequest), ex);
                return InternalServerError();
            }
        }

        public class ChangeRequestViewModels
        {
            [Required]
            public int LocationId { get; set; }

            [Required(ErrorMessage = "Tên không được trống")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Địa chỉ không được trống")]
            public string Address { get; set; }

            [Required]
            public string Description { get; set; }

            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            public string PhoneNumber { get; set; }

            public string Website { get; set; }

            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            public string Tags { get; set; }

            public ICollection<BusinessHourViewModels> BusinessHours { get; set; }
        }

        [HttpPost]
        [Route("api/Location/CreateChangeRequest")]
        public async Task<IHttpActionResult> CreateChangeRequest(ChangeRequestViewModels changeRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                ChangeRequest cr = new ChangeRequest
                {
                    Description = changeRequest.Description,
                    Address = changeRequest.Address,
                    EmailAddress = changeRequest.Email,
                    LocationId = changeRequest.LocationId,
                    Name = changeRequest.Name,
                    Website = changeRequest.Website,
                    Tags = changeRequest.Tags,
                    UserId = (await CurrentUser()).Id,
                    PhoneNumber = changeRequest.PhoneNumber,
                    BusinessHours = JsonConvert.SerializeObject(ModelBuilder.ConvertToModels(changeRequest.BusinessHours))
                };
                _locationService.CreateChangeRequest(cr);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(CreateChangeRequest), ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Authorize, Route("api/Location/AddImageToLocation")]
        public async Task<IHttpActionResult> AddImageToLocation(PhotoForLocationViewModels view)
        {
            try
            {
                int locationId = view.LocationId;
                string avatar = view.Avatar;
                int userId = (await CurrentUser()).Id;
                var temp = _locationService.Find(locationId);
                var photo = new Photo
                {
                    Path = avatar,
                    UserId = userId,
                };
                await _photoService.Create2(photo);

                List<LocationPhoto> locationPhotos = new List<LocationPhoto>();
                locationPhotos.Add(new LocationPhoto
                {
                    IsPrimary = false,
                    LocationId = locationId,
                    PhotoId = photo.Id
                });
                photo.Locations = locationPhotos;
                _photoService.Update(photo);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddImageToLocation), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> Post(CreateLocationViewModels data)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                int userId = (await CurrentUser()).Id;

                Photo primaryPhoto = ModelBuilder.ConvertToModels(data.PrimaryPhoto, userId);
                IEnumerable<Photo> otherPhoto = ModelBuilder.ConvertToModels(data.OtherPhotos.AsEnumerable(), userId);

                Location location = ModelBuilder.ConvertToModels(data);

                IEnumerable<BusinessHour> businessHours = ModelBuilder.ConvertToModels(data.Days);

                bool result = await _locationService.Create(location, primaryPhoto, otherPhoto, businessHours, data.Tags);

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
        public class TagsABCViewModels
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Categories { get; set; }

            public int LocationCount { get; set; }
        }

        [HttpPut]
        [Route("api/Location/AcceptLocationChangeRequest")]
        public IHttpActionResult AcceptLocationChangeRequest([FromBody]int requestId)
        {
            try
            {
                ChangeRequest temp = _locationService.FindChangeRequest(requestId);
                temp.Status = (int)RequestStatus.Approved;
                _locationService.UpdateChageRequest(temp);

                List<TagsABCViewModels> tags = JsonConvert.DeserializeObject<List<TagsABCViewModels>>(temp.Tags);

                List<Tag> tags2 = new List<Tag>();

                tags.ForEach(_ => 
                {
                    if (_tagService.Find(_.Id) != null)
                    {
                        tags2.Add(_tagService.Find(_.Id));
                    }
                });
                var location = _locationService.Find(temp.LocationId);

                location.Address = temp.Address;
                location.Name = temp.Name;
                location.Description = temp.Description;
                location.EmailAddress = temp.EmailAddress;
                location.Website = temp.Website;
                location.EmailAddress = temp.EmailAddress;
                location.PhoneNumber = temp.PhoneNumber;
                location.Tags = tags2;
                _locationService.Update(location);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AcceptLocationChangeRequest), ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/Request/DenyReportReview")]
        public IHttpActionResult DenyReportReview(int reportId)
        {
            try
            {
                var temp = _locationService.FindChangeRequest(reportId);
                temp.Status = (int)RequestStatus.Rejected;
                _locationService.UpdateChageRequest(temp);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(DenyReportReview), ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/Request/DenyLocationChangeRequest")]
        public IHttpActionResult DenyLocationChangeRequest(int requestId)
        {
            try
            {
                var temp = _locationService.FindChangeRequest(requestId);
                temp.Status = (int)RequestStatus.Rejected;
                _locationService.UpdateChageRequest(temp);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(DenyLocationChangeRequest), ex);
                return InternalServerError();
            }
        }


        public class UserPositionViewModels
        {
            public int Lat { get; set; }

            public double Long { get; set; }

            public int? LastLocationId { get; set; }

            public TimeSpan TimeOfLastLocation { get; set; }
        }

        private bool IsInRange(TimeSpan start, TimeSpan end, TimeSpan time)
        {
            return (time > start) && (time < end);
        }

        [Authorize, HttpPut]
        [Route("api/Location/UpdateUserPosition")]
        public async Task<IHttpActionResult> UpdateUserPosition(UserPositionViewModels userPosition)
        {
            try
            {
                User user = await CurrentUser();
                int? currentLocationId = null;
                var searchLoation = new GeoCoordinate(userPosition.Lat, userPosition.Long);//vị trí hiện tại
                if (userPosition.LastLocationId.HasValue)
                {
                    var lastLocation = _locationService.Find(userPosition.LastLocationId.Value);//last location
                    var tmpLocation = new GeoCoordinate(lastLocation.Latitude, lastLocation.Longitude);
                    if (searchLoation.GetDistanceTo(tmpLocation) < lastLocation.LocationRadius)
                    {
                        currentLocationId = lastLocation.Id;
                    }
                    else
                    {
                        var plans = _planService.GetPlans(user.Id);
                        foreach (var item in plans)
                        {
                            if (item.GroupId.HasValue)
                            {
                                if (item.Group.CreatorId == user.Id)
                                {
                                    foreach (var item2 in item.PlanLocations)
                                    {
                                        foreach (var item3 in item2.Location.BusinessHours.ToList())
                                        {
                                            if (IsInRange(item3.OpenTime, item3.CloseTime, DateTimeOffset.Now.TimeOfDay))
                                            {
                                                item2.Done = true;
                                                _planService.UpdatePlanLocation(item2);
                                                foreach (var item4 in item.PlanLocations.Select(__ => __.Location))
                                                {
                                                    item4.TotalStayCount = item4.TotalStayCount + 1;
                                                    item4.TotalTimeStay = item4.TotalTimeStay.Value + ((DateTimeOffset.Now.TimeOfDay - userPosition.TimeOfLastLocation)).Minutes;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item2 in item.PlanLocations)
                                {
                                    foreach (var item3 in item2.Location.BusinessHours.ToList())
                                    {
                                        if (IsInRange(item3.OpenTime, item3.CloseTime, DateTimeOffset.Now.TimeOfDay))
                                        {
                                            item2.Done = true;
                                            _planService.UpdatePlanLocation(item2);
                                            foreach (var item4 in item.PlanLocations.Select(__ => __.Location))
                                            {
                                                item4.TotalStayCount = item4.TotalStayCount + 1;
                                                item4.TotalTimeStay = item4.TotalTimeStay.Value + ((DateTimeOffset.Now.TimeOfDay - userPosition.TimeOfLastLocation)).Minutes;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                return Ok(currentLocationId);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UpdateUserPosition), ex);
                return InternalServerError();
            }
        }

        [HttpPut, Authorize]
        public async Task<IHttpActionResult> Edit(EditLocationViewModels data)
        {
            try
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    Location location = _locationService.Find(data.Id);
                    int userId = (await CurrentUser()).Id;

                    Photo primaryPhoto = ModelBuilder.ConvertToModels(data.PrimaryPhoto, userId);
                    IEnumerable<Photo> otherPhoto = ModelBuilder.ConvertToModels(data.OtherPhotos.AsEnumerable(), userId);

                    IEnumerable<BusinessHour> businessHours = ModelBuilder.ConvertToModels(data.Days);

                    bool result = _locationService.Edit(location, primaryPhoto, otherPhoto, businessHours, data.Tags);

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
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Edit), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize, Route("api/Location/AddReview")]
        public async Task<IHttpActionResult> Review(ReviewViewModels viewModels)
        {
            try
            {
                int userId = (await CurrentUser()).Id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                List<Photo> photos = new List<Photo>();
                foreach (var item in viewModels.Photos ?? new List<string>())
                {
                    var photo = new Photo
                    {
                        UserId = userId,
                        Path = item
                    };
                    await _photoService.Create2(photo);
                    photos.Add(photo);
                }
                _locationService.AddReview(new Review
                {
                    CreatorId = userId,
                    Title = viewModels.Title,
                    Description = viewModels.Description,
                    LocationId = viewModels.LocationId,
                    Rating = viewModels.Rating,
                    Photos = photos
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Review), ex);

                return InternalServerError(ex);
            }
        }

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