import axios, { AxiosPromise } from 'axios';
import { AttachmentCategory } from '@/models/attachmentCategory.model';

export class AttachmentCategoriesService {
    constructor() { }

    public index(page: string = ''): AxiosPromise<AttachmentCategory[]> {
        return axios.get('/api/attachmentcategories?page=' + page);
    }

    public details(id: string): AxiosPromise<AttachmentCategory> {
        return axios.get('/api/attachmentcategories/details?id=' + id);
    }

    public save(category: AttachmentCategory): AxiosPromise<AttachmentCategory> {
        return axios.post('/api/attachmentcategories/save', category);
    }

    public delete(id: number): AxiosPromise<AttachmentCategory> {
        return axios.delete('/api/attachmentcategories/delete?id=' + id);
    }

}