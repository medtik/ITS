import {axiosInstance} from "../../common/util";
import formatter from "../../formatter"
import _ from "lodash";
import Raven from "raven-js";

import moment from "moment";

export default {
  namespaced: true,
  state: {
    featuredPlans: [],
    detailedPlan: {
      days: undefined,
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
      createNoteBtn: false,
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
    myVisiblePlansFlattened(state) {
      return _.flatten(state.myVisiblePlans);
    },
    createNoteLoading(state){
      return state.loading.createNoteBtn;
    },
    addLocationToPlanLoading(state) {
      return state.loading.addLocationToPlan;
    },
    removeLocationFromPlan(state) {
      return state.loading.removeLocationFromPlan;
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
    deletePlan(state, payload) {
      if (state.myPlans && state.myPlans.length > 0) {
        state.myPlans = _.filter(state.myPlans, plan => {
          return plan.id != payload.id;
        })
      }
      if (state.myVisiblePlans && state.myVisiblePlans.length > 0) {
        state.myVisiblePlans = _.map(state.myVisiblePlans, group => {
          return _.filter(group, plan => {
            return plan.id != payload.id;
          });
        });
      }
    },
    setDetailedPlan(state, payload) {
      const detailedPlan = _.cloneDeep(payload.plan);
      const startDate = moment(detailedPlan.startDay);
      const endDate = moment(detailedPlan.endDate);
      const diffDays = endDate.diff(startDate, 'days');

      let items = _.concat(detailedPlan.locations, detailedPlan.notes);
      items = _(items)
        .map(item => {
          if (item.locationId) {
            //  Location
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
          } else {
            // Note
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
          }
        })
        .orderBy(['index'], ['asc'])
        .groupBy(item => {
          return item.planDay
        })
        .map((value, key) => {
          return {
            ...formatter.getDaysObj(key, detailedPlan.startDay),
            items: value
          }
        })
        .values()
        .value();


      const planDays = [];
      for (let i = 0; i < diffDays + 2; i++) {
        const matchedItem = _.find(items, item =>{
          return item.planDay == i
        });

        if(matchedItem){
          planDays.push(matchedItem);
        }else{
          planDays.push(formatter.getDaysObj(i, detailedPlan.startDay));
        }
      }

      detailedPlan.days = planDays;
      state.detailedPlan = detailedPlan;
    },
    removeItemFromPlan(state, payload) {
      const {
        itemId
      } = payload;

      const days = _.map(state.detailedPlan.days, (day) => {
        day.items = _.filter(day.items, (item) => {
          return itemId != item.id;
        });
        return day;
      });

      const plan = state.detailedPlan;
      plan.days = days;

      state.detailedPlan = plan;
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
        id,
        noLoading
      } = payload;

      if (!noLoading) {
        context.commit('setLoading', {
          loading: {
            detailedPlan: true
          }
        });
      }

      axiosInstance.get('api/Plan/Details', {
        params: {id}
      })
        .then((value) => {
          context.commit('setDetailedPlan', {plan: value.data});
          if (!noLoading) {
            context.commit('setLoading', {
              loading: {
                detailedPlan: false
              }
            });
          }
        })
        .catch(reason => {
          console.error('fetchPlanById', reason.response);
          if (!noLoading) {
            context.commit('setLoading', {
              loading: {
                detailedPlan: false
              }
            });
          }
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
    fetchVisiblePlans(context,payload) {
      //get /api/User/MyVisiblePlan
      const {
        areaId
      } = payload || {};

      context.commit('setLoading', {
        loading: {myVisiblePlans: true}
      });
      axiosInstance.get('api/User/MyVisiblePlan',{
        params:{
          areaId
        }
      })
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
        comment,
        planDay
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.put('api/Plan/AddLocations', {
          comment,
          locationId,
          planId,
          planDay
        })
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response);
          })
      })
    },
    removeLocationFromPlan(context, payload) {
      //delete /api/Plan/AddLocations
      const {
        itemId
      } = payload;
      const planId = context.getters.detailedPlan.id;

      let foundLocationPlan = undefined;
      _.forEach(context.getters.detailedPlan.days, (day) => {
        _.forEach(day.items, (item) => {
          if (item.id == itemId) {
            foundLocationPlan = item;
          }
        });
      });

      context.commit('removeItemFromPlan', {
        itemId
      });
      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/Plan/AddLocations', {
          params: {
            planId,
            locationId: foundLocationPlan.location.id
          }
        })
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            reject(reason.response)
          })
      })
    },
    addNoteToPlan(context, payload) {
      //post /api/Plan/AddNote
      const {
        title,
        content,
        planDay,
        planId,
      } = payload;

      //loading.createNoteBtn
      context.commit('setLoading', {
        loading: {createNoteBtn: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Plan/AddNote', {
          title,
          content,
          planDay,
          planId,
        }).then(value => {
          context.commit('setLoading', {
            loading: {createNoteBtn: false}
          });
          resolve(value.data)
        }).catch(reason => {
          context.commit('setLoading', {
            loading: {createNoteBtn: false}
          });
          reject(reason.response);
        })
      })

    },
    removeNoteFromPlan(context, payload) {
      // delete /api/Plan/DeleteNote
      const {
        id
      } = payload;

      context.commit('removeItemFromPlan', {
        itemId: id
      });
      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/Plan/DeleteNote', {
          params: {
            noteId: id
          }
        }).then(value => {

          resolve(value.data)
        }).catch(reason => {
          Raven.captureException(reason);
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
            resolve({id: value.data});
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
            context.commit('deletePlan', {
              id
            });
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
