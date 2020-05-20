import axios, { AxiosPromise } from 'axios';
import { Notification } from "../models/notification.model";

export class NotificationsService {

    constructor() {}

    public getAll(username: string): AxiosPromise<Notification[]> {
        return axios.get('/api/notifications/all?username=' + username);
    }

}