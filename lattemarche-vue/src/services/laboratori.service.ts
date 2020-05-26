import axios, { AxiosPromise } from 'axios';
import { Laboratorio } from "../models/laboratorio.model";

export class LaboratoriService {
    constructor() { }

    public index(): AxiosPromise<Laboratorio[]> {
        return axios.get('/api/labratori');
    }

    public details(id: number): AxiosPromise<Laboratorio> {
        return axios.get('/api/labratori/details?id=' + id);
    }

    public save(laboratorio: Laboratorio): AxiosPromise<Laboratorio> {
        return axios.post('/api/labratori/save', laboratorio);
    }

    public delete(idLaboratorio: number): AxiosPromise<Laboratorio> {
        return axios.delete('/api/labratori/delete?id=' + idLaboratorio);
    }

}