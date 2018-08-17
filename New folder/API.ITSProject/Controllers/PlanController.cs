namespace API.ITSProject.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Entities.EnumType;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using Core.ApplicationService.Business.EntityService;
    using API.ITSProject.ViewModels;

    public class PlanController : _BaseController
    {
        private readonly IPlanService _planService;
        private readonly ILocationService _locationService;

        public PlanController(ILoggingService loggingService, IPagingService paggingService,
            IIdentityService identityService, IPlanService planService, ILocationService locationService, IPhotoService photoService) : base(loggingService, paggingService, identityService, photoService)
        {
            this._planService = planService;
            this._locationService = locationService;
        }

        #region Get
        [HttpGet]
        [Route("api/Plan/Details")]
        public async Task<IHttpActionResult> Details([FromUri]int id)
        {
            try
            {
                int userId = (await CurrentUser()).Id;
                Plan plan = _planService.Find(id, _ =>
                    _.PlanLocations.Select(__ => __.Location).Select(___ => ___.Photos.Select(_____ => _____.Photo)),
                    _ => _.PlanLocations.Select(__ => __.Location.Reviews),
                    _ => _.Notes, _ => _.Area);

                if (plan == null)
                    return BadRequest("Not found");
                else
                {
                    var temp = ModelBuilder.ConvertToPlanDetailViewModels(plan);
                    temp.IsOwner = temp.MemberId == userId;
                    return Ok(temp);
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Details), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/FeaturedTrip")]
        public IHttpActionResult FeaturedTrip()
        {
            try
            {
                IQueryable<Plan> planList = _planService.GetFeaturedTrip();

                return Ok(ModelBuilder.ConvertToFeaturedPlanViewModels(planList));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(FeaturedTrip), ex);

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/MyPlans")]
        public async Task<IHttpActionResult> GetPlans()
        {
            try
            {
                int userId = (await CurrentUser()).Id;
                IQueryable<Plan> plans = _planService.GetPlans(userId);

                return Ok(ModelBuilder.ConvertToMyPlan(plans, userId));
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetPlans), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [Route("api/Plan/AddSuggestion")]
        public IHttpActionResult AddSuggestionToPlan(LocationSuggestionViewModels locationSuggestion)
        {
            try
            {
                int userId = 1;
                ICollection<Location> locations = new List<Location>();

                foreach (var item in locationSuggestion.LocationIds ?? new List<int>())
                {
                    var location = _locationService.Find(item);
                    if (location != null)
                        locations.Add(location);
                    else
                        return BadRequest("Location not existed");
                }
                bool result = _planService.Create(new LocationSuggestion
                {
                    Comment = locationSuggestion.Comment,
                    Locations = locations,//need to edit
                    UserId = userId,
                    PlanId = locationSuggestion.PlanId,
                    Status = RequestStatus.NotYet,
                    PlanDay = locationSuggestion.PlanDay
                });

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddNoteToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Plan/AddNote")]
        public IHttpActionResult AddNoteToPlan(CreatePlanNoteViewModles planNote)
        {
            try
            {
                Note note = ModelBuilder.ConvertToModels(planNote);

                if (!planNote.Index.HasValue)
                {
                    Plan plan = _planService.Find(planNote.PlanId, _ => _.Notes, _ => _.PlanLocations);

                    IEnumerable<PlanLocation> planLocations = plan.PlanLocations;
                    IEnumerable<Note> planNotes = plan.Notes;

                    int maxLocationIndex = planLocations.OrderByDescending(_ => _.Index).FirstOrDefault() != null ? planLocations.OrderByDescending(_ => _.Index).FirstOrDefault().Index : 0;
                    int maxPlanIndex = planNotes.OrderByDescending(_ => _.Index).FirstOrDefault() != null ? planNotes.OrderByDescending(_ => _.Index).FirstOrDefault().Index : 0;

                    note.Index = (maxLocationIndex > maxPlanIndex ? maxLocationIndex : maxPlanIndex) + 1;
                }

                bool result = _planService.Create(note);
                if (result)
                    return Ok(note.Id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddNoteToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> Post(CreatePlanViewModels plan)
        {
            try
            {
                int userId = (await CurrentUser()).Id;

                Plan tmpPlan = ModelBuilder.ConvertToModels(plan);
                tmpPlan.CreatorId = userId;
                tmpPlan.MemberId = userId;

                bool result = _planService.Create(tmpPlan);
                if (result)
                {
                    return Ok(tmpPlan.Id);
                }
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
        [Route("api/Plan/UpdatePlan")]
        public IHttpActionResult UpdatePlan(UpdatePlanViewModels viewModels)
        {
            try
            {
                Plan plan = _planService.Find(viewModels.Id);
                if (plan == null)
                    return BadRequest();
                plan.StartDate = viewModels.StartDate;
                plan.EndDate = viewModels.EndDate;
                plan.Name = viewModels.Name;

                _planService.Update(plan);
                return Ok();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UpdatePlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/PublicPlan")]
        public IHttpActionResult PublicPlan(int planId)
        {
            try
            {
                var temp = _planService.PublicPlan(planId);

                return Ok(temp.Id);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(PublicPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/EditIndexPlanLocationAndNote")]
        public IHttpActionResult EditIndexPlanLocationAndNote(UpdateIndexPlanLocationAndNote updateIndexPlanLocationAndNote)
        {
            try
            {
                bool result = false;

                foreach (var item in updateIndexPlanLocationAndNote.PlanLocation)
                {
                    var planLocation = _planService.FindPlanLocation(item.Id);
                    planLocation.Index = item.Index;
                    planLocation.PlanDay = item.PlanDay;

                    result = _planService.UpdatePlanLocation(planLocation);
                }

                if (result)
                {
                    foreach (var item in updateIndexPlanLocationAndNote.PlanNotes)
                    {
                        var planNote = _planService.FindNote(item.Id);
                        planNote.Index = item.Index;
                        planNote.PlanDay = item.PlanDay;

                        result = _planService.UpdatePlanNote(planNote);
                    }
                }
                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(EditIndexPlanLocationAndNote), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/SwitchStatusPlanNote")]
        public IHttpActionResult SwitchStatusPlanNote(int id)
        {
            try
            {
                Note planNote = _planService.FindNote(id);

                planNote.Done = !planNote.Done;
                bool result = _planService.UpdatePlanNote(planNote);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/SwitchStatusPlanLocation")]
        public IHttpActionResult SwitchStatusPlanLocation(int id)
        {
            try
            {
                PlanLocation planLocation = _planService.FindPlanLocation(id);

                planLocation.Done = !planLocation.Done;
                bool result = _planService.UpdatePlanLocation(planLocation);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/AddLocations")]
        public IHttpActionResult AddLocationToPlan(LocationInPlanViewModels planLocation)
        {
            try
            {
                PlanLocation location = ModelBuilder.ConvertToModels(planLocation);

                

                if (!planLocation.Index.HasValue)
                {
                    Plan plan = _planService.Find(planLocation.PlanId, _ => _.PlanLocations, _ => _.Notes);

                    IEnumerable<PlanLocation> planLocations = plan.PlanLocations;
                    IEnumerable<Note> planNotes = plan.Notes;

                    int maxLocationIndex = planLocations.OrderByDescending(_ => _.Index).FirstOrDefault() != null ? planLocations.OrderByDescending(_ => _.Index).FirstOrDefault().Index : 0;
                    int maxPlanIndex = planNotes.OrderByDescending(_ => _.Index).FirstOrDefault() != null ? planNotes.OrderByDescending(_ => _.Index).FirstOrDefault().Index : 0;

                    location.Index = (maxLocationIndex > maxPlanIndex ? maxLocationIndex : maxPlanIndex) + 1;
                }//end if identity max index

                bool result = _planService.AddLocationToPlan(location);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/UpdateIndexPlanLocation")]
        public IHttpActionResult UpdatePlanLocation(UpdatePlanLocationViewModels planLocation)
        {
            try
            {
                Note updateNote = _planService.FindNote(planLocation.PlanLocationId);
                updateNote.Index = planLocation.Index;
                bool result = _planService.UpdatePlanNote(updateNote);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/Plan/UpdateIndexPlanNote")]
        public IHttpActionResult UpdatePlanNote(UpdatePlanNoteViewModels noteLocation)
        {
            try
            {
                PlanLocation updatePlanLocation = _planService.FindPlanLocation(noteLocation.NoteId);
                updatePlanLocation.Index = noteLocation.Index;
                bool result = _planService.UpdatePlanLocation(updatePlanLocation);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        [Route("api/Plan/DeleteNote")]
        public IHttpActionResult RemoveNote(int noteId)
        {
            try
            {
                bool result = _planService.DeleteNote(noteId);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/Plan/AddLocations")]
        public IHttpActionResult RemoveLocationToPlan(int planId, int locationId)
        {
            try
            {
                bool result = _planService.DeletePlanLocation(planId, locationId);

                if (result)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> Remove(int planId)
        {
            try
            {
                int userId = (await CurrentUser()).Id;

                Plan plan = _planService.Find(planId);
                if (plan != null)
                {
                    if (userId == plan.CreatorId)
                    {
                        bool result = _planService.Delete(plan);

                        if (result)
                            return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Remove), ex);

                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
