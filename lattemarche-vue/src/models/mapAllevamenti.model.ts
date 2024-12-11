export class WidgetMap {
    public Markers: Marker[] = [];
    public AcquirentiLegend: ColorLegend[] = [];
    public TipiLatteLegend: ColorLegend[] = [];
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
    public IdAcquirente?: number = 0;
    public IdTipoLatte?: number = 0;
}