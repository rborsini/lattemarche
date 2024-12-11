import axios, { AxiosPromise } from 'axios';
import SommarioWidgetModel from "@/models/sommarioWidget.model";
import GraficoWidgetModel from "@/models/graficoWidget.model";
import { AnalisiQuantitativaWidget } from '@/models/analisiQuantitativaWidget.model';
import { AnalisiQualitativaWidget } from '@/models/analisiQualitativaWidget.model';
import { AnalisiComparativaWidget } from '@/models/analisiComparativaWidget.model';
import { WidgetMap, WidgetMapSearchModel } from '@/models/mapAllevamenti.model';

export default class WidgetsService {
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

    public analisiQuantitativa(idAllevamento: number, da: string, a: string): AxiosPromise<AnalisiQuantitativaWidget> {
        return axios.get('/api/widgets/analisiQuantitativa?idAllevamento=' + idAllevamento + '&da=' + da + '&a=' + a);
    }

    public analisiQualitativa(idAllevamento: number, da: string, a: string): AxiosPromise<AnalisiQualitativaWidget> {
        return axios.get('/api/widgets/analisiQualitativa?idAllevamento=' + idAllevamento + '&da=' + da + '&a=' + a);
    }
    
    public analisiComparativa(idAllevamento: number, da: string, a: string): AxiosPromise<AnalisiComparativaWidget> {
        return axios.get('/api/widgets/analisiComparativa?idAllevamento=' + idAllevamento + '&da=' + da + '&a=' + a);
    }

    public analisiMappa(searchModel: WidgetMapSearchModel): AxiosPromise<WidgetMap> {
        return axios.post('/api/widgets/analisiMappa', searchModel);
    }    
}