import axios, { AxiosPromise } from 'axios';
import { Profilo } from '../models/profilo.model';

export class ProfiliService {
    public getProfili(): AxiosPromise<Profilo[]> {
        return axios.get('/api/tipiprofilo');
    }

    public getDetails(id: number): AxiosPromise<Profilo[]> {
        return axios.get('/api/tipiprofilo/details?id=' + id);
    }
}