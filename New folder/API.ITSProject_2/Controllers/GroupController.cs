namespace API.ITSProject_2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Entities.EnumType;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using Infrastructure.Identity.Models;
    using API.ITSProject_2.ViewModels;

    public class GroupController : _BaseController
    {
        private HttpClient client;
        private readonly IGroupService _groupService;
        private readonly IPlanService _planService;
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;

        public GroupController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService, IGroupService groupService, IPlanService planService,
            IUserService userService, IPhotoService photoService, ILocationService locationService) : base(loggingService, paggingService, identityService, photoService)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://exp.host");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            this._groupService = groupService;
            this._planService = planService;
            this._userService = userService;
            this._locationService = locationService;
        }

        #region Get
        [HttpGet]
        [Authorize, Route("api/Group/GroupInvitation")]
        public async Task<IHttpActionResult> GetGroupInvitation()
        {
            try
            {
                return Ok(_groupService.GetGroupInvitations((await CurrentUser()).Id).
                    OrderByDescending(_ => _.Status).ThenByDescending(__ => __.Id).
                    Select(___ => new
                    {
                        ___.Id,
                        ___.Message,
                        GroupName = ___.Group.Name,
                        OwnerName = ___.Group.Creator.FullName
                    }));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetGroupInvitation), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Group/GetLocationSuggestions")]
        [Authorize]
        public async Task<IHttpActionResult> GetLocationSuggestion()
        {
            try
            {
                int userId = (await CurrentUser()).Id;

                List<object> temp = new List<object>();

                var list = _locationService.GetLocationSuggestion(userId);

                foreach (var item in list)
                {

                    temp.Add(new
                    {
                        item.Id,
                        item.Comment,
                        item.Status,
                        item.PlanId,
                        item.PlanDay,
                        item.Plan.Name,
                        Locations = item.Locations.Select(_ => (_.Id, _.Name))
                    });
                }
                return Ok(temp);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetLocationSuggestion), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize, Route("api/Group/Details")]
        public async Task<IHttpActionResult> Details([FromUri]int id)
        {
            try
            {
                int userId = (await CurrentUser()).Id;
                Group group = _groupService.Find(id,
                    _ => _.Creator,
                    _ => _.Members,
                    _ => _.Plans.Select(__ => __.PlanLocations.Select(___ => ___.Location).Select(____ => ____.Photos.Select(_____ => _____.Photo))),
                    _ => _.Plans.Select(__ => __.PlanLocations.Select(___ => ___.Location).Select(____ => ____.Reviews)),
                    _ => _.Plans.Select(__ => __.Area));

                if (group == null)
                    return BadRequest("Not found");

                GroupDetailViewModels result = ModelBuilder.ConvertToGroupDetailViewModel(group, userId);

                Account userInfo = (_identityService.FindAccount(int.Parse(result.Creator.EmailAddress)).Data as Account);
                result.Creator.EmailAddress = userInfo.Email;
                result.Creator.PhoneNumber = userInfo.PhoneNumber;

                result.Users.ToList().ForEach(_ =>
                {
                    userInfo = (_identityService.FindAccount(int.Parse(_.EmailAddress)).Data as Account);
                    if (userInfo != null)
                    {
                        _.PhoneNumber = userInfo.PhoneNumber;
                        _.EmailAddress = userInfo.Email;
                    }
                    else
                    {
                        _.PhoneNumber = null;
                        _.EmailAddress = null;
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Details), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> Create(CreatedGroupViewModels models)
        {
            try
            {
                User user = await CurrentUser();
                Group group = new Group
                {
                    CreatorId = user.Id,
                    Name = models.Name,
                    Members = new List<Member>()
                };
                group.Members.Add(user as Member);
                bool result = _groupService.Create(group);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        [Authorize]
        [Route("api/Group/RemovePlan")]
        public async Task<IHttpActionResult> RemovePlanToGroup(int planId)
        {
            try
            {
                Plan plan = _planService.Find(planId, _ => _.Group);
                if (plan != null)
                {
                    bool isAuthor = plan.Group.CreatorId == (await CurrentUser()).Id;
                    if (isAuthor)
                    {
                        bool result = _planService.Delete(plan);

                        if (result)
                            return Ok();
                    }
                    else
                        return Unauthorized();
                    
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(RemovePlanToGroup), ex);

                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                int userId = (await CurrentUser()).Id;

                Group group = _groupService.Find(id);
                if (group == null)
                    return BadRequest();

                if (group.CreatorId == userId)
                {
                    bool result = _groupService.Delete(group);

                    if (result)
                        return Ok();
                }
                else
                    return Unauthorized();

                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Delete), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        [Authorize, Route("api/Group/SavePlan")]
        public async Task<IHttpActionResult> SavePublicPlan(int planId, int? groupId = null)
        {
            try
            {
                Plan plan = _planService.ClonePlan(planId);
                if (plan == null)
                    return BadRequest("Plan doesn't existed");
                plan.IsPublic = false;

                if (groupId.HasValue)
                {
                    var group = _groupService.Find(groupId.Value);

                    if (group == null)
                        return BadRequest("Group doesn't existed");

                    plan.GroupId = groupId.Value;
                    plan.MemberId = null;

                    _planService.Create(plan);
                }//add to group
                else
                {
                    int userId = (await CurrentUser()).Id;

                    plan.MemberId = userId;
                    plan.GroupId = null;

                    _planService.Create(plan);
                }//add to personal

                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(SavePublicPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize, Route("api/Group/DenyGroupInvitation")]
        public IHttpActionResult DenyGroupInvitation(int groupInvitationId)
        {
            try
            {
                bool result = false;
                GroupInvitation groupInvitation = _groupService.GetGroupInvitation(groupInvitationId);

                if (groupInvitation != null)
                {
                    groupInvitation.Status = RequestStatus.Rejected;
                    result = _groupService.UpdateGroupInvitation(groupInvitation);
                }

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Invitation), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize, Route("api/Group/AcceptGroupInvitation")]
        public IHttpActionResult AcceptGroupInvitation(int groupInvitationId)
        {
            try
            {
                bool result = false;
                GroupInvitation groupInvitation = _groupService.GetGroupInvitation(groupInvitationId);

                if (groupInvitation != null)
                {
                    result = _groupService.AddUser(groupInvitation.UserId, groupInvitation.GroupId);
                }

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Invitation), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize, Route("api/Group/UserInvitation")]
        public async Task<IHttpActionResult> Invitation(UserInvitationViewModels userInvitation)
        {
            try
            {
                bool isExited = _groupService.IsExisted(userInvitation.UserId, userInvitation.GroupId);
                if (isExited)
                {
                    return BadRequest("Người này hiện đã là thành viên của nhóm");
                }

                string sender = (await CurrentUser()).FullName;
                string groupName = _groupService.Find(userInvitation.GroupId).Name;

                bool result = _groupService.
                    GroupInvitations(userInvitation.UserId, userInvitation.GroupId, userInvitation.Message);


                if (result)
                {
                    var user = _userService.Find(userInvitation.UserId);
                    var content = new
                    {
                        to = $"{user.MobileToken}",
                        title = $"{sender} đã mời bạn vào {groupName}",
                        body = $"{userInvitation.Message}",
                        data = new
                        {
                            type = "GroupInvitation"
                        }
                    };
                    var temp = JsonConvert.SerializeObject(content);
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("/--/api/v2/push/send", content);

                    return Ok();
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Invitation), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Group/RemoveUser")]
        public IHttpActionResult RemoveUser(UserRemoveViewModels userRemove)
        {
            try
            {
                bool result = _groupService.
                    UserRemoved(userRemove.UserId, userRemove.GroupId);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(RemoveUser), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Group/AddPlan")]
        public IHttpActionResult AddPlanToGroup(int planId, int groupId)
        {
            try
            {
                bool result = _groupService.AddPlanToGroup(planId, groupId);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddPlanToGroup), ex);

                return InternalServerError(ex);
            }
        }
        #endregion
    }
}