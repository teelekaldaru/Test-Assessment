import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IShipment } from '../domain/shipments/IShipment';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertService } from './alert.service';

@Injectable()
export class ShipmentsService extends BaseService<IShipment> {

    constructor(alertService: AlertService, httpClient: HttpClient) {
        super(alertService, httpClient, 'Shipments');
    }

    finalize(id: string): Observable<IShipment> {
        const url = this.baseUrl + "/finalize/" + id;
        return this.httpClient.put<IShipment>(
            url,
            { headers: this.headers }
        ).pipe(
            catchError(error => this.errorHandler(error))
        );
    }
}
