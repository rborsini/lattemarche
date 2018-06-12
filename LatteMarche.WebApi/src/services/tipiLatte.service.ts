import { TipoLatte } from "../models/tipoLatte.model"
import axios, { AxiosPromise } from 'axios';

export class TipiLatteService {

    constructor() { }

    public getTipiLatte(): AxiosPromise<TipoLatte> {
        return axios.get('/api/TipiLatte');
    }

}