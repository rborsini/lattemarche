import { Page } from './page.model';
import { User } from './user.model';

export class Role {
    public Id: number = 0;
    public Code: string = "";
    public Description: string = "";
    public MVC_Pages: Page[] = [];
    public API_Pages: Page[] = [];
    public Users: User[] = [];
}