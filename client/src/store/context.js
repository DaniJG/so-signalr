import Vue from 'vue'
import axios from 'axios'

const store = {
  namespaced: true,

  state: {
    profile: {},
    jwtToken: null
  },

  getters: {
    isAuthenticated: state => state.profile.name && state.profile.email
  },

  mutations: {
    setProfile (state, profile) {
      state.profile = profile
    },
    setJwtToken (state, jwtToken) {
      state.jwtToken = jwtToken
      // WARNING: storing JWT tokens in local storage or anywhere accessible by JavaScript is a security issue!
      // This is just for demonstrative purposes on how SignalR can be authenticated using JWT
      // Store token securily on a real application
      // For example, a web app can store them in cookies not accessible to JavaScript, while a native app will need to use the device secure storage
      if (jwtToken) window.localStorage.setItem('jwtToken', jwtToken)
      else window.localStorage.removeItem('jwtToken')
    }
  },

  actions: {
    // Used during startup to reload the profile from the server
    restoreContext ({ commit, getters, state }) {
      const jwtToken = window.localStorage.getItem('jwtToken')
      if (jwtToken) commit('setJwtToken', jwtToken)

      return axios.get('account/context').then(res => {
        commit('setProfile', res.data)
        if (getters.isAuthenticated) return Vue.prototype.startSignalR(state.jwtToken)
      })
    },
    // Login methods. Either use cookie-based auth or jwt-based auth
    login ({ state, dispatch }, { authMethod, credentials }) {
      const loginAction = authMethod === 'jwt'
        ? dispatch('loginToken', credentials)
        : dispatch('loginCookies', credentials)

      return loginAction.then(() => Vue.prototype.startSignalR(state.jwtToken))
    },
    loginCookies ({ commit }, credentials) {
      return axios.post('account/login', credentials).then(res => {
        commit('setProfile', res.data)
      })
    },
    loginToken ({ commit }, credentials) {
      return axios.post('account/token', credentials).then(res => {
        const profile = res.data
        const jwtToken = res.data.token
        delete profile.token
        commit('setProfile', profile)
        commit('setJwtToken', jwtToken)
      })
    },
    // Logout. (With JWT the request isnt strictly necessary unless the server needs some cleanup/auditing)
    logout ({ commit, state }) {
      const logoutAction = state.jwtToken
        ? Promise.resolve()
        : axios.post('account/logout')

      return logoutAction.then(() => {
        commit('setProfile', {})
        commit('setJwtToken', null)
        return Vue.prototype.stopSignalR()
      })
    }
  }
}

export default store
