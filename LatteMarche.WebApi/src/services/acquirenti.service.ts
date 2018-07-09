import axios, { AxiosPromise } from 'axios';
import { Acquirente } from "../models/acquirente.model";

export class AcquirentiService {
    constructor() { }

    public getAcquirenti(): AxiosPromise<Acquirente[]> {
        return axios.get('/api/acquirenti');
    }
}