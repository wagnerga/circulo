/* tslint:disable */
/* eslint-disable */
/**
 * API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { exists, mapValues } from '../runtime';
/**
 * 
 * @export
 * @interface GetPharmaciesToVisitResponse
 */
export interface GetPharmaciesToVisitResponse {
    /**
     * 
     * @type {Array<string>}
     * @memberof GetPharmaciesToVisitResponse
     */
    pharmacyNames?: Array<string>;
}

/**
 * Check if a given object implements the GetPharmaciesToVisitResponse interface.
 */
export function instanceOfGetPharmaciesToVisitResponse(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function GetPharmaciesToVisitResponseFromJSON(json: any): GetPharmaciesToVisitResponse {
    return GetPharmaciesToVisitResponseFromJSONTyped(json, false);
}

export function GetPharmaciesToVisitResponseFromJSONTyped(json: any, ignoreDiscriminator: boolean): GetPharmaciesToVisitResponse {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'pharmacyNames': !exists(json, 'pharmacyNames') ? undefined : json['pharmacyNames'],
    };
}

export function GetPharmaciesToVisitResponseToJSON(value?: GetPharmaciesToVisitResponse | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'pharmacyNames': value.pharmacyNames,
    };
}

