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

    public save(tipolatte: TipoLatte, isNew: boolean) {
        if (isNew)
            return axios.post('/api/tipilatte/create', tipolatte);
        else
            return axios.put('/api/tipilatte/update', tipolatte);
    }

}