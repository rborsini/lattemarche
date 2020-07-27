import $ from 'jquery';

export class BaseSearchModel {

    public ToUrlQueryString(): string {
        return jQuery.param(this);
    }

}