import axios, { AxiosPromise } from 'axios';
import { Laboratorio } from "../models/laboratorio.model";

export class LaboratoriService {
    constructor() { }

    public index(): AxiosPromise<Laboratorio[]> {
        return axios.get('/api/laboratoriAnalisi');
    }

    public details(id: number): AxiosPromise<Laboratorio> {
        return axios.get('/api/laboratoriAnalisi/details?id=' + id);
    }

    public save(laboratorio: Laboratorio): AxiosPromise<Laboratorio> {
        return axios.post('/api/laboratoriAnalisi/save', laboratorio);
    }

    public delete(idLaboratorio: number): AxiosPromise<Laboratorio> {
        return axios.delete('/api/laboratoriAnalisi/delete?id=' + idLaboratorio);
    }

}