import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { IShipment } from 'src/app/domain/shipments/IShipment';
import { formatDateTime } from 'src/app/helpers/DateTimeExtensions';
import { AlertService } from 'src/app/services/alert.service';
import { ShipmentsService } from 'src/app/services/shipments.service';

@Component({
  selector: 'app-shipment-details',
  templateUrl: './shipment-details.component.html'
})
export class ShipmentDetailsComponent implements OnInit {
    shipment: IShipment;
    flightDateStr: string;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private shipmentService: ShipmentsService
    ) {
        this.shipment = {
            id: "",
            shipmentNumber: "",
            airport: "",
            flightDate: "",
            flightNumber: "",
            isFinalized: false
        }
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            const shipmentId = params['id'];

            this.shipmentService.get(shipmentId)
            .subscribe(data => {
                this.shipment = data;
                this.shipment.flightDate = formatDateTime(this.shipment.flightDate);
            });
        });

    }

    finalize(shipmentId: string) {
        if (confirm("Are you sure you want to finalize this shipment?")) {
            this.shipmentService.finalize(shipmentId)
            .subscribe({
                next: () => {
                    this.alertService.success('Shipment finalized', { keepAfterRouteChange: true });
                    this.router.navigate(['/shipments', this.shipment.id], { relativeTo: this.route });
                }
            });
        }
    }

    delete(shipmentId: string) {
        if (confirm("Are you sure you want to delete this shipment?")) {
            this.shipmentService.delete(shipmentId)
            .subscribe({
                next: () => {
                    this.alertService.success('Shipment deleted', { keepAfterRouteChange: true });
                    this.router.navigate(['/'], { relativeTo: this.route });
                }
            });
        }
    }
}
