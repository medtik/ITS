using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Service.Implement.Entity
{
    using System;
    using System.Linq.Expressions;
    using System.Transactions;
    using System.Linq;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ObjectModels.Entities.Helper;

    public class PlanService : _BaseService<Plan>, IPlanService
    {
        private readonly IRepository<PlanLocation> _planLocationRepository;
        private readonly IRepository<LocationSuggestion> _locationSuggestionRepository;
        private readonly IRepository<Note> _noteRepository;

        public PlanService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
            _planLocationRepository = unitOfWork.GetRepository<PlanLocation>();
            _noteRepository = unitOfWork.GetRepository<Note>();
            _locationSuggestionRepository = unitOfWork.GetRepository<LocationSuggestion>();
        }

        public bool UpdatePlanLocation(PlanLocation entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _planLocationRepository.Update(entity);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UpdatePlanLocation), ex);
                return false;
            }
        }

        public bool AddLocationToPlan(PlanLocation planLocation)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    _planLocationRepository.Create(planLocation);
                    _unitOfWork.SaveChanges();

                    scope.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AddLocationToPlan), ex);

                return false;
            }
        }

        public bool DeletePlanLocation(int planId, int locationId)
        {
            try
            {
                var planLocation = _planLocationRepository.Get(_ => _.PlanId == planId && _.LocationId == locationId);

                if (planLocation == null)
                    return false;

                _planLocationRepository.Delete(planLocation);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(DeletePlanLocation), ex);
                return false;
            }
        }

        public Plan Find(int id, params Expression<Func<Plan, object>>[] includes)
            => _repository.Get(_ => _.Id == id, includes);

        public IQueryable<Plan> GetFeaturedTrip()
            => _repository.SearchAsQueryable(_ => _.IsPublic, _ => _.Voters, _ => _.PlanLocations
                            .Select(__ => __.Location)
                            .Select(___ => ___.Photos.Select(____ => ____.Photo)), _ => _.Area)
                            .OrderByDescending(_ => _.Voters.Count()).Take(10);

        public IQueryable<Plan> GetGroupPlans(int groupId)
            => _repository.SearchAsQueryable(_ => _.GroupId == groupId,
                    _ =>
                        _.PlanLocations.Select(__ => __.Location).Select(___ => ___.Photos.Select(_____ => _____.Photo)),
                        _ => _.PlanLocations.Select(__ => __.Location.Reviews),
                        _ => _.Notes,
                        _ => _.Area, _ => _.Group);

        public IQueryable<Plan> GetPlans(int userId)
            => _repository.SearchAsQueryable(_ => _.MemberId == userId,
                _ =>
                    _.PlanLocations.Select(__ => __.Location).Select(___ => ___.Photos.Select(_____ => _____.Photo)),
                    _ => _.PlanLocations.Select(__ => __.Location.Reviews),
                    _ => _.Notes,
                    _ => _.Area, _ => _.Group);

        public PlanLocation FindPlanLocation(int id)
            => _planLocationRepository.Get(_ => _.Id == id);

        public Note FindNote(int id)
        {
            return _noteRepository.Get(_ => _.Id == id);
        }

        public bool UpdatePlanNote(Note note)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _noteRepository.Update(note);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UpdatePlanNote), ex);
                return false;
            }
        }

        public bool Create(Note planNote)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _noteRepository.Create(planNote);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return false;
            }
        }

        public bool DeleteNote(int noteId)
        {
            try
            {
                var entity = FindNote(noteId);
                if (entity != null)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        _noteRepository.Delete(entity);
                        _unitOfWork.SaveChanges();

                        scope.Complete();
                        return true;
                    }//end scope
                }
                return false;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(DeleteNote), ex);
                return false;
            }
        }

        public bool Create(LocationSuggestion locationSuggestion)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _locationSuggestionRepository.Create(locationSuggestion);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return false;
            }
        }

        public Plan PublicPlan(int planId)
        {
            var plan = Find(planId, _ => _.Notes, _ => _.PlanLocations);
            var cloner = Clone.CloneObject(plan);
            cloner.IsPublic = true;
            foreach (var item in cloner.Notes)
            {
                item.Done = false;
            }

            foreach (var item in cloner.PlanLocations)
            {
                item.Done = false;
            }

            Create(cloner);

            return cloner;
        }

        public Plan ClonePlan(int planId)
        {
            var plan = Find(planId, _ => _.Notes, _ => _.PlanLocations);
            return Clone.CloneObject(plan);
        }

        public Plan CreateSuggestedPlan(Plan plan, List<Location> locations)
        {
            try
            {
                var diffDays = (plan.EndDate - plan.StartDate).TotalDays;
                for (int i = 1; i <= diffDays; i++)
                {
                    DateTimeOffset currentDate = plan.StartDate.AddDays(i);

                    PolulateNecessityLocations(plan, locations, currentDate);
                    PolulateEntertainmentLocations(plan, locations, currentDate);
                }
                _repository.Create(plan);
                _unitOfWork.SaveChanges();
                return plan;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(CreateSuggestedPlan), ex);
                return null;
            }           
        }

        private void PolulateNecessityLocations(Plan plan, List<Location> locations, DateTimeOffset currentDate)
        {
            throw new NotImplementedException();
        }
        
        private void PolulateEntertainmentLocations(Plan plan, List<Location> locations,DateTimeOffset currentDate)
        {
            throw new NotImplementedException();
        }
    }
}
