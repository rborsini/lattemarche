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

    //public save(tipolatte: TipoLatte, isNew: boolean) {
    //    if (isNew)
    //        return axios.post('/api/tipilatte/create', tipolatte);
    //    else
    //        return axios.put('/api/tipilatte/update', tipolatte);
    //}

    public save(tipolatte: TipoLatte): AxiosPromise<TipoLatte> {
        return axios.post('/api/autocisterne/save', tipolatte);
    }

    public delete(idTipoLatte: number): AxiosPromise<TipoLatte> {
        return axios.delete('/api/autocisterne/delete?id=' + idTipoLatte);
    }
}