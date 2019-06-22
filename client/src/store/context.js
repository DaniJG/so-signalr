import Vue from 'vue'
import axios from 'axios'

const store = {
  namespaced: true,

  state: {
    profile: {}
  },

  getters: {
    isAuthenticated: state => state.profile.name && state.profile.email
  },

  mutations: {
    setProfile (state, profile) {
      state.profile = profile
    }
  },

  actions: {
    loadProfile ({ commit, getters }) {
      return axios.get('account/context').then(res => {
        commit('setProfile', res.data)
        if (getters.isAuthenticated) return Vue.prototype.startSignalR()
      })
    },
    login ({ commit }, credentials) {
      return axios.post('account/login', credentials).then(res => {
        commit('setProfile', res.data)
        return Vue.prototype.startSignalR()
      })
    },
    logout ({ commit }) {
      return axios.post('account/logout').then(() => {
        commit('setProfile', {})
        return Vue.prototype.stopSignalR()
      })
    }
  }
}

export default store
