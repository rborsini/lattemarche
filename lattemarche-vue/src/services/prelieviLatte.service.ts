import axios, { AxiosPromise } from "axios";
import { PrelievoLatte, PrelieviLatteSearchModel } from "../models/prelievoLatte.model";
import { LaboratorioAnalisi } from "../models/laboratorioAnalisi.model";

export class PrelieviLatteService {

    constructor() { }

    public search(parameters?: PrelieviLatteSearchModel): AxiosPromise<PrelievoLatte[]> {
        return axios.get('/api/prelievilatte/Search', { params: parameters });
    }

    public details(id: string): AxiosPromise<PrelievoLatte> {
        return axios.get('/api/prelieviLatte/Details?id=' + id);
    }

    public save(prelievo: PrelievoLatte): AxiosPromise<PrelievoLatte> {
        return axios.post('/api/PrelieviLatte/save', prelievo);
    }

    public delete(idPrelievo: number): AxiosPromise<PrelievoLatte> {
        return axios.delete('/api/PrelieviLatte/delete?id=' + idPrelievo);
    }

}