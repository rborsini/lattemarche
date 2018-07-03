import axios, { AxiosPromise } from 'axios';
import { Trasportatore } from '../models/trasportatore.model';

export class TrasportatoreService {
    constructor() { }

    public getTrasportatori(): AxiosPromise<Trasportatore> {
        return axios.get('/api/trasportatori');
    }
}