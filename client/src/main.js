import Vue from 'vue'
import BootstrapVue from 'bootstrap-vue'
import axios from 'axios'
import router from './router'
import store from './store'
import App from './App'
import QuestionHub from './question-hub'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import '@fortawesome/fontawesome-free/css/all.css'

Vue.config.productionTip = false

// By default the project simulates the client application being hosted independently from the server
// These lines setup axios so all requests are sent to the backend server
// However, you can comment them and the site will behave as if both client and server were hosted in localhost:8080
// due to the proxy dev server configured in vue.config.js
axios.defaults.baseURL = 'http://localhost:5100' // same as the Url the server listens to
axios.defaults.withCredentials = true

// Include the Authentication header when using JWT authentication
axios.interceptors.request.use(request => {
  if (store.state.context.jwtToken) request.headers['Authorization'] = 'Bearer ' + store.state.context.jwtToken
  return request
})

// Setup axios as the Vue default $http library
Vue.prototype.$http = axios

// Install Vue extensions
Vue.use(BootstrapVue)
Vue.use(QuestionHub)

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
