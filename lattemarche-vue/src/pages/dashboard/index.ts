import Vue from 'vue';
import VueI18n from 'vue-i18n';
import App from './index.vue';
import HighchartsVue from 'highcharts-vue';
import Highcharts from 'highcharts';
import hcnd from 'highcharts/modules/no-data-to-display';
hcnd(Highcharts);

Vue.use(HighchartsVue, {
	highcharts: Highcharts
})

Vue.use(VueI18n);

const i18n = new VueI18n({
  locale: "it",
  fallbackLocale: "it",
});

new Vue({
  i18n,
  render: h => h(App),
}).$mount('#app')
