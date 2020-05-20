import Vue from 'vue'
import App from './index.vue'
import HighchartsVue from 'highcharts-vue'

Vue.use(HighchartsVue);

new Vue({
  render: h => h(App),
}).$mount('#app')

