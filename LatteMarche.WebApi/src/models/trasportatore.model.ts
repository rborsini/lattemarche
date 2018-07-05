import { Giro } from "./giro.model";

export class Trasportatore {
    public Id: number = 0;
    public Nome: string = "";
    public Cognome: string = "";
    public Indirizzo: string = "";
    public Telefono: string = "";
    public Cellulare: string = "";
    public Comune: string = "";
    public Provincia: string = "";
    public Giri?: Giro[] = [];
}