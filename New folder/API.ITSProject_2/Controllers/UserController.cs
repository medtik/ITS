namespace API.ITSProject_2.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Core.ObjectModels.Entities;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using Infrastructure.Identity.Models;
    using API.ITSProject_2.ViewModels;
    using Core.ObjectModels.Pagination;

    public class UserController : _BaseController
    {
        private readonly IUserService _userService;
        private readonly IPlanService _planService;

        public UserController(ILoggingService loggingService, IPagingService paggingService, 
            IIdentityService identityService, IUserService userService, IPlanService planService, IPhotoService photoService) : base(loggingService, paggingService, identityService, photoService)
        {
            this._userService = userService;
            this._planService = planService;
            
        }

        #region Get
        [HttpGet]
        [Authorize, Route("api/User/GetGroupInvitation")]
        public async Task<IHttpActionResult> GetGroupInvitation()
        {
            int userId = (await CurrentUser()).Id;

            var result = _userService.GetGroupInvitations(userId, _ => _.Group).ToList().OrderBy(_ => _.Status).ThenByDescending(__ => __.Id);

            return Ok(result.Select(_ => new
            {
                _.Id, _.Status, _.Message, _.GroupId,
                GroupName = _.Group.Name,
            }));
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult SearchUser(string searchValue = "", string nameSearchValue = "", string orderBy = "", int? pageIndex = 1, int? pageSize = 10)
        {
            try
            {
                IEnumerable<User> users = _userService.Search(_ => true);
                IList<SearchUserViewModels> usersSearch = new List<SearchUserViewModels>();
                foreach (var item in users)
                {
                    Account accountInfo = (_identityService.FindAccount(item.Id)).Data as Account;
                    usersSearch.Add(ModelBuilder.ConvertToSearchUserViewModels(item, accountInfo));
                }
                if (!string.IsNullOrEmpty(nameSearchValue))
                {
                    usersSearch = usersSearch.Where(_ => _.Name.Contains(nameSearchValue) || _.EmailAddress.Contains(nameSearchValue)).ToList();
                }//search client
                else
                {
                    usersSearch = usersSearch.Where(_ => string.IsNullOrEmpty(searchValue)
                                                    || _.Name.Contains(searchValue)
                                                    || _.PhoneNumber.Contains(searchValue)
                                                    || _.EmailAddress.Contains(searchValue)
                                                    || _.Address.Contains(searchValue)).ToList();
                }//search admin
                Pager<SearchUserViewModels> pager = null;

                orderBy = orderBy?.ToLower();
                switch (orderBy)
                {
                    case "name_asc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderBy(e => e.Name), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "name_desc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.Name), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "emailaddress_asc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderBy(e => e.EmailAddress), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "emailaddress_desc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.EmailAddress), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "phonenumber_asc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderBy(e => e.PhoneNumber), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "phonenumber_desc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.PhoneNumber), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "birthdate_asc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderBy(e => e.Birthdate), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "birthdate_desc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.Birthdate), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "address_asc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderBy(e => e.Address), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "address_desc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.Address), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "isbanned_asc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderBy(e => e.IsBanned), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    case "isbanned_desc":
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.IsBanned), pageIndex ?? 1, pageSize ?? 10);
                        break;
                    default:
                        //"content_desc"
                        pager = _paggingService.ToPagedList(usersSearch.AsQueryable().
                            OrderByDescending(e => e.Name), pageIndex ?? 1, pageSize ?? 10);
                        break;
                }
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
                    pager.CurrentList
                });
            }
            catch (Exception ex)
            {

                _loggingService.Write(GetType().Name, nameof(SearchUser), ex);
                return InternalServerError();
            }
            
        }

        [HttpGet]
        [Authorize, Route("api/CurrentUser")]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            try
            {
                User userInfo = await CurrentUser();
                Account accountInfo = (await _identityService.FindAccount(User.Identity.GetUserId())).Data as Account;

                return Ok(new UserViewModels
                {
                    Id = userInfo.Id,
                    Name = userInfo.FullName,
                    Address = userInfo.Address,
                    Birthdate = userInfo.Birthdate,
                    EmailAddress = accountInfo.Email,
                    PhoneNumber = accountInfo.PhoneNumber,
                    Avatar = userInfo.Avatar
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize, Route("api/GetGroups")]
        public async Task<IHttpActionResult> GetGroups()
        {
            IEnumerable<GroupViewModels> currentList = Enumerable.Empty<GroupViewModels>();
            try
            {
                int userId = (await CurrentUser()).Id;
                IEnumerable<Group> groups = _userService.GetGroups(userId);

                currentList = ModelBuilder.ConvertToGroupViewModels(groups, userId);

                return Ok(currentList);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetGroups), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize, Route("api/User/MyVisiblePlan")]
        public async Task<IHttpActionResult> MyVisiblePlan([FromUri]int? areaId = null)
        {
            List<MyPlanViewModels> currentList = new List<MyPlanViewModels>();
            try
            {
                int userId = (await CurrentUser()).Id;
                List<Plan> plans = _planService.GetPlans(userId).ToList().Where(_ => !_.IsPublic).ToList();
                IEnumerable<Group> groups = _userService.GetGroups(userId);

                if (!areaId.HasValue)
                {
                    currentList.AddRange(ModelBuilder.ConvertToMyPlan(plans, userId));

                    foreach (var item in groups)
                    {
                        plans = _planService.GetGroupPlans(item.Id).Where(_ => !_.IsPublic).ToList();

                        currentList.AddRange(ModelBuilder.ConvertToMyPlan(plans, userId));
                    }

                    return Ok(currentList.GroupBy(_ => _.GroupName));
                }
                else
                {
                    var tmpMyPlan = ModelBuilder.ConvertToMyPlan(plans, userId);

                    foreach (var item in tmpMyPlan)
                    {
                        if (item.AreaId == areaId)
                        {
                            currentList.Add(item);
                        }
                    }

                    foreach (var item in groups)
                    {
                        plans = _planService.GetGroupPlans(item.Id).Where(_ => !_.IsPublic).ToList();

                        foreach (var ele in plans)
                        {
                            if (ele.AreaId == areaId)
                            {
                                currentList.Add(ModelBuilder.ConvertToMyPlan(ele, userId));
                            }
                        }
                    }


                    return Ok(currentList.GroupBy(_ => _.GroupName));
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(MyVisiblePlan), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Put
        public static int DateDiffYears(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var yr = endDate.Year - startDate.Year - 1 +
                     (endDate.Month >= startDate.Month && endDate.Day >= startDate.Day ? 1 : 0);
            return yr < 0 ? 0 : yr;
        }

        [HttpPut]
        [Authorize, Route("api/User/Update")]
        public async Task<IHttpActionResult> UpdateUser(UpdateUserViewModels viewModels)
        {
            try
            {
                var year = DateDiffYears(viewModels.Birthdate, DateTimeOffset.Now);
                if (year <= 10)
                    ModelState.AddModelError(string.Empty, "Ngày sinh phải cách hiện tại 10 năm");
                if (!ModelState.IsValid)
                    return BadRequest();
                User userInfo = await CurrentUser();
                Account accountInfo = (await _identityService.FindAccount(User.Identity.GetUserId())).Data as Account;

                userInfo.Address = viewModels.Address;
                userInfo.Avatar = viewModels.Avatar;
                userInfo.Birthdate = viewModels.Birthdate;
                userInfo.FullName = viewModels.Name;
                accountInfo.PhoneNumber = viewModels.PhoneNumber;

                bool user = _userService.Update(userInfo);
                _identityService.SaveChanges();
                if (user)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UpdateUser), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize, Route("api/User/SetMobileToken")]
        public async Task<IHttpActionResult> SetMobileToken(string token)
        {
            try
            {
                bool result = _userService.SetMobileToken(await CurrentUser(), token);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(SetMobileToken), ex);

                return InternalServerError(ex);
            }
        }
        #endregion
    }
}