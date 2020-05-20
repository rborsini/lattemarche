import Vue from 'vue'
import App from './pages/signup/index.vue'

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
}).$mount('#app')
