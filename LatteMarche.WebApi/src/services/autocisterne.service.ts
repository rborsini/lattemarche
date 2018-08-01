import axios, { AxiosPromise } from 'axios';
import { Autocisterna } from "../models/autocisterna.model";

export class AutocisterneService {
    constructor() { }

    public getAutocisterne(): AxiosPromise<Autocisterna[]> {
        return axios.get('/api/autocisterne');
    }

    public getDetails(id: number): AxiosPromise<Autocisterna> {
        return axios.get('/api/autocisterne/details?id=' + id);
    }

    public update(autocisterna: Autocisterna): AxiosPromise<Autocisterna> {
        return axios.put('/api/autocisterne/update', autocisterna);
    }

}