namespace Service.Implement.Entity
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ObjectModels.Entities.EnumType;

    public class LocationService : _BaseService<Location>, ILocationService 
    {
        private readonly IRepository<Photo> _photoRepository;
        private readonly IRepository<LocationSuggestion> _locationSuggestionRepository;
        private readonly IRepository<LocationPhoto> _locationPhotoRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<PlanLocation> _planLocationRepository;
        private readonly IRepository<BusinessHour> _businessHourRepository;
        private readonly IRepository<Review> _reviewRepository;

        public LocationService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
            _tagRepository = unitOfWork.GetRepository<Tag>();
            _photoRepository = unitOfWork.GetRepository<Photo>();
            _locationPhotoRepository = unitOfWork.GetRepository<LocationPhoto>();
            _businessHourRepository = unitOfWork.GetRepository<BusinessHour>();
            _locationSuggestionRepository = unitOfWork.GetRepository<LocationSuggestion>();
            _planRepository = unitOfWork.GetRepository<Plan>();
            _planLocationRepository = unitOfWork.GetRepository<PlanLocation>();
            _reviewRepository = unitOfWork.GetRepository<Review>();
        }

        public bool AcceptStatusLocationSuggestion(int suggestionId)
        {
            try
            {
                LocationSuggestion suggest = _locationSuggestionRepository.Get(_ => _.Id == suggestionId, _ => _.Locations);
                
                suggest.Status = RequestStatus.Approved;

                _locationSuggestionRepository.Update(suggest);
                ICollection<Location> locations = new List<Location>();
                var plan = _planRepository.Get(_ => _.Id == suggest.PlanId, _ => _.PlanLocations);
                
                foreach (var item in suggest.Locations)
                {
                    _planLocationRepository.Create(new PlanLocation
                    {
                        PlanId = suggest.PlanId,
                        LocationId = item.Id,
                        Done = false,
                        PlanDay = suggest.PlanDay ?? 0
                    });
                }

                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(AcceptStatusLocationSuggestion), ex);

                return false;
            }
        }

        public bool AddReview(Review review)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    _reviewRepository.Create(review);

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

        public bool Create(Location location, Photo primaryPhoto, IEnumerable<Photo> photos, IEnumerable<BusinessHour> businessHours, int[] tagList)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    base.Create(location);

                    List<Tag> tags = new List<Tag>();
                    tagList.ToList().ForEach(_ =>
                    {
                        var ele = _tagRepository.Get(__ => __.Id == _);
                        tags.Add(ele);
                    });
                    location.Tags = tags;
                    base.Update(location);

                    _photoRepository.Create(primaryPhoto);
                    _unitOfWork.SaveChanges();
                    _locationPhotoRepository.Create(new LocationPhoto
                    {
                        LocationId = location.Id,
                        PhotoId = primaryPhoto.Id,
                        IsPrimary = true
                    });

                    photos.ToList().ForEach(_ =>
                    {
                        _photoRepository.Create(_);
                        _unitOfWork.SaveChanges();
                        _locationPhotoRepository.Create(new LocationPhoto
                        {
                            LocationId = location.Id,
                            PhotoId = _.Id,
                            IsPrimary = false
                        });
                    });

                    businessHours.ToList().ForEach(_ =>
                    {
                        _businessHourRepository.Create(new BusinessHour
                        {
                            LocationId = location.Id,
                            Day = _.Day,
                            OpenTime = _.OpenTime,
                            CloseTime = _.CloseTime
                        });
                    });

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

        public bool Delete(int locationId)
        {
            Location entity = Find(locationId);
            entity.IsDelete = true;
            return Update(entity);
        }

        public Location Find(int locationId, params Expression<Func<Location, object>>[] includes)
        {
            Location location = null;
            try
            {
                location = _repository.GetAsQueryable(_ => _.Id == locationId, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return location;
        }

        public Location Find(string name, params Expression<Func<Location, object>>[] includes)
        {
            Location location = null;
            try
            {
                location = _repository.GetAsQueryable(_ => _.Name == name, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return location;
        }

        public IQueryable<Location> GetAll(params Expression<Func<Location, object>>[] includes)
        {
            IQueryable<Location> locations = null;
            try
            {
                locations = _repository.GetAllAsQueryable(includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(GetAll), ex);
            }

            return locations ?? Enumerable.Empty<Location>().AsQueryable();
        }

        public IEnumerable<string> GetCategories()
        {
            return GetAll().Select(_ => _.Category);
        }

        public bool RejectStatusLocationSuggestion(int suggestionId)
        {
            try
            {
                LocationSuggestion suggest = _locationSuggestionRepository.Get(_ => _.Id == suggestionId);

                suggest.Status = RequestStatus.Rejected;
                _locationSuggestionRepository.Update(suggest);

                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(RejectStatusLocationSuggestion), ex);

                return false;
            }
        }

        public IQueryable<Location> Search(Expression<Func<Location, bool>> searchValue, params Expression<Func<Location, object>>[] includes)
        {
            IQueryable<Location> locations = null;
            try
            {
                locations = _repository.SearchAsQueryable(searchValue, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Search), ex);
            }

            return locations ?? Enumerable.Empty<Location>().AsQueryable();
        }
    }
}