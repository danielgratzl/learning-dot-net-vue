import Vue from 'vue'
import App from './App.vue'

import store from './store'
import axios from 'axios'
import router from './router'

Vue.prototype.$http = axios;
const token = localStorage.getItem('token')

if (token) {
  axios.defaults.headers.common['Authorization'] = token
}

Vue.config.productionTip = true

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
