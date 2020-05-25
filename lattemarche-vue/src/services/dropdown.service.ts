import axios, { AxiosPromise } from 'axios';
import { Dropdown } from "@/models/dropdown.model";

export class DropdownService {
    constructor() { }

    public getAutocisterne(idTrasportatore: number): AxiosPromise<Dropdown> {
        return axios.get('/api/autocisterne/dropdown?idTrasportatore=' + idTrasportatore);
    }

    public getProvince(): AxiosPromise<Dropdown> {
        return axios.get('/api/comuni/province');
    }

    public getComuni(siglaProvincia: string): AxiosPromise<Dropdown> {
        return axios.get('/api/comuni/dropdown?siglaProvincia=' + siglaProvincia);
    }

    public getProfili(): AxiosPromise<Dropdown> {
        return axios.get("/api/tipiProfilo/dropdown");
    }

    public getAcquirenti(): AxiosPromise<Dropdown> {
        return axios.get("/api/acquirenti/dropdown");
    }

    public getDestinatari(): AxiosPromise<Dropdown> {
        return axios.get("/api/destinatari/dropdown");
    }

    public getCessionari(): AxiosPromise<Dropdown> {
        return axios.get("/api/cessionari/dropdown");
    }

    public getAllevamenti(): AxiosPromise<Dropdown> {
        return axios.get("/api/allevamenti/dropdown");
    }

    public getTipiLatte(): AxiosPromise<Dropdown> {
        return axios.get("/api/tipiLatte/dropdown");
    }

    public getTrasportatori(): AxiosPromise<Dropdown> {
        return axios.get("/api/aziendeTrasportatori/dropdown");
    }

}