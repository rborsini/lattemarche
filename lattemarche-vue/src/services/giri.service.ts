import axios, { AxiosPromise } from 'axios';
import { Giro } from '../models/giro.model';

export class GiriService {
    constructor() { }

    public getGiroDetails(id: number): AxiosPromise<Giro> {
        return axios.get('/api/giri/details?id=' + id);
    }

    public save(giro: Giro): AxiosPromise<Giro> {
        return axios.post('/api/giri/save', giro);
    }

}