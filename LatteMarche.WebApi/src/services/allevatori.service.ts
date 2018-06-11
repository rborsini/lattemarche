import axios, { AxiosPromise } from 'axios';
import { Allevatore } from '../models/allevatore.model';

export class AllevatoriServices {
    constructor() { }

    public getUtenti(): AxiosPromise<Allevatore[]> {
        return axios.get('/api/utenti');
    }

    public update(allevatore: Allevatore): AxiosPromise<Allevatore> {
        return axios.post('/api/utenti/save', allevatore);
    }

    public getDetails(id: string): AxiosPromise<Allevatore> {
        return axios.get('/api/utenti/details?id=' + id);
    }

}