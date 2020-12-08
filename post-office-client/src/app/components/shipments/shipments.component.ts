import { Component, OnInit } from '@angular/core';
import { formatDateTime } from 'src/app/helpers/DateTimeExtensions';
import { ShipmentsService } from 'src/app/services/shipments.service';
import { IShipment } from '../../domain/shipments/IShipment';

@Component({
  selector: 'app-shipments',
  templateUrl: './shipments.component.html',
  styleUrls: ['./shipments.component.css']
})
export class ShipmentsComponent implements OnInit {
    unfinalizedShipments: IShipment[];
    finalizedShipments: IShipment[];

    constructor(private shipmentService: ShipmentsService) { }

    ngOnInit() {
        this.shipmentService.getAll()
            .subscribe(
                data => {
                    data.map(s => s.flightDate = formatDateTime(s.flightDate));
                    this.finalizedShipments = data.filter(s => s.isFinalized === true);
                    this.unfinalizedShipments = data.filter(s => s.isFinalized === false);
                });
            }

}
