import axios, { AxiosPromise } from "axios";
import { PrelievoLatte } from "../models/prelievoLatte.model";
import { LaboratorioAnalisi } from "../models/laboratorioAnalisi.model";

export class PrelieviLatteService {

    constructor() { }

    public getPrelievi(id: string, dataInizio: string, dataFine: string): AxiosPromise<PrelievoLatte[]> {
        var url = '/api/PrelieviLatte/Search?idAllevamento=' + id;
        url += '&dal=' + dataInizio;
        url += '&al=' + dataFine;
        return axios.get(url);
    }

    public getLaboratoriAnalisi(): AxiosPromise<LaboratorioAnalisi> {
        return axios.get('/api/laboratorianalisi');
    }
}