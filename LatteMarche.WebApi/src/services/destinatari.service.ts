import axios, { AxiosPromise } from 'axios';
import { Destinatario } from "../models/destinatario.model";

export class DestinatariService {
    constructor() { }

    public getDestinatari(): AxiosPromise<Destinatario[]> {
        return axios.get('/api/destinatari');
    }

    public getDetails(id: number): AxiosPromise<Destinatario> {
        return axios.get('/api/destinatari/details?id=' + id);
    }

    public update(destinatario: Destinatario): AxiosPromise<Destinatario> {
        return axios.put('/api/destinatari/update', destinatario);
    }

}