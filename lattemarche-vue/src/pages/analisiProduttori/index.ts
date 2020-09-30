import Vue from 'vue'
import App from './index.vue'

import Highcharts from 'highcharts'
import HighchartsVue from 'highcharts-vue'
import highchartsMore from 'highcharts/highcharts-more'

highchartsMore(Highcharts)
Vue.use(HighchartsVue, {Highcharts})


new Vue({
  render: h => h(App),
}).$mount('#app')
