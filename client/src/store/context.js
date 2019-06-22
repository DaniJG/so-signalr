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
    loadProfile ({ commit }) {
      return axios.get('account/context').then(res => {
        commit('setProfile', res.data)
      })
    },
    login ({ commit }, credentials) {
      return axios.post('account/login', credentials).then(res => {
        commit('setProfile', res.data)
      })
        .then(() => axios.post('account/token', credentials))
        .then(res => console.log(res.data))
        .then(() => axios.get('account/context'))
        .then(res => console.log(res.data))
    },
    logout ({ commit }) {
      return axios.post('account/logout').then(() => {
        commit('setProfile', {})
      })
    }
  }
}

export default store
