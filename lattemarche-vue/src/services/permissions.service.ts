export class PermissionsService {
    constructor() {}

    public static isAuthenticated() : boolean {
        var jwt = localStorage.getItem('jwt') as string;
        return jwt != '';
    }

    public static isViewItemAuthorized(controller: string, action: string, viewItem: string, type: string = "MVC") :boolean {
        var permissions = this.getPermissions(controller, action, type);
        return permissions.indexOf(viewItem) != -1;
    }

    public static getPermissions(controller: string, action: string, type: string = "MVC"): string[] {

        var permissions = [];

        var jwt = localStorage.getItem('jwt') as string
        var obj = PermissionsService.decodeToken(jwt);

        var items: string[] = [];
        
        if(obj.permissions)
            items = obj.permissions.split('|') as string[];

        var prefix = type + '-' + controller + '-' + action + '-';

        var pageItems = items
            .filter(i => i.startsWith(prefix))
            .map(i => i.replace(prefix, ''));

        return pageItems;

    }

    public static getCurrentUser() {
        var jwt = localStorage.getItem('jwt') as string
        var obj = PermissionsService.decodeToken(jwt);
        return obj.username;
    }

    private static decodeToken(token: string = '') {
        if (token === null || token === '') { return { 'upn': '' }; }
        const parts = token.split('.');
        if (parts.length !== 3) {

            throw new Error('JWT must have 3 parts');
        }
        const decoded = this.urlBase64Decode(parts[1]);
        if (!decoded) {
            throw new Error('Cannot decode the token');
        }
        return JSON.parse(decoded);
    }

    private static urlBase64Decode(str: string) {
        let output = str.replace(/-/g, '+').replace(/_/g, '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                // tslint:disable-next-line:no-string-throw
                throw 'Illegal base64url string!';
        }
        return decodeURIComponent((<any>window).escape(window.atob(output)));
    }




}