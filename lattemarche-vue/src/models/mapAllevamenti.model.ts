export class WidgetMap {
    public Markers: Marker[] = [];
    public Legend: ColorLegend[] = [];
}

export class Marker {
    public Lat!: number;
    public Lng!: number;
    public Allevamento!: string;
    public Acquirente!: string;
    public TipoLatte!: string;
}

export class ColorLegend {
    public Color!: string;
    public Label!: string;
}

export class WidgetMapSearchModel {
    public DataInizio_Str: string = "";
    public DataFine_Str: string = "";    
    public IdAcquirente?: number = 0;
    public IdTipoLatte?: number = 0;
    public CodiceGiro: string = "";
    public IdTrasportatore?: number = 0;
    public AggregazioneColore: string = "giro";
}