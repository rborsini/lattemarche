export class Position {
    public lat: number = 0;
    public lng: number = 0;

    constructor(lat: number, lng: number) {
        this.lat = lat;
        this.lng = lng;
    }

}

export class Marker {
    public lat: number = 0;
    public lng: number = 0;
    public title: string = "";

    constructor(lat: number, lng: number, title: string) {
        this.lat = lat;
        this.lng = lng;
        this.title = title;
    }

}