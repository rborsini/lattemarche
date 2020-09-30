import { Widget } from './widget.model';

export class BollaModel {
    public Nome!: string;
    public X!: number;
    public Y!: number;
    public Z!: number;
    public Colore!: string;
}

export class SerieModel {
    public Id!: string;
    public Nome!: string;
    public Y_Axis!: number;
    public Valori!: number[];
    public Bolle!: BollaModel[];
}

export default class GraficoWidgetModel extends Widget {

    public ValoriAsseX: Array<string>;
    public Serie: Array<SerieModel>;


    constructor(ValoriAsseX:Array<string> = [], Serie:Array<SerieModel> = []) {
        super();

        this.Serie = Serie;
        this.ValoriAsseX = ValoriAsseX;
    }
    
    /**
     * toHighchartsSerie
     */
    public toHighchartsSerie() {
        return this.Serie.map(
            elem => ({
                name: elem.Nome,
                data: elem.Valori
            })
        );
    }

    /**
     * toHighchartsLabels
     */
    public toHighchartsLabels() {
        return this.ValoriAsseX;
    }

}