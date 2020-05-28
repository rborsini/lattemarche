import axios, { AxiosPromise } from 'axios';
import SommarioWidgetModel from "@/models/sommarioWidget.model";
import GraficoWidgetModel from "@/models/graficoWidget.model";

export default class DashboardService {
    constructor() { }

    public sommario(): AxiosPromise<SommarioWidgetModel> {
        return axios.get('/api/widgets/sommario');
    }

    public acquirenti(): AxiosPromise<GraficoWidgetModel> {
        return axios.get('/api/widgets/acquirenti');
    }

    public tipiLatte(): AxiosPromise<GraficoWidgetModel> {
        return axios.get('/api/widgets/tipiLatte');
    }
}