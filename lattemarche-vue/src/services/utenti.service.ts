import axios, { AxiosPromise } from 'axios';
import { Utente } from '../models/utente.model';
import { ChangePassword } from '@/models/changePassword.model';

export class UtentiService {
    constructor() { }

    public index(idProfilo?: number): AxiosPromise<Utente[]> {
        
        if(idProfilo)
            return axios.get('/api/utenti/search?idProfilo=' + idProfilo);
        else
            return axios.get('/api/utenti');
    }

    public details(id: string): AxiosPromise<Utente> {
        return axios.get('/api/utenti/details?id=' + id);
    }

    public save(utente: Utente): AxiosPromise<Utente> {
        return axios.post('/api/utenti/save', utente);
    }

    public delete(id: number): AxiosPromise<Utente> {
        return axios.delete('/api/utenti/delete?id=' + id);
    }

    public changePassword(model: ChangePassword): AxiosPromise<string> {
        return axios.post('/api/utenti/changepassword', model);
    }

}
