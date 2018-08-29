import _ from "lodash"
import Raven from "raven-js"
import formatter from "../../formatter"
import {axiosInstance} from "../../common/util"

export default {
  namespaced: true,
  state: {
    // {id: 3, status: 0, message: null, groupId: 36, groupName: "nhóm phượt 2"}
    groupInvitation: [],
    // [{"id":36,"comment":"test nè","status":0,"planId":542,"planDay":0,"name":"tlp","locations":[{"item1":44,"item2":"Xôi Gà Bà Chiểu"}]}]
    locationSuggestion: [],
    loading: {
      groupInvitation: true,
      locationSuggestion: true
    }
  },
  getters: {
    locationSuggestions(state) {
      return _.map(state.locationSuggestion, (suggestion) => {
        return {
          id: suggestion.id,
          key: "LocationSuggestion_" + suggestion.id,
          status: suggestion.status,
          statusText: formatter.getStatusText(suggestion.status),
          message: suggestion.comment,
          type: "LocationSuggestion",
          data: suggestion
        }
      });
    },
    groupInvitations(state) {
      return _.map(state.groupInvitation, (invitation) => {
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
    },
    notificationsLoading(state) {
      return state.loading.groupInvitation && state.loading.locationSuggestion;
    }
  },
  mutations: {
    setGroupInvitationRequests(state, payload) {
      state.groupInvitation = payload.requests;
    },
    setLocationSuggestionRequest(state, payload) {
      state.locationSuggestion = payload.requests;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    },
    changeStatusGroupInvitation(state, payload) {
      const {
        id,
        status
      } = payload;

      state.groupInvitation = _.map(state.groupInvitation, (invitation) => {
        if (invitation.id == id) {
          invitation.status = status;
        }
        return invitation;
      })
    },
    changeStatusLocationSuggestion(state, payload) {
      const {
        id,
        status
      } = payload;

      state.locationSuggestion = _.map(state.locationSuggestion, (suggestion) => {
        if (suggestion.id == id) {
          suggestion.status = status;
        }
        return suggestion;
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

    fetchLocationSuggestionRequests(context) {
      // get /api/Group/GetLocationSuggestions
      context.commit('setLoading', {
        loading: {locationSuggestion: true}
      });

      return new Promise((resolve, reject) => {
        axiosInstance.get('api/Group/GetLocationSuggestions')
          .then((value) => {
            context.commit('setLocationSuggestionRequest', {requests: value.data});
            context.commit('setLoading', {
              loading: {locationSuggestion: false}
            });
            resolve(value.data)
          })
          .catch((reason) => {
            Raven.captureException(reason);
            context.commit('setLoading', {
              loading: {locationSuggestion: false}
            });
            reject(reason.response);
          })
      })
    },
    fetchNotifications(context) {
      const groupInvitationPromise = context.dispatch('fetchGroupInvitationRequests');
      const locationSuggestionPromise = context.dispatch('fetchLocationSuggestionRequests');

      return new Promise((resolve, reject) => {
        Promise.all([groupInvitationPromise, locationSuggestionPromise])
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
        locations,
        planId,
        planDay,
        message
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Plan/AddSuggestion', {
          "planId": planId,
          "locationIds": locations,
          "planDay": planDay,
          "comment": message
        })
          .then((value) => {
            resolve(value.data);
          })
          .catch((reason) => {
            Raven.captureException(reason);
            reject(reason.response);
          })
      })

    },
    acceptLocationSuggestion(context, payload) {
      // PUT /api/Location/ApproveSuggestion
      const {
        id
      } = payload;

      axiosInstance.put('api/Location/ApproveSuggestion?suggestionId=' + id);
    },
    denyLocationSuggestion(context, payload) {
      // PUT /api/Location/RejectSuggestion
      const {
        id
      } = payload;

      axiosInstance.put('api/Location/RejectSuggestion?suggestionId=' + id);
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
    },
    sendReportReview(context, payload) {
      // post /api/Location/CreateReview
      const {
        reviewId,
        commentInput
      } = payload;

      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Location/CreateReview', {
          params: {
            reviewId,
            commentInput
          }
        })
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject(reason.response);
          })
      })
    },
    createChangeRequest(context, payload){
      // post /api/Location/CreateChangeRequest
      const {
        id,
        name,
        address,
        description,
        phone,
        email,
        website,
        tags,
        businessHours
      } = payload;

      const data  = {
        "locationId": id,
        "name": name,
        "address": address,
        "description": description,
        "phoneNumber": phone,
        "website": website,
        "businessHours": businessHours,
        "email": email,
        "tags": JSON.stringify(_.map(tags, 'id'))
      };

      return new Promise((resolve, reject) => {
        axiosInstance.post('api/Location/CreateChangeRequest', data)
          .then(value => {
            resolve(value.data);
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject(reason.response);
          })
      })
    }
  }
}
