import axios, { AxiosPromise } from 'axios';
import { Attachment } from '../models/attachment.model';

export class AttachmentsService {
    constructor() { }

    public delete(id: number): AxiosPromise<Attachment> {
        return axios.delete('/api/attachments/delete?id=' + id);
    }

}