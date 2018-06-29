import { Pagina } from './pagina.model';
import { Utente } from './utente.model';

export class Ruolo {
    public Id: number = 0;
    public Codice: string = "";
    public Descrizione: string = "";
    public Pagine_MVC: Pagina[] = [];
    public Utenti: Utente[] = [];
}