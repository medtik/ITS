import _ from "lodash"
import Raven from "raven-js"
import formatter from "../../formatter"
import {axiosInstance} from "../../common/util"

export default {
  namespaced: true,
  state: {
    // {id: 3, status: 0, message: null, groupId: 36, groupName: "nhóm phượt 2"}
    groupInvitation: [],
    loading: {
      groupInvitation: true
    }
  },
  getters: {
    notifications(state) {
      const groupInvitation = _.map(state.groupInvitation, (invitation) => {
        return {
          id: invitation.id,
          key: "GroupInvitation_" + invitation.id,
          status: invitation.status,
          statusText: formatter.getStatusText(invitation.status),
          title: `Mời vào`,
          message: invitation.message,
          type: "GroupInvitation",
          data: invitation
        }
      });
      return groupInvitation
    },
    notificationsLoading(state) {
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
    changeStatusGroupInvitation(state, payload) {
      const {
        id,
        status
      } = payload;

      state.groupInvitation = _.mapValues(state.groupInvitation, (invitation) => {
        if (invitation.id == id) {
          return invitation.status = status;
        }
      })
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
          .then(() => {
            resolve();
          })
          .catch((reason) => {
            Raven.captureException(reason);
            reject(reason);
          })
      })
    },
    createLocationSuggestion(context, payload) {
      // post /api/Plan/AddSuggestion
      const {
        locationId,
        planId,
        comment
      } = payload;

      axiosInstance.post('api/Plan/AddSuggestion', {
        locationSuggestion: {
          "planId": planId,
          "locationId": locationId,
          "comment": comment
        }
      })
    },
    acceptLocationSuggestion(context, payload) {

    },
    denyLocationSuggestion(context, payload) {
    },
    acceptGroupInvitation(context, payload) {
      // put /api/Group/AcceptGroupInvitation
      const {
        id
      } = payload;

      axiosInstance.put('api/Group/AcceptGroupInvitation?groupInvitationId=' + id)
    },
    denyGroupInvitation(context, payload) {
      // put /api/Group/DenyGroupInvitation
      const {
        id
      } = payload;

      axiosInstance.put('api/Group/DenyGroupInvitation?groupInvitationId=' + id)
    }
  }
}
