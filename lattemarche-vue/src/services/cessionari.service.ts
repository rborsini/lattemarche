import axios, { AxiosPromise } from 'axios';
import { Cessionario } from "../models/cessionario.model";

export class CessionariService {
    constructor() { }

    public index(): AxiosPromise<Cessionario[]> {
        return axios.get('/api/cessionari');
    }

    public details(id: number): AxiosPromise<Cessionario> {
        return axios.get('/api/cessionari/details?id=' + id);
    }

    public save(cessionario: Cessionario): AxiosPromise<Cessionario> {
        return axios.post('/api/cessionari/save', cessionario);
    }

    public delete(idCessionario: number): AxiosPromise<Cessionario> {
        return axios.delete('/api/cessionari/delete?id=' + idCessionario);
    }

}