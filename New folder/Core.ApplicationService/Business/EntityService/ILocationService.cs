namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;

    public interface ILocationService
    {
        IEnumerable<LocationSuggestion> GetLocationSuggestion(int userId);

        bool AddReview(Review review);

        bool Create(Location location, Photo priamryPhoto, IEnumerable<Photo> photos, IEnumerable<BusinessHour> businessHours, int[] tagList);

        bool Edit(Location location, Photo priamryPhoto, IEnumerable<Photo> photos, IEnumerable<BusinessHour> businessHours, int[] tagList);

        bool Delete(int locationId);

        bool AcceptStatusLocationSuggestion(int suggestionId);

        bool RejectStatusLocationSuggestion(int suggestionId);

        IEnumerable<string> GetCategories();

        Location Find(int locationId, params Expression<Func<Location, object>>[] includes);

        Location Find(string name, params Expression<Func<Location, object>>[] includes);

        IQueryable<Location> GetAll(params Expression<Func<Location, object>>[] includes);

        IQueryable<Location> Search(Expression<Func<Location, bool>> searchValue, params Expression<Func<Location, object>>[] includes);
    }
}