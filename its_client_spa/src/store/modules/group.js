import {axiosInstance} from "../../common/util";
import Raven from "raven-js";

import _ from "lodash";

export default {
  namespaced: true,
  state: {
    myGroups: [],
    detailedGroup: {},
    loading: {
      myGroups: true,
      detailedGroup: true,
      create: false,
      delete: false,
      addPlanToGroup: false,
      updateDetailedGroup: false,
    }
  },
  getters: {
    myGroups(state) {
      return state.myGroups;
    },
    detailedGroup(state) {
      return state.detailedGroup;
    },
    myGroupsLoading(state) {
      return state.loading.myGroups;
    },
    createLoading(state) {
      return state.loading.create;
    },
    deleteLoading(state) {
      return state.loading.delete;
    },
    detailedGroupLoading(state) {
      return state.loading.detailedGroup;
    },
    addPlanToGroupLoading(state) {
      return state.loading.addPlanToGroup;
    }
  },
  mutations: {
    setMyGroup(state, payload) {
      state.myGroups = payload.groups;
    },
    setDetailedGroup(state, payload) {
      state.detailedGroup = payload.group;
    },
    setLoading(state, payload) {
      state.loading = _.assign(state.loading, payload.loading);
    },
    deleteGroupPlan(state, payload) {
      const {
        id
      } = payload;

      let plans = state.detailedGroup.plans;
      if (state.detailedGroup.plans && state.detailedGroup.plans.length > 0) {
        plans = _.filter(plans, plan => {
          return plan.id != id
        });

        state.detailedGroup = {
          ...state.detailedGroup,
          plans
        }
      }
    },
    deleteUser(state, payload) {
      const {
        id
      } = payload;

      let users = _.filter(state.detailedGroup.users, user => {
        return user.id != id;
      });

      const group = {
        ...state.detailedGroup,
      };
      group.users = users;
      state.detailedGroup = group;
    }
  },
  actions: {
    fetchMyGroups(context) {
      context.commit('setLoading', {
        loading: {myGroups: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/GetGroups')
          .then(value => {
            context.commit('setMyGroup', {groups: value.data});
            context.commit('setLoading', {
              loading: {myGroups: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {myGroups: false}
            });
            reject(reason.response);
          })
      });
    },
    deleteUser(context, payload) {
      // put /api/Group/RemoveUser
      const {
        groupId,
        userId
      } = payload;

      axiosInstance.put('api/Group/RemoveUser', {
        "userId": userId,
        "groupId": groupId
      })
    },
    fetchById(context, payload) {
      const {
        id
      } = payload;

      context.commit('setLoading', {
        loading: {detailedGroup: true}
      });

      axiosInstance.get('api/group/Details', {
        params: {
          id
        }
      })
        .then(value => {
          context.commit('setDetailedGroup', {
            group: {
              ...value.data,
              id
            }
          });
          context.commit('setLoading', {
            loading: {detailedGroup: false}
          });
        })
        .catch(reason => {
          context.commit('setLoading', {
            loading: {detailedGroup: false}
          });
          Raven.captureException(reason);
        })
    },
    updateDetailedGroup(context, payload) {
      if (!context.state.detailedGroup) {
        const error = new Error('Missing detailed group when attempt to update');
        Raven.captureException(error);
        return Promise.reject(error);
      }

      context.commit('setLoading', {
        loading: {updateDetailedGroup: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.get('api/group/Details', {
          params: {
            id: context.state.detailedGroup.id
          }
        }).then((value) => {
          context.commit('setDetailedGroup', {
            group: value.data
          });
          context.commit('setLoading', {
            loading: {updateDetailedGroup: false}
          });
          resolve(value.data);
        }).catch(reason => {
          Raven.captureException(reason);
          context.commit('setLoading', {
            loading: {updateDetailedGroup: false}
          });
          reject(reason.response);
        })
      });
    },
    addPlanToGroup(context, payload) {
      // PUT /api/Group/SavePlan
      const {
        planId,
        groupId
      } = payload;

      context.commit('setLoading', {
        loading: {addPlanToGroup: true}
      });
      let url = `api/Group/SavePlan?planId=${planId}`;
      if(groupId){
        url += `&groupId=${groupId}`
      }
      return new Promise((resolve, reject) => {
        axiosInstance.put(url)
          .then(value => {
            context.commit('setLoading', {
              loading: {addPlanToGroup: false}
            });
            resolve(value.data);
          })
          .catch(reason => {
            context.commit('setLoading', {
              loading: {addPlanToGroup: false}
            });
            Raven.captureException(reason);
            reject(reason.response);
          })
      })
    },
    sendGroupInvitationRequest(context, payload) {
      const {
        userId,
        groupId,
        message
      } = payload;

      const reqData = {
        "userId": userId,
        "groupId": groupId,
        "message": message
      };

      // put /api/Group/UserInvitation
      return new Promise((resolve, reject) => {
        axiosInstance.put("api/Group/UserInvitation", reqData)
          .then((value) => {
            resolve(value.data);
          })
          .catch(reason => {
            Raven.captureException(reason);
            reject(reason.response);
          })
      });

    },

    create(context, payload) {
      const {
        name
      } = payload;

      context.commit('setLoading', {
        loading: {create: true}
      });

      return new Promise((resolve, reject) => {
        axiosInstance.post('api/group', {
          name
        })
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
            reject(reason.response);
          })
      })
    },
    delete(context, payload) {
      const {
        id
      } = payload;
      context.commit('setLoading', {
        loading: {delete: true}
      });
      return new Promise((resolve, reject) => {
        axiosInstance.delete('api/group', {
          params: {
            id
          }
        })
          .then(value => {
            context.commit('setLoading', {
              loading: {delete: false}
            });
            resolve(value.data);
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
