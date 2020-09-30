export class DateService {
    constructor() {}

    public static subtractMonth(date: Date): Date {
        var days = 0;
        //get month ritorna un numero da 0 a 11. Dovendo considerare il mese precedente
        //da sottrarre, per comodità aggiungo un numero al case, considerando quindi lo 0 come dicembre
        switch (date.getMonth()) {
            case 4: //Aprile
            case 6: //Giugno
            case 9: //Settembre
            case 11: {
            //Novembre
            days = 30;
            break;
            }
            case 2: {
            //febbraio
            if (date.getFullYear() % 4 != 0) {
                //anno non bisestile
                days = 28;
            } else {
                days = 29;
            }
            break;
            }
            default: //altri mesi
            {
            days = 31;
            break;
            }
        }
        date.setDate(date.getDate() - days);
        return date;
    }

    public static formatDate(date: Date): string {
        var returnDate = "";
    
        var dd = date.getDate();
        var mm = date.getMonth() + 1; //because January is 0!
        var yyyy = date.getFullYear();
    
        if (dd < 10) {
          returnDate += `0${dd}-`;
        } else {
          returnDate += `${dd}-`;
        }
    
        if (mm < 10) {
          returnDate += `0${mm}-`;
        } else {
          returnDate += `${mm}-`;
        }
        returnDate += yyyy;
        return returnDate;
      }    

}