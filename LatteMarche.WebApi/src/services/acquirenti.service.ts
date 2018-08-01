import axios, { AxiosPromise } from 'axios';
import { Acquirente } from "../models/acquirente.model";

export class AcquirentiService {
    constructor() { }

    public index(): AxiosPromise<Acquirente[]> {
        return axios.get('/api/acquirenti');
    }

    public details(id: number): AxiosPromise<Acquirente> {
        return axios.get('/api/acquirenti/details?id=' + id);
    }

    public save(acquirente: Acquirente): AxiosPromise<Acquirente> {
        return axios.post('/api/Acquirenti/save', acquirente);
    }

    public delete(idAcquirente: number): AxiosPromise<Acquirente> {
        return axios.delete('/api/Acquirenti/delete?id=' + idAcquirente);
    }

}