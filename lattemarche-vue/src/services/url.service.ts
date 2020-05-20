export class UrlService {

    // recupero parametro lato server '&'
    public static getUrlParameter(paramName: string): string {

        var sPageURL: string = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        var p: any = sURLVariables.find(v => v.split('=')[0] == paramName);

        if (p != undefined)
            return decodeURIComponent(p.split('=')[1]);
        else
            return '';

    };

    // redirect verso un nuovo indirizzo
    public static redirect(path: string) {
        window.location.assign(path);
    }

    // ricarica pagina corrente
    public static reload() {
        window.location = window.location;
    }

    // Set parametro hash
    public static setHashParamareter(name: string, value: string) {

        var data: any = UrlService.getHashObj();
        data[name] = value;

        var fieldNames: string[] = Object.getOwnPropertyNames(data).filter(pn => pn != '');
        var params: string[] = [];

        for (var i = 0; i < fieldNames.length; i++) {
            params.push(fieldNames[i] + '=' + data[fieldNames[i]]);
        }

        window.location.hash = '#' + params.join('&');

    }

    // Recupero parametro hash
    public static getHashParamareter(name: string): string {
        var data: any = UrlService.getHashObj();
        return data[name];
    }

    // conversione hash in oggetto generico
    private static getHashObj(): any {

        var data: any = {};
        var parts: any[] = [];

        var pieces = window.location.hash.replace('#', '').split("&");

        // process each query pair
        for (var i = 0; i < pieces.length; i++) {
            parts = pieces[i].split("=");
            if (parts.length < 2) {
                parts.push("");
            }
            data[parts[0]] = decodeURIComponent(parts[1]);
        }

        return data;

    }


}