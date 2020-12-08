import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { IBagWithParcels } from '../domain/bags/IBagWithParcels';
import { BaseService } from './base.service';
import { IBagWithParcelsView } from '../domain/bags/IBagWithParcelsView';
import { AlertService } from './alert.service';

@Injectable()
export class BagWithParcelsService extends BaseService<IBagWithParcels> {

    constructor(alertService: AlertService, httpClient: HttpClient) {
        super(alertService, httpClient, 'BagWithParcelses');
    }

    getAllByShipmentId(shipmentId: string): Observable<IBagWithParcelsView[]> {
        const url = this.baseUrl + "/shipment/" + shipmentId;
        return this.httpClient.get<IBagWithParcelsView[]>(url)
            .pipe(
                catchError(error => this.errorHandler(error))
            );
    }
}
