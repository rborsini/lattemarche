import axios, { AxiosPromise } from 'axios';
import { Destinatario } from "../models/destinatario.model";

export class DestinatariService {
    constructor() { }

    public index(): AxiosPromise<Destinatario[]> {
        return axios.get('/api/destinatari');
    }

    public details(id: number): AxiosPromise<Destinatario> {
        return axios.get('/api/destinatari/details?id=' + id);
    }

    public save(destinatario: Destinatario): AxiosPromise<Destinatario> {
        return axios.post('/api/destinatari/save', destinatario);
    }

    public delete(idDestinatario: number): AxiosPromise<Destinatario> {
        return axios.delete('/api/destinatari/delete?id=' + idDestinatario);
    }

}