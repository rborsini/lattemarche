import axios, { AxiosPromise } from 'axios';
import { Autocisterna } from "../models/autocisterna.model";

export class AutocisterneService {
    constructor() { }

    public index(): AxiosPromise<Autocisterna[]> {
        return axios.get('/api/autocisterne');
    }

    public details(id: number): AxiosPromise<Autocisterna> {
        return axios.get('/api/autocisterne/details?id=' + id);
    }

    public save(autocisterna: Autocisterna): AxiosPromise<Autocisterna> {
        return axios.post('/api/autocisterne/save', autocisterna);
    }

    public delete(idAutocisterna: number): AxiosPromise<Autocisterna> {
        return axios.delete('/api/autocisterne/delete?id=' + idAutocisterna);
    }

}