export class Giro {
    public Id: number = 0;
    public Denominazione: string = "";
    public CodiceGiro: string = "";
    public IdTrasportatore: number = 0;
    public Items?: Items[] = [];
}

export class Items {
    public IdGiro: number = 0;
    public IdAllevamento: number = 0;
    public Allevatore: string = "";
    public RagioneSociale: string = "";
    public Indirizzo: string = "";
    public BoolPriorita: boolean = false;
    public Priorita: number = 0;
}