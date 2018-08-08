import { TipoLatte } from "../models/tipoLatte.model"
import axios, { AxiosPromise } from 'axios';

export class TipiLatteService {

    constructor() { }

    public index(): AxiosPromise<TipoLatte[]> {
        return axios.get('/api/tipilatte');
    }

    public details(id: string): AxiosPromise<TipoLatte> {
        return axios.get('/api/tipilatte/details?id=' + id);
    }

    public save(tipolatte: TipoLatte): AxiosPromise<TipoLatte> {
        return axios.post('/api/tipilatte/save', tipolatte);
    }

    public delete(idTipoLatte: number): AxiosPromise<TipoLatte> {
        return axios.delete('/api/tipilatte/delete?id=' + idTipoLatte);
    }
}