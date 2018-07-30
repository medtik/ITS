import {axiosInstance} from "../../common/util";
import _ from "lodash";


export default {
  namespaced: true,
  state: {
    featuredPlans: [],
    loading: {
      featuredPlans: true
    }
  },
  getters: {
    featuredPlans(state) {
      return state.featuredPlans
    },
    featuredPlansLoading(state) {
      return state.loading.featuredPlans;
    }
  },
  mutations: {
    setFeaturedPlans(state, payload) {
      state.featuredPlans = payload.plans;
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
      axiosInstance.get('api/myPlans')
    }
  }
}
