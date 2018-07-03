import { TipoLatte } from "../models/tipoLatte.model"
import axios, { AxiosPromise } from 'axios';

export class TipiLatteService {

    constructor() { }

    public getTipiLatte(): AxiosPromise<TipoLatte> {
        return axios.get('/api/tipilatte');
    }

    public getTipoLatte(id: string): AxiosPromise<TipoLatte> {
        return axios.get('/api/tipilatte/details?id=' + id);
    }

    //TODO
    public create(tipolatte: TipoLatte): AxiosPromise<TipoLatte> {
        return axios.post('/api/tipilatte/create', tipolatte);
    }

    public update(tipolatte: TipoLatte): AxiosPromise<TipoLatte> {
        return axios.put('/api/utenti/update', tipolatte);
    }

}