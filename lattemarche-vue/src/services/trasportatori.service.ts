import axios, { AxiosPromise } from 'axios';
import { Trasportatore } from '../models/trasportatore.model';
import { DropdownItem, Dropdown } from "../models/dropdown.model";

export class TrasportatoriService {
    constructor() { }

    public getTrasportatori(): AxiosPromise<Trasportatore[]> {
        return axios.get('/api/trasportatori');
    }

    public getTrasportatoreDetails(idTrasportatore: string): AxiosPromise<Trasportatore> {
        var url = '/api/trasportatori/details?id=';
        url += idTrasportatore;
        return axios.get(url);
    }

}