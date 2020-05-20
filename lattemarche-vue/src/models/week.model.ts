export class Week {
    public Index: number = 0;
    public WeekNumber: number = 0
    public Days: Date [] = [];
    public DaysStr: string[] = [];
    public WeekAndDateForSelectBox: string = "";

    constructor () {
        this.DaysStr[0] = 'Lunedì';
        this.DaysStr[1] = 'Martedì';
        this.DaysStr[2] = 'Mercoledì';
        this.DaysStr[3] = 'Giovedì';
        this.DaysStr[4] = 'Venerdì';
        this.DaysStr[5] = 'Sabato';
        this.DaysStr[6] = 'Domenica';
    }
}