import { Comune } from "../models/comune.model";
import { DropdownItem, Dropdown } from "../models/dropdown.model";
import axios, { AxiosPromise } from "axios";

export class ComuniService {

    constructor() { }

    public getComuni(idProvincia: string): AxiosPromise<Comune[]> {
        var url = '/api/comuni';
        if (idProvincia != '') {
            url += '/search?provincia=';
            url += idProvincia;
        }
        return axios.get(url);
    }

    public getComuneDetails(idComune: string): AxiosPromise<Comune> {
        var url = '/api/comuni/details?id=';
        url += idComune;
        return axios.get(url);
    }

    public getProvince(): AxiosPromise<DropdownItem[]> {
        return axios.get('/api/comuni/province');
    }



}