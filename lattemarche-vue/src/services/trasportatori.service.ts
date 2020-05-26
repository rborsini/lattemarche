import axios, { AxiosPromise } from 'axios';
import { Trasportatore } from '../models/trasportatore.model';
import { AziendaTrasportatore } from '../models/aziendaTrasportatore.model';

export class TrasportatoriService {
    constructor() { }

    public index(): AxiosPromise<AziendaTrasportatore[]> {
        return axios.get('/api/aziendeTrasportatori');
    }

    public details(id: number): AxiosPromise<AziendaTrasportatore> {
        return axios.get('/api/aziendeTrasportatori/details?id=' + id);
    }

    public save(aziendaTrasportatore: AziendaTrasportatore): AxiosPromise<AziendaTrasportatore> {
        return axios.post('/api/aziendeTrasportatori/save', aziendaTrasportatore);
    }

    public delete(idAziendaTrasportatore: number): AxiosPromise<AziendaTrasportatore> {
        return axios.delete('/api/aziendeTrasportatori/delete?id=' + idAziendaTrasportatore);
    }



    public getTrasportatori(): AxiosPromise<Trasportatore[]> {
        return axios.get('/api/trasportatori');
    }

    public getTrasportatoreDetails(idTrasportatore: string): AxiosPromise<Trasportatore> {
        var url = '/api/trasportatori/details?id=';
        url += idTrasportatore;
        return axios.get(url);
    }

}