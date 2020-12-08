import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { NgbModal, NgbTimepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { ShipmentsService } from 'src/app/services/shipments.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IDate } from 'src/app/domain/common/IDate';
import { IShipmentCreateEdit } from 'src/app/domain/shipments/IShipmentCreateEdit';
import { getDateFromString, getStringFromDateTime, getTimeFromString } from 'src/app/helpers/DateTimeExtensions';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-add-update-shipment',
  templateUrl: './add-update-shipment.component.html',
  styleUrls: ['./add-update-shipment.component.css'],
  providers: [NgbTimepickerConfig]
})
export class AddUpdateShipmentComponent implements OnInit {
    @Input() id!: string;

    isAddMode: boolean;
    shipmentForm: FormGroup;
    submitted: boolean;
    minDate: IDate;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private shipmentService: ShipmentsService,
        private modalService: NgbModal,
        private formBuilder: FormBuilder,
        private config: NgbTimepickerConfig
    ) {
        this.submitted = false;

        const current = new Date();
        this.minDate = {
            year: current.getFullYear(),
            month: current.getMonth() + 1,
            day: current.getDate() + 1
        };

        this.config.spinners = false;
    }

    ngOnInit() {
        this.isAddMode = !this.id;

        this.shipmentForm = this.formBuilder.group({
            shipmentNumber: ['', [
                Validators.required,
                Validators.pattern(/^[a-zA-Z\d]{3}-[a-zA-Z\d]{6}$/)
            ]],
            airport: ["TLL", Validators.required],
            flightNumber: ['', [
                Validators.required,
                Validators.pattern(/^[a-zA-Z]{2}\d{4}$/)
            ]],
            flightDate: [this.minDate],
            flightTime: [{ hour: 0, minute: 0 }]
        });

        if (!this.isAddMode) {
            this.shipmentService.get(this.id)
                .subscribe(shipment => {
                    const formValues = {
                        shipmentNumber: shipment.shipmentNumber,
                        airport: shipment.airport,
                        flightNumber: shipment.flightNumber,
                        flightDate: getDateFromString(shipment.flightDate),
                        flightTime: getTimeFromString(shipment.flightDate)
                    }
                    this.shipmentForm.patchValue(formValues);
                });
        }
    }

    open(content: TemplateRef<any>) {
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
    }

    get f() { return this.shipmentForm.controls; }

    onSubmit() {
        this.submitted = true;

        if (this.shipmentForm.invalid) {
            return;
        }

        if (this.isAddMode) {
            this.createShipment();
        } else {
            this.updateShipment();
        }
        this.onClose();
    }

    createShipment() {
        const shipment = this.getShipmentFromForm();
        this.shipmentService.create(shipment)
            .subscribe({
                next: () => {
                    this.alertService.success('Shipment created', { keepAfterRouteChange: true });
                    this.router.navigate([this.route.url], { relativeTo: this.route });
                }
            });
    }

    updateShipment() {
        const shipment = this.getShipmentFromForm();
        this.shipmentService.update(this.id, shipment)
            .subscribe({
                next: () => {
                    this.alertService.success('Shipment updated', { keepAfterRouteChange: true });
                    this.router.navigate([this.route.url], { relativeTo: this.route });
                }
            });
    }

    getShipmentFromForm(): IShipmentCreateEdit {
        return {
            id: this.id,
            shipmentNumber: this.shipmentForm.controls['shipmentNumber'].value,
            airport: this.shipmentForm.controls['airport'].value,
            flightNumber: this.shipmentForm.controls['flightNumber'].value,
            flightDate: getStringFromDateTime(
                this.shipmentForm.controls['flightDate'].value,
                this.shipmentForm.controls['flightTime'].value
            ),
            isFinalized: false
        }
    }

    onClose() {
        this.modalService.dismissAll({ ariaLabelledBy: 'modal-basic-title' });
        this.submitted = false;

        if (this.isAddMode) {
            this.shipmentForm.reset();
        }
    }
}
