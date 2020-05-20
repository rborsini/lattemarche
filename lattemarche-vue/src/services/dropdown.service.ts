import axios, { AxiosPromise } from 'axios';
import { Dropdown } from "@/models/dropdown.model";

export class DropdownService {
    constructor() { }

    public getAutocisterne(idTrasportatore: number): AxiosPromise<Dropdown> {
        return axios.get('/api/autocisterne/dropdown?idTrasportatore=' + idTrasportatore);
    }



}