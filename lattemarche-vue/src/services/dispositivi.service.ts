import axios, { AxiosPromise } from 'axios';
import { Dispositivo } from '../models/dispositivo.model';

export class DispositiviService {
    constructor() { }

    public index(): AxiosPromise<Dispositivo[]> {
        return axios.get('/api/dispositivi');
    }

    public details(id: string): AxiosPromise<Dispositivo> {
        return axios.get('/api/dispositivi/details?id=' + id);
    }

    public update(dispositivo: Dispositivo): AxiosPromise<Dispositivo> {
        return axios.put('/api/dispositivi/update', dispositivo);
    }

}
