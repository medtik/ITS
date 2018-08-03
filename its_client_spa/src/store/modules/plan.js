import {axiosInstance} from "../../common/util";
import _ from "lodash";
import moment from "moment";

export default {
  namespaced: true,
  state: {
    featuredPlans: [],
    detailedPlan: {
      locations: undefined,
      notes: undefined
    },
    myPlans: [],
    myVisiblePlans: [],
    loading: {
      myPlans: true,
      myVisiblePlans: false,
      featuredPlans: true,
      detailedPlan: true,
      addLocationToPlan: false,
      create: false,
      delete: false
    }
  },
  getters: {
    featuredPlans(state) {
      return state.featuredPlans
    },
    detailedPlan(state) {
      return state.detailedPlan;
    },
    myPlans(state) {
      return state.myPlans;
    },
    myVisiblePlans(state) {
      return state.myVisiblePlans;
    },
    addLocationToPlanLoading(state){
      return state.loading.addLocationToPlan;
    },
    myVisiblePlansLoading(state) {
      return state.loading.myVisiblePlans;
    },
    featuredPlansLoading(state) {
      return state.loading.featuredPlans;
    },
    detailedPlanLoading(state) {
      return state.loading.detailedPlan;
    },
    myPlansLoading(state) {
      return state.loading.myPlans;
    },
    createLoading(state) {
      return state.loading.create;
    }
  },
  mutations: {
    setFeaturedPlans(state, payload) {
      state.featuredPlans = payload.plans;
    },
    setMyVisiblePlans(state, payload) {
      state.myVisiblePlans = payload.plans
    },
    setDetailedPlan(state, payload) {
      const detailedPlan = _.cloneDeep(payload.plan);

      const getDayText = (planDay) => {
        switch (planDay) {
          case "0":
          case 0:
            return "Chưa lên lịch";
          default:
            if (detailedPlan.startDay) {
              return moment(detailedPlan.startDay)
                .add(planDay - 1, "days")
                .format('DD/MM/YYYY');
            } else {
              return `Ngày ${planDay}`
            }

        }
      };
      const locations = _(detailedPlan.locations)
        .map(item => {
          return {
            location: {
              id: item.locationId,
              address: item.address,
              primaryPhoto: item.photo,
              location: item.title,
              reviewCount: item.reviewCount,
              rating: item.rating
            },
            id: item.planLocationId,
            planDay: item.planDay,
            index: item.index,
            type: 'location',
          }
        })
        .value();

      const items = _(detailedPlan.notes)
        .map(item => {
          return {
            note: {
              name: item.name,
              description: item.description
            },
            id: item.id,
            planDay: item.planDay,
            index: item.index,
            type: 'note',
          }
        })
        .concat(locations)
        .orderBy(['index'], ['asc'])
        .groupBy(item => {
          return item.planDay
        })
        .map((value, key) => {
          return {
            planDayText: getDayText(key),
            items: value
          }
        })
        .values()
        .value();

      detailedPlan.days = items;
      state.detailedPlan = detailedPlan;
    },
    setMyPlans(state, payload) {
      state.myPlans = _.cloneDeep(payload.plans);
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    }
  },
  actions: {
    getFeatured(context) {
      // get /api/FeaturedTrip
      context.commit('setLoading', {
        loading: {featuredPlans: true}
      });
      axiosInstance.get('api/FeaturedTrip')
        .then(value => {
          context.commit('setFeaturedPlans', {plans: value.data});
          context.commit('setLoading', {
            loading: {featuredPlans: false}
          })
        })
    },
    fetchPlanById(context, payload) {
      const {
        id
      } = payload;

      context.commit('setLoading', {
        loading: {
          detailedPlan: true
        }
      });
      axiosInstance.get('api/Plan/Details', {
        params: {id}
      })
        .then((value) => {
          context.commit('setDetailedPlan', {plan: value.data});
          context.commit('setLoading', {
            loading: {
              detailedPlan: false
            }
          });
        })
        .catch(reason => {
          console.error('fetchPlanById', reason.response);
          context.commit('setLoading', {
            loading: {
              detailedPlan: false
            }
          });
        })
    },
    fetchMyPlans(context) {
      context.commit('setLoading', {
        loading: {myPlans: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/myPlans')
          .then(value => {
            context.commit('setMyPlans', {
              plans: value.data
            });
            context.commit('setLoading', {
              loading: {myPlans: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {myPlans: false}
            });
            reject(reason.response);
          })
      });
    },
    fetchVisiblePlans(context) {
      //get /api/User/MyVisiblePlan
      context.commit('setLoading', {
        loading: {myVisiblePlans: true}
      });
      axiosInstance.get('api/User/MyVisiblePlan')
        .then(value => {
          context.commit('setLoading', {
            loading: {myVisiblePlans: true}
          });
          context.commit('setMyVisiblePlans', {plans: value.data});
        })
        .catch(reason => {
          console.error('plan/fetchVisiblePlans', reason.response)
        })
    },
    addLocationToPlan(context, payload) {
      const {
        locationId,
        planId,
        comment
      } = payload;

     return new Promise((resolve, reject) => {
       axiosInstance.put('api/Plan/AddLocations', {
         comment,
         locationId,
         planId
       })
         .then(value =>{
           resolve(value.data);
         })
         .catch(reason => {
           reject(reason.response);
         })
     })

    },
    moveItemUp(state, payload) {
      const {
        item
      } = payload;

    },
    moveItemDown(state, payload) {
      const {
        item
      } = payload;
    },
    setDayItems(state, payload) {

    },
    create(context, payload) {
      const {
        name,
        startDate,
        endDate,
        areaId
      } = payload;

      context.commit('setLoading', {
        loading: {create: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.post('api/plan', {name, startDate, endDate, areaId})
          .then(value => {
            context.commit('setLoading', {
              loading: {create: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {create: false}
            });
            console.error('plan/create', reason.response);
            reject(reason.response);
          })
      });
    },
    delete(context, payload) {
      const {
        id
      } = payload;

      context.commit('setLoading', {
        loading: {delete: true}
      });

      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/plan', {
          params: {
            planId: id
          }
        })
          .then((value) => {
            context.commit('setLoading', {
              loading: {delete: false}
            });
            resolve(value.data)
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {delete: false}
            });
            reject(reason.response);
          })
      })
    }
  }
}
