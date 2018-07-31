import axios, { AxiosPromise } from "axios";
import { PrelievoLatte } from "../models/prelievoLatte.model";
import { LaboratorioAnalisi } from "../models/laboratorioAnalisi.model";

export class PrelieviLatteService {

    constructor() { }

    public getPrelievi(id: string, dataInizio: string, dataFine: string): AxiosPromise<PrelievoLatte[]> {
        var url = '/api/prelievilatte/Search?idAllevamento=' + id;
        url += '&dal=' + dataInizio;
        url += '&al=' + dataFine;
        return axios.get(url);
    }

    public getPrelievo(id: string): AxiosPromise<PrelievoLatte> {
        return axios.get('/api/prelieviLatte/Details?id=' + id);
    }

    public getLaboratoriAnalisi(): AxiosPromise<LaboratorioAnalisi[]> {
        return axios.get('/api/laboratorianalisi');
    }

    public save(prelievo: PrelievoLatte): AxiosPromise<PrelievoLatte> {
        return axios.post('/api/PrelieviLatte/save', prelievo);
    }

}