import _ from "lodash"
import Raven from "raven-js"
import {axiosInstance} from "../../common/util"

export default {
  namespaced: true,
  state: {
    map: [
      {key: 0, value: "Đang chờ"},
      {key: 1, value: "Chấp nhận"},
      {key: 2, value: "Từ chối"},
    ],
    groupInvitation: [],
    loading: {
      groupInvitation: true
    }
  },
  getters :{
    notifications(state){
      return state.groupInvitation;
    },
    notificationsLoading(state){
      return state.loading.groupInvitation;
    }
  },
  mutations: {
    setGroupInvitationRequests(state, payload) {
      state.groupInvitation = payload.requests;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    },
    acceptGroupInvitation(){

    },
    denyGroupInvitation(){

    }
  },
  actions: {
    fetchGroupInvitationRequests(context) {
      // get /api/User/GetGroupInvitation
      context.commit('setLoading', {
        loading: {groupInvitation: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/User/GetGroupInvitation')
          .then((value) => {
            context.commit('setGroupInvitationRequests', {requests: value.data});
            context.commit('setLoading', {
              loading: {groupInvitation: false}
            });
            resolve(value.data)
          })
          .catch((reason) => {
            Raven.captureException(reason);
            context.commit('setLoading', {
              loading: {groupInvitation: false}
            });
            reject(reason.response);
          })
      })
    },
    fetchNotifications(context) {
      const groupInvitationPromise = context.dispatch('fetchGroupInvitationRequests');

      return new Promise((resolve, reject) => {
        Promise.all([groupInvitationPromise])
          .then(()=>{
            resolve();
          })
          .catch((reason)=>{
            Raven.captureException(reason);
            reject(reason);
          })
      })
    },
    createLocationSuggestion(context, payload){
      // post /api/Plan/AddSuggestion
      const {
        locationId,
        planId,
        comment
      } = payload;

      axiosInstance.post('api/Plan/AddSuggestion',{
        locationSuggestion: {
          "planId": planId,
          "locationId": locationId,
          "comment": comment
        }
      })
    },
    acceptLocationSuggestion(context, payload){

    },
    denyLocationSuggestion(context, payload){

    },
    acceptGroupInvitation(context, payload){
      const {
        id
      } = payload;

    },
    denyGroupInvitation(context, payload){
      const {
        id
      } = payload;


      // axiosInstance.put
    }
  }
}
