import { Role } from './role.model'
export class User {
    public Username: string = "";
    public FirstName: string = "";
    public LastName: string = "";
    public Email: string = "";
    public Active: boolean = false;
    public Roles: Role[] = [];
    public RolesStr: string = "";
}