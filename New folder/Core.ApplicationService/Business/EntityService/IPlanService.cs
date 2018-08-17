using System.Collections.Generic;

namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;

    public interface IPlanService
    {
        IQueryable<Plan> GetFeaturedTrip();

        IQueryable<Plan> GetPlans(int userId);

        IQueryable<Plan> GetGroupPlans(int groupId);

        Plan Find(int id, params Expression<Func<Plan, object>>[] includes);

        bool Create(Plan plan);

        bool Create(Note planNote);

        bool Update(Plan plan);

        bool Create(LocationSuggestion locationSuggestion);

        bool Delete(Plan plan);

        bool UpdatePlanLocation(PlanLocation planLocation);

        bool UpdatePlanNote(Note note);

        PlanLocation FindPlanLocation(int id);

        Note FindNote(int id);

        bool AddLocationToPlan(PlanLocation planLocation);

        bool DeletePlanLocation(int planId, int locationId);

        bool DeleteNote(int noteId);

        Plan PublicPlan(int planId);

        Plan ClonePlan(int planId);

        Plan CreateSuggestedPlan(Plan plan, List<Location> locations);
    }
}