import { TrasbordiSearchModel, Trasbordo } from "@/models/trasbordo.model";
import axios, { AxiosPromise } from "axios";

export class TrasbordiService {

    public search(parameters?: TrasbordiSearchModel): AxiosPromise<Trasbordo[]> {
        return axios.get('/api/trasbordi/Search', { params: parameters });
    }

    public details(id: string): AxiosPromise<Trasbordo> {
        return axios.get('/api/trasbordi/Details?id=' + id);
    }

}