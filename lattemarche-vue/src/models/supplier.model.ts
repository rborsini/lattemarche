import { Skill } from './skill.model';
import { AssigneeRate } from './assigneeRate.model';
import { Auditor } from './auditor.model';

export class Supplier {
    public Id: number = 0;
    public BusinessName: string="";
    public Address: string="";
    public PostalCode: string="";
    public City: string="";
    public Province: string="";
    public VAT_Number: string="";
    public FiscalCode: string="";
    public Phone: string="";
    public Fax: string="";
    public CellPhone: string="";
    public Email: string="";
    public PEC: string="";
    public PaymentCondition: string="";

    public Skills: Skill[] = [];
    public Rates: AssigneeRate[] = [];
    public Auditors: Auditor[] = [];
}

