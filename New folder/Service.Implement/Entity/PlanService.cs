using System.Collections.Generic;
using System.Device.Location;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private readonly IRepository<Location> _locationRepository;
        private readonly HttpClient _client;


        private enum NessecityType
        {
            Hotel,
            Breakfast,
            Lunch,
            Dinner
        }


        public PlanService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
            _planLocationRepository = unitOfWork.GetRepository<PlanLocation>();
            _noteRepository = unitOfWork.GetRepository<Note>();
            _locationSuggestionRepository = unitOfWork.GetRepository<LocationSuggestion>();
            _locationRepository = unitOfWork.GetRepository<Location>();

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                } //end scope
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
                    .Select(___ => ___.Photos.Select(____ => ____.Photo)), _ => _.Area, _ => _.Creator)
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
                } //end scope
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
                } //end scope
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
                    } //end scope
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
                } //end scope
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
            cloner.MemberId = null;
            cloner.GroupId = null;
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

        public async Task<Plan> CreateSuggestedPlan(Plan plan, List<TreeViewModels> locations)
        {
            _loggingService.AddSentryBreadCrum("CreateSuggestedPlan", data: new Dictionary<string, object>
            {
                ["plan"] = plan,
                ["locations count"] = locations.Count
            });
            try
            {
                var planLocations = new List<PlanLocation>();
                var notes = new List<Note>();

                var diffDays = (plan.EndDate - plan.StartDate).TotalDays + 1;
                for (int i = 1; i <= diffDays; i++)
                {
                    DateTimeOffset currentDate = plan.StartDate.AddDays(i);
                    PolulateNecessityLocations(plan, locations, currentDate, out var nessecityLocationMap, i);
                    var locationsWithRouteList = await PolulateEntertainmentLocations(
                        plan,
                        locations,
                        currentDate,
                        nessecityLocationMap
                    );


                    int index = 0;
                    var hotel = nessecityLocationMap[NessecityType.Hotel];
                    planLocations.Add(new PlanLocation
                    {
                        PlanDay = i,
                        Index = index++,
                        Done = false,
                        Location = hotel
                    });


                    #region Buiding notes
                    
                    JObject fromHotelJObject = await FetchGoogleRoute(
                        nessecityLocationMap[NessecityType.Hotel],
                        nessecityLocationMap[NessecityType.Breakfast],
                        null
                    );

                    StringBuilder builder = new StringBuilder();
                    
                    JArray fromHotelSteps = fromHotelJObject["routes"][0]["legs"][0]["steps"].Value<JArray>();
                    builder.Append("Xuất phát từ khách sạn");                        
                    builder.Append("<br/>");
                    foreach (JToken step in fromHotelSteps)
                    {
                        builder.Append(step["html_instructions"]);
                        builder.Append("<br/>");
                    }
                    notes.Add(new Note
                    {
                        Index = index++,
                        PlanDay = i,
                        Title = $"Cách đi từ {hotel.Name} đến {nessecityLocationMap[NessecityType.Breakfast].Name}",
                        Content = builder.ToString()
                    });
                    
                    planLocations.Add(new PlanLocation
                    {
                        PlanDay = i,
                        Index = index++,
                        Done = false,
                        Location = nessecityLocationMap[NessecityType.Breakfast]
                    });


                    Location lastLocation = null;
                    foreach (var locationsWithRoute in locationsWithRouteList)
                    {
                       
                        JArray steps = locationsWithRoute.Key["steps"].Value<JArray>();
                        builder.Clear();
                        foreach (JToken step in steps)
                        {
                            builder.Append(step["html_instructions"]);
                            builder.Append("<br/>");
                        }
                        notes.Add(new Note
                            {
                                Index = index++,
                                PlanDay = i,
                                Title = $"Cách đi từ {locationsWithRoute.Value.Key.Name} đến {locationsWithRoute.Value.Value.Name}",
                                Content = builder.ToString()
                            });
                        
                        planLocations.Add(new PlanLocation
                        {
                            PlanDay = i,
                            Index = index++,
                            Done = false,
                            Location = locationsWithRoute.Value.Value
                        });
                        lastLocation = locationsWithRoute.Value.Value;
                    }
                    
                    
                    JObject toHotelStepsJObject = await FetchGoogleRoute(
                        lastLocation,
                        nessecityLocationMap[NessecityType.Hotel],
                        null
                    );
                    JArray toHotelSteps = toHotelStepsJObject["routes"][0]["legs"][0]["steps"].Value<JArray>();    
                    builder.Clear();
                    foreach (JToken step in toHotelSteps)
                    {
                        builder.Append(step["html_instructions"]);
                        builder.Append("<br/>");
                    }

                    Note toHotel = new Note
                    {
                        Index = index++,
                        PlanDay = i,
                        Title = $"Cách đi từ {lastLocation.Name} đến {hotel.Name}",
                        Content = builder.ToString()
                    };
                    
                    planLocations.Add(new PlanLocation
                    {
                        PlanDay = i,
                        Index = index++,
                        Done = false,
                        Location = nessecityLocationMap[NessecityType.Hotel]
                    });
                    
                    notes.Add(toHotel);
                    #endregion

                }

                _repository.Create(plan);
                _unitOfWork.SaveChanges();

                planLocations.ForEach(planLocation =>
                {
                    planLocation.PlanId = plan.Id;
                    _planLocationRepository.Create(planLocation);
                });
                notes.ForEach(note =>
                {
                    note.PlanId = plan.Id;
                    _noteRepository.Create(note);
                });
                _unitOfWork.SaveChanges();


                return plan;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(CreateSuggestedPlan), ex);
                return null;
            }
        }

        private List<TreeViewModels> localList = new List<TreeViewModels>();
        private int maxDate = 0;

        private void PolulateNecessityLocations(
            Plan plan,
            List<TreeViewModels> locations,
            DateTimeOffset currentDate,
            out Dictionary<NessecityType, Location> nessecityLocationMap, int dateIndex)
        {
            if (dateIndex == 1 || localList.Count == maxDate)
            {
                maxDate = localList.Count;
                localList = locations;
            }
            Location hotel = null;
            Location breakfast = null;
            Location lunch = null;
            Location dinner = null;
            #region hotel

            var findHotel = locations.Where(_ => _.Categories == "Nơi ở").OrderByDescending(_ => _.Percent);
            if (findHotel.ElementAtOrDefault(0) != null)
            {
                hotel = _locationRepository.Get(_ => _.Id == findHotel.ElementAtOrDefault(0).Id, _ => _.BusinessHours);
            }

            #endregion

            #region breakfast

            TimeSpan breakFastTime = GetMealTime(NessecityType.Breakfast);
            TimeSpan lunchTime = GetMealTime(NessecityType.Lunch);
            TimeSpan dinnerTime = GetMealTime(NessecityType.Dinner);

            _loggingService.AddSentryBreadCrum(
                "PolulateNecessityLocations",
                message: "Start adding restaurant",
                data: new Dictionary<string, object>
                {
                    ["foundHotel"] = findHotel,
                });

            foreach (var _ in locations.Where(_ => !_.Reasons.Contains("cafe")).ToList())
            {
                var tmpLocation = _locationRepository.Get(__ => __.Id == _.Id && _.Categories == "Ăn uống",
                    __ => __.BusinessHours);

                if (tmpLocation != null)
                {
                    if (breakfast == null)
                    {
                        foreach (var __ in tmpLocation.BusinessHours.ToList())
                        {
                            foreach (var ___ in tmpLocation.BusinessHours.ToList())
                            {
                                if (IsInRange(___.OpenTime, ___.CloseTime, breakFastTime))
                                {
                                    if (___.Day.Contains(ParseDate(currentDate)))
                                    {
                                        breakfast = tmpLocation;
                                    }
                                }
                            }
                        }
                    }
                    else if (lunch == null)
                    {
                        foreach (var __ in tmpLocation.BusinessHours.ToList())
                        {
                            foreach (var ___ in tmpLocation.BusinessHours.ToList())
                            {
                                if (IsInRange(___.OpenTime, ___.CloseTime, lunchTime))
                                {
                                    if (___.Day.Contains(ParseDate(currentDate)))
                                    {
                                        lunch = tmpLocation;
                                    }
                                }
                            }
                        }
                    }
                    else if (dinner == null)
                    {
                        foreach (var __ in tmpLocation.BusinessHours.ToList())
                        {
                            foreach (var ___ in tmpLocation.BusinessHours.ToList())
                            {
                                if (IsInRange(___.OpenTime, ___.CloseTime, dinnerTime))
                                {
                                    if (___.Day.Contains(ParseDate(currentDate)))
                                    {
                                        dinner = tmpLocation;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (breakfast == null || lunch == null || dinner == null)
            {
                locations = localList;//reset list
                foreach (var _ in locations.ToList())
                {
                    var tmpLocation = _locationRepository.Get(__ => __.Id == _.Id && _.Categories == "Ăn uống",
                        __ => __.BusinessHours);

                    if (tmpLocation != null)
                    {
                        if (breakfast == null)
                        {
                            foreach (var __ in tmpLocation.BusinessHours.ToList())
                            {
                                foreach (var ___ in tmpLocation.BusinessHours.ToList())
                                {
                                    if (IsInRange(___.OpenTime, ___.CloseTime, breakFastTime))
                                    {
                                        if (___.Day.Contains(ParseDate(currentDate)))
                                        {
                                            breakfast = tmpLocation;
                                        }
                                    }
                                }
                            }
                        }
                        else if (lunch == null)
                        {
                            foreach (var __ in tmpLocation.BusinessHours.ToList())
                            {
                                foreach (var ___ in tmpLocation.BusinessHours.ToList())
                                {
                                    if (IsInRange(___.OpenTime, ___.CloseTime, lunchTime))
                                    {
                                        if (___.Day.Contains(ParseDate(currentDate)))
                                        {
                                            lunch = tmpLocation;
                                        }
                                    }
                                }
                            }
                        }
                        else if (dinner == null)
                        {
                            foreach (var __ in tmpLocation.BusinessHours.ToList())
                            {
                                foreach (var ___ in tmpLocation.BusinessHours.ToList())
                                {
                                    if (IsInRange(___.OpenTime, ___.CloseTime, dinnerTime))
                                    {
                                        if (___.Day.Contains(ParseDate(currentDate)))
                                        {
                                            dinner = tmpLocation;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion
            _loggingService.AddSentryBreadCrum(
                "PolulateNecessityLocations",
                message: "wrapping up nessecity",
                data: new Dictionary<string, object>
                {
                    ["hotel"] = hotel,
                    ["breakfast"] = breakfast,
                    ["lunch"] = lunch,
                    ["dinner"] = dinner,
                });

            if (hotel != null &&
                breakfast != null &&
                lunch != null &&
                dinner != null)
            {
                var selector = locations.FirstOrDefault(_ => _.Id == breakfast.Id);
                locations.Remove(selector);
                selector = locations.FirstOrDefault(_ => _.Id == lunch.Id);
                locations.Remove(selector);
                selector = locations.FirstOrDefault(_ => _.Id == dinner.Id);
                locations.Remove(selector);

                nessecityLocationMap = new Dictionary<NessecityType, Location>
                {
                    [NessecityType.Hotel] = hotel,
                    [NessecityType.Breakfast] = breakfast,
                    [NessecityType.Lunch] = lunch,
                    [NessecityType.Dinner] = dinner
                };

                _loggingService.AddSentryBreadCrum(
                    "PolulateNecessityLocations",
                    message: "Returning",
                    data: new Dictionary<string, object>
                    {
                        ["hotel"] = hotel,
                        ["breakfast"] = breakfast,
                        ["lunch"] = lunch,
                        ["dinner"] = dinner,
                    });
            }
            else
            {
                throw new InvalidOperationException("Missing Necessity");
            }
        }

        private async Task<List<KeyValuePair<JObject, KeyValuePair<Location, Location>>>>
            PolulateEntertainmentLocations(
                Plan plan,
                List<TreeViewModels> treeLocations,
                DateTimeOffset currentDate,
                Dictionary<NessecityType, Location> nessecityLocationMap)
        {
            var result = new List<KeyValuePair<JObject, KeyValuePair<Location, Location>>>();
            int[] locationIds = treeLocations.Select(location => location.Id).ToArray();
            List<Location> locationList = _locationRepository
                .Search(location => locationIds.Contains(location.Id))
                .ToList();

            foreach (KeyValuePair<NessecityType, Location> nessecityLocation in nessecityLocationMap)
            {
                Location breakfast = _locationRepository.Get(
                    location => nessecityLocationMap[NessecityType.Breakfast].Id == location.Id
                );
                ;
                Location lunch = _locationRepository.Get(
                    location => nessecityLocationMap[NessecityType.Lunch].Id == location.Id
                );
                ;
                Location dinner = _locationRepository.Get(
                    location => nessecityLocationMap[NessecityType.Dinner].Id == location.Id
                );
                ;

                KeyValuePair<Location, NessecityType> currentMealPair;
                KeyValuePair<Location, NessecityType> nextMealPair;

                switch (nessecityLocation.Key)
                {
                    case NessecityType.Breakfast:
                        currentMealPair = new KeyValuePair<Location, NessecityType>(
                            breakfast, NessecityType.Breakfast
                        );
                        nextMealPair = new KeyValuePair<Location, NessecityType>(
                            lunch, NessecityType.Lunch
                        );
                        break;
                    case NessecityType.Lunch:
                        currentMealPair = new KeyValuePair<Location, NessecityType>(
                            breakfast, NessecityType.Lunch
                        );
                        nextMealPair = new KeyValuePair<Location, NessecityType>(
                            dinner, NessecityType.Dinner
                        );
                        break;
                    default:
                        continue;
                }

                List<KeyValuePair<JObject, KeyValuePair<Location, Location>>> locationWithRouteList = null;
                int areaOffSet = 4000;
                try
                {
                    while (locationWithRouteList == null && areaOffSet <= 20000)
                    {
                        var locationsBetweenMeal = GetLocationsBetweenMeal(
                            currentMealPair,
                            nextMealPair,
                            locationList,
                            areaOffSet
                        );

                        _loggingService.AddSentryBreadCrum("PolulateEntertainmentLocations",
                            new Dictionary<string, object>
                            {
                                ["areaOffSet"] = areaOffSet,
                                ["locationsBetweenMeal length"] = locationsBetweenMeal.Count
                            });
                        areaOffSet += 4000;

                        locationWithRouteList = await FitSchedule(
                            currentMealPair,
                            nextMealPair,
                            locationsBetweenMeal,
                            currentDate
                        );
                    }
                }
                catch (Exception ex)
                {
                    _loggingService.CaptureSentryException(ex);
                }

                if (locationWithRouteList != null)
                {
                    for (int i = 0; i < locationWithRouteList.Count; i++)
                    {
                        var locationWithRoute = locationWithRouteList[i];
                        if (i == 0)
                        {
                            var location = locationWithRoute.Value.Value;
                            locationList.RemoveAll(tmpLocation => tmpLocation.Id == location.Id);
                        }
                        else
                        {
                            var location = locationWithRoute.Value.Key;
                            locationList.RemoveAll(tmpLocation => tmpLocation.Id == location.Id);
                        }
                    }
                    result.AddRange(locationWithRouteList);
                }
                

            }

            return result;
        }

        public GeoCoordinate GetFrationGeoPoint(double frac, GeoCoordinate origin, GeoCoordinate destination)
        {
            Double longitude = origin.Longitude + frac * (destination.Longitude - origin.Longitude);
            Double latitude = origin.Latitude + frac * (destination.Latitude - origin.Latitude);

            return new GeoCoordinate(latitude, longitude);
        }

        private List<Location> GetLocationsBetweenMeal(
            KeyValuePair<Location, NessecityType> origin,
            KeyValuePair<Location, NessecityType> destination,
            List<Location> locations,
            int areaOffset)
        {
            List<Location> resultLocations = new List<Location>();
            var originGeo = new GeoCoordinate(origin.Key.Latitude, origin.Key.Longitude);
            var destinationGeo = new GeoCoordinate(destination.Key.Latitude, destination.Key.Longitude);

            var distanceGeo = originGeo.GetDistanceTo(destinationGeo);
            double numberOfFraction = Math.Ceiling(distanceGeo / 1000);
            for (int i = 0; i <= numberOfFraction; i++)
            {
                GeoCoordinate middleGeoPoint = GetFrationGeoPoint(i / numberOfFraction, originGeo, destinationGeo);
                
                foreach (var location in locations)
                {
                    if (location.Category == "Địa điểm thăm quan")
                    {
                        var isdupped = false;
                        resultLocations.ForEach(resultLocation =>
                        {
                            if (location.Id == resultLocation.Id)
                            {
                                isdupped = true;
                            }
                        });
                        
                        GeoCoordinate locationGeo = new GeoCoordinate(location.Latitude, location.Longitude);
                        if (!isdupped && locationGeo.GetDistanceTo(middleGeoPoint) <= areaOffset)
                        {
                            resultLocations.Add(location);
                        }
                    }
                }
            }

            return resultLocations;
        }

        private async Task<List<KeyValuePair<JObject, KeyValuePair<Location, Location>>>> FitSchedule(
            KeyValuePair<Location, NessecityType> origin,
            KeyValuePair<Location, NessecityType> destination,
            List<Location> locationsToGo,
            DateTimeOffset currentDate)
        {
            var result = new List<KeyValuePair<JObject, KeyValuePair<Location, Location>>>();
            TimeSpan departureTime =
                GetMealTime(origin.Value).Add(GetLocationStayTime(origin.Key));
            TimeSpan arriveTime = GetMealTime(destination.Value);

            locationsToGo.RemoveAll(location => !IsLocationOpenIn(location, departureTime, arriveTime, currentDate));

            #region Calculate arrive time

            JObject responseJObject =
                await FetchGoogleRoute(origin.Key, destination.Key, locationsToGo.Take(23).ToList(), true);

            JArray legs = responseJObject["routes"][0]["legs"].Value<JArray>();
            JArray waypointOrder = responseJObject["routes"][0]["waypoint_order"].Value<JArray>();

            var map = MapLegsAndLocationPair(legs, waypointOrder, origin.Key, destination.Key, locationsToGo);
            TimeSpan totalTime = new TimeSpan(0);
            bool enough = false;
            try
            {
                foreach (KeyValuePair<JObject, KeyValuePair<Location, Location>> keyValuePair in map)
                {
                    int travelTimeMinutes = keyValuePair.Key["duration"]["value"].Value<int>();
                    TimeSpan travelTime = new TimeSpan(0, 0, 0, travelTimeMinutes, 0);
                    TimeSpan stayTime = GetLocationStayTime(keyValuePair.Value.Key);

                    totalTime = totalTime.Add(travelTime + stayTime);

                    if (departureTime.Add(totalTime) < arriveTime)
                    {
                        result.Add(keyValuePair);
                    }
                    else
                    {
                        enough = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.CaptureSentryException(ex);
            }

            #endregion

            return enough ? result : null;
        }

        private List<KeyValuePair<JObject, KeyValuePair<Location, Location>>> MapLegsAndLocationPair(
            JArray legs,
            JArray waypointOrder,
            Location origin,
            Location destination,
            List<Location> middle)
        {
            var result = new List<KeyValuePair<JObject, KeyValuePair<Location, Location>>>();
            for (int i = 0; i < legs.Count; i++)
            {
                var leg = legs[i].Value<JObject>();

                KeyValuePair<Location, Location> fromToPair;
                if (i == 0)
                {
                    if (legs.Count > 1)
                    {
                        leg.Add("index", waypointOrder.First.Value<int>());
                        fromToPair = new KeyValuePair<Location, Location>(
                            origin,
                            middle[waypointOrder.First.Value<int>()]
                        );
                    }
                    else
                    {
                        leg.Add("index", 0);
                        fromToPair = new KeyValuePair<Location, Location>(
                            origin,
                            destination
                        );
                    }
                }
                else if (i == legs.Count - 1)
                {
                    leg.Add("index", waypointOrder.Last.Value<int>() + 1);
                    fromToPair = new KeyValuePair<Location, Location>(
                        middle[waypointOrder.Last.Value<int>()],
                        destination
                    );
                }
                else
                {
                    leg.Add("index", waypointOrder[i].Value<int>());
                    fromToPair = new KeyValuePair<Location, Location>(
                        middle[waypointOrder[i - 1].Value<int>()],
                        middle[waypointOrder[i].Value<int>()]
                    );
                }

                result.Add(
                    new KeyValuePair<JObject, KeyValuePair<Location, Location>>(
                        leg,
                        fromToPair
                    )
                );
            }

            result.Sort((pair1, pair2) => pair1.Key["index"].Value<int>().CompareTo(pair2.Key["index"].Value<int>()));

            return result;
        }

        private TimeSpan GetLocationStayTime(Location location)
        {
            if (location.TotalTimeStay == null || location.TotalStayCount == null || location.TotalStayCount == 0)
                return new TimeSpan(0);

            int minutes = (int) location.TotalTimeStay / (int) location.TotalStayCount;
            return new TimeSpan(0, minutes, 0);
        }

        private string ParseDate(DateTimeOffset currentDate)
        {
            string date = currentDate.DayOfWeek.ToString("d");
            int dateValue = int.Parse(date) + 1;
            return dateValue.ToString();
        }

        private bool IsInRange(TimeSpan start, TimeSpan end, TimeSpan time)
        {
            return (time > start) && (time < end);
        }

        private bool IsLocationOpenIn(Location location, TimeSpan start, TimeSpan end, DateTimeOffset date)
        {
            foreach (BusinessHour businessHour in location.BusinessHours)
            {
                if (businessHour.Day.Contains(ParseDate(date)))
                {
                    TimeSpan zeroTime = new TimeSpan(0, 0, 0);
                    if (businessHour.OpenTime == zeroTime && businessHour.CloseTime == zeroTime)
                    {
                        return true;
                    }

                    if (start >= businessHour.OpenTime &&  businessHour.CloseTime >= end)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<JObject> FetchGoogleRoute(
            Location origin,
            Location destination,
            List<Location> waypoints,
            bool isOptimized = false)
        {
            var uriBuilder = new UriBuilder("https://maps.googleapis.com/maps/api/directions/json");
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["key"] = "AIzaSyCEm8r5LQCvBGGon-WfuT9u1gJkYZCkYHQ";
            query["language"] = "vi";
            query["units"] = "metric";
            query["origin"] = $"{origin.Latitude},{origin.Longitude}";
            query["destination"] = $"{destination.Latitude},{destination.Longitude}";

            if (waypoints != null && waypoints.Count > 0)
            {
                StringBuilder waypointsStringBuilder = new StringBuilder();
                if (isOptimized)
                {
                    waypointsStringBuilder.Append("optimize:true");
                }

                foreach (Location waypoint in waypoints)
                {
                    waypointsStringBuilder.Append('|');
                    waypointsStringBuilder.Append($"{waypoint.Latitude},{waypoint.Longitude}");
                }

                query["waypoints"] = waypointsStringBuilder.ToString();
            }

            uriBuilder.Query = query.ToString();
            _loggingService.AddSentryBreadCrum(
                "FetchGoogleRoute",
                new Dictionary<string, object>
                {
                    ["query"] = query.ToString(),
                    ["url"] = uriBuilder.ToString(),
                });

            HttpResponseMessage response = await _client.GetAsync(uriBuilder.ToString());
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseBody);
        }

        private TimeSpan GetMealTime(NessecityType type)
        {
            switch (type)
            {
                case NessecityType.Breakfast:
                    return new TimeSpan(8, 30, 00);
                case NessecityType.Lunch:
                    return new TimeSpan(12, 00, 00);
                case NessecityType.Dinner:
                    return new TimeSpan(17, 30, 00);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}