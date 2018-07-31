import axios, { AxiosPromise } from 'axios';
import { Acquirente } from "../models/acquirente.model";

export class AcquirentiService {
    constructor() { }

    public getAcquirenti(): AxiosPromise<Acquirente[]> {
        return axios.get('/api/acquirenti');
    }

    public getDetails(id: number): AxiosPromise<Acquirente> {
        return axios.get('/api/acquirenti/details?id=' + id);
    }

    public update(acquirente: Acquirente): AxiosPromise<Acquirente> {
        return axios.put('/api/Acquirenti/update', acquirente);
    }

}