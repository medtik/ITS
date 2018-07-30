import {axiosInstance} from "../../common/util";
import _ from "lodash";
import moment from "moment";


export default {
  namespaced: true,
  state: {
    featuredPlans: [],
    myPlans: [],
    loading: {
      myPlans: true,
      featuredPlans: true
    }
  },
  getters: {
    featuredPlans(state) {
      return state.featuredPlans
    },
    featuredPlansLoading(state) {
      return state.loading.featuredPlans;
    },
    myPlans(state) {
      return state.myPlans;
    },
    myPlansLoading(state) {
      return state.loading.myPlans;
    },
  },
  mutations: {
    setFeaturedPlans(state, payload) {
      state.featuredPlans = payload.plans;
    },
    setMyPlans(state, payload) {
      const plans = _.cloneDeep(payload.plans);
      const formattedPlans = _.map(plans, plan => {
         return _.mapValues(plan,
          (value, key) => {
            switch (key) {
              case 'startDate':
              case 'endDate':
                return moment(value).format('YYYY-MM-DD');
              default:
                return value;
            }
          })
      });
      state.myPlans = formattedPlans;
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

    }
  }
}
