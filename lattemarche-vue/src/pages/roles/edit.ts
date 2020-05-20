import Vue from 'vue'
import App from './edit.vue'

new Vue({
  render: h => h(App, { props: { aa: 'abc' } }),
}).$mount('#app')

Vue.config.devtools = true;
