import axios, { AxiosPromise } from 'axios';
import { Utente } from '../models/utente.model';

export class UtentiService {
    constructor() { }

    public getUtenti(): AxiosPromise<Utente[]> {
        return axios.get('/api/utenti');
    }

    public update(utente: Utente): AxiosPromise<Utente> {
        return axios.put('/api/utenti/update', utente);
    }

    public create(utente: Utente): AxiosPromise<Utente> {
        return axios.post('/api/utenti/create', utente);
    }

    public getDetails(id: string): AxiosPromise<Utente> {
        return axios.get('/api/utenti/details?id=' + id);
    }

}
