import Vue from 'vue'
import App from './new.vue'

new Vue({
  render: h => h(App),
}).$mount('#app')

Vue.config.devtools = true;
