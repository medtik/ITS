import {axiosInstance} from "../../common/util";
import _ from "lodash";
import moment from "moment";

export default {
  namespaced: true,
  state: {
    featuredPlans: [],
    detailedPlan: {},
    myPlans: [],
    loading: {
      myPlans: true,
      featuredPlans: true,
      detailedPlan: true,
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
    setDetailedPlan(state, payload) {
      state.detailedPlan = _.cloneDeep(payload.plan);
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
    create(context, payload) {
      const {
        name,
        startDate,
        endDate
      } = payload;

      context.commit('setLoading', {
        loading: {create: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.post('api/plan', {name, startDate, endDate})
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
