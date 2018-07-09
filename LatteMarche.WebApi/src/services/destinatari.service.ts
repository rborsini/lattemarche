import axios, { AxiosPromise } from 'axios';
import { Destinatario } from "../models/destinatario.model";

export class DestinatariService {
    constructor() { }

    public getDestinatari(): AxiosPromise<Destinatario[]> {
        return axios.get('/api/destinatari');
    }
}