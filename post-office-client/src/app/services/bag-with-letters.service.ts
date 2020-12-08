import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";
import { IBagWithLetters } from "../domain/bags/IBagWithLetters";
import { AlertService } from "./alert.service";
import { BaseService } from "./base.service";

@Injectable()
export class BagWithLettersService extends BaseService<IBagWithLetters> {

    constructor(alertService: AlertService, httpClient: HttpClient) {
        super(alertService, httpClient, 'BagWithLetterses');
    }

    getAllByShipmentId(shipmentId: string): Observable<IBagWithLetters[]> {
        const url = this.baseUrl + "/shipment/" + shipmentId;
        return this.httpClient.get<IBagWithLetters[]>(url)
            .pipe(
                catchError(error => this.errorHandler(error))
            );
    }

}
