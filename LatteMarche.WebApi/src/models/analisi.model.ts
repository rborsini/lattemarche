export class Analisi {
    public Id: string = "";
    public CodiceProduttore: string = "";
    public NomeProduttore: string = "";
    public CodiceASL: string = "";
    public TipoLatte_Descr: string = "";

    public DataRapportoDiProva_Str: string = "";
    public DataAccettazione_Str: string = "";
    public DataPrelievo_Str: string = "";

    public Valori: ValoreAnalisi[] = [];      
}

export class ValoreAnalisi {
    public Id: number = 0;
    public Nome: string = "";
    public Uom: string = "";
    public Valore: string = "";
    public FuoriSoglia: boolean = false;
}

export class AnalisiSearchModel {
    public CodiceProduttore: string = "";
    public CodiceAsl: string = "";
}

export class AnalisiTable {
    public Cols: AnalisiCol[] = [];
    public Rows: AnalisiRow[] = [];
}

export class AnalisiCol {
    public Nome: string = "";
    public Uom: string = "";

    constructor(nome: string, uom: string) {
        this.Nome = nome;
        this.Uom = uom;
    }

}

export class AnalisiRow {
    public Cells: AnalisiCell[] = [];
}

export class AnalisiCell {
    public Valore: string = "";
    public FuoriSoglia: boolean = false;

    constructor(valore: string, fuoriSoglia: boolean = false) {
        this.Valore = valore;
        this.FuoriSoglia = fuoriSoglia;
    }

}