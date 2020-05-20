import axios, { AxiosPromise } from 'axios';
import { ViewItem } from '@/models/viewItem.model';

export class ViewItemsService {
    constructor() { }

    public index(): AxiosPromise<ViewItem[]> {
        return axios.get('/api/viewItems');
    }

}
