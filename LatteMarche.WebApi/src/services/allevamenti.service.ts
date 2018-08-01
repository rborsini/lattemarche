import axios, { AxiosPromise } from 'axios';
import { Allevamento } from '../models/allevamento.model';

export class AllevamentiService {
    constructor() { }

    public index(): AxiosPromise<Allevamento[]> {
        return axios.get('/api/allevamenti');
    }

    public details(id: string): AxiosPromise<Allevamento> {
        return axios.get('/api/allevamenti/details?id=' + id);
    }

    public save(allevatore: Allevamento): AxiosPromise<Allevamento> {
        return axios.post('/api/allevamenti/save', allevatore);
    }

    public delete(idAllevamento: number): AxiosPromise<Allevamento> {
        return axios.delete('/api/allevamenti/delete?id=' + idAllevamento);
    }

}