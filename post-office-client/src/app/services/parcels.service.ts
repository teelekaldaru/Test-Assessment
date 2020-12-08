import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IParcel } from '../domain/parcels/IParcel';
import { BaseService } from './base.service';
import { AlertService } from './alert.service';

@Injectable()
export class ParcelsService extends BaseService<IParcel> {

    constructor(alertService: AlertService, httpClient: HttpClient) {
        super(alertService, httpClient, 'Parcels');
    }
}
