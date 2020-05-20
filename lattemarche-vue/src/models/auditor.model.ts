import { Skill } from './skill.model';
import { AssigneeRate } from './assigneeRate.model';

export class Auditor {
    public Id: number = 0;
    public FirstName: string = "";
    public LastName: string = "";
    public UserId: string = "";
    public IsDefault: boolean = false;

    public Skills: Skill[] = [];
    public Rates: AssigneeRate[] = [];

}