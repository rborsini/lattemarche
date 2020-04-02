import axios, { AxiosPromise } from 'axios';
import { AnalisiSearchModel, Analisi } from '../models/analisi.model';

export class AnalisiService {
    constructor() { }

    public search(parameters?: AnalisiSearchModel): AxiosPromise<Analisi[]> {
        return axios.get('/api/analisi/search', { params: parameters });
    }

    public synch(): AxiosPromise<void> {
        return axios.post('/api/analisi/synch');
    }

}