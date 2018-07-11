import axios, { AxiosPromise } from 'axios';
import { Ruolo } from '../models/ruolo.model';

export class RuoliService {
    constructor() { }

    public getRuoli(): AxiosPromise<Ruolo[]> {
        return axios.get('/api/ruoli');
    }

    public getDetails(id: string): AxiosPromise<Ruolo> {
        return axios.get('/api/ruoli/details?id=' + id);
    }

    public update(ruolo: Ruolo): AxiosPromise<Ruolo> {
        return axios.put('/api/ruoli/update', ruolo);
    }

    public create(ruolo: Ruolo): AxiosPromise<Ruolo> {
        return axios.post('/api/ruoli/create', ruolo);
    }

}
