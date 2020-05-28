import Vue from 'vue';
import App from './index.vue';
import HighchartsVue from 'highcharts-vue';
import Highcharts from 'highcharts';
import hcnd from 'highcharts/modules/no-data-to-display';
hcnd(Highcharts);

Vue.use(HighchartsVue, {
	highcharts: Highcharts
})

new Vue({
  render: h => h(App),
}).$mount('#app')
