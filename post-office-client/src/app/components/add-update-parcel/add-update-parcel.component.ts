import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { IParcelCreateEdit } from 'src/app/domain/parcels/IParcelCreateEdit';
import { AlertService } from 'src/app/services/alert.service';
import { ParcelsService } from 'src/app/services/parcels.service';

@Component({
  selector: 'app-add-update-parcel',
  templateUrl: './add-update-parcel.component.html'
})
export class AddUpdateParcelComponent implements OnInit {
    @Input() id!: string;
    @Input() bagWithParcelsId!: string;

    isAddMode: boolean;
    parcelForm: FormGroup;
    submitted: boolean;
    intMaxValue: number;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private modalService: NgbModal,
        private formBuilder: FormBuilder,
        private parcelsService: ParcelsService
    ) {
        this.submitted = false;
        this.intMaxValue = Number.MAX_VALUE;
    }

    ngOnInit() {
        this.isAddMode = !this.id;

        this.parcelForm = this.formBuilder.group({
            parcelNumber: ['', [
                Validators.required,
                Validators.pattern(/^[a-zA-Z]{2}\d{6}[a-zA-Z]{2}$/)
            ]],
            recipientName: ['', [
                Validators.required,
                Validators.minLength(1),
                Validators.maxLength(100)
            ]],
            destinationCountry: ['', [
                Validators.required,
                Validators.minLength(2),
                Validators.maxLength(2)
            ]],
            weight: [0, [
                Validators.required,
                Validators.min(0.001),
                Validators.max(this.intMaxValue),
                Validators.pattern(/^\d*\.?\d{1,3}$/)
            ]],
            price: [0, [
                Validators.required,
                Validators.min(0.001),
                Validators.max(this.intMaxValue),
                Validators.pattern(/^\d*\.?\d{1,2}$/)
            ]]
        });

        if (!this.isAddMode) {
            this.parcelsService.get(this.id)
                .subscribe(x => this.parcelForm.patchValue(x));
        }
    }

    get f() {
        return this.parcelForm.controls;
    }

    open(content: TemplateRef<any>) {
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
    }

    onSubmit() {
        this.submitted = true;

        if (this.parcelForm.invalid) {
            return;
        }

        if (this.isAddMode) {
            this.createParcel();
        } else {
            this.updateParcel();
        }
        this.onClose();
    }

    createParcel() {
        const parcel = this.getParcelFromForm();
        this.parcelsService.create(parcel)
            .subscribe({
                next: () => {
                    this.alertService.success('Parcel created', { keepAfterRouteChange: true });
                    this.router.navigate([this.route.url], { relativeTo: this.route });
                }
            });
    }

    updateParcel() {
        const parcel = this.getParcelFromForm();
        this.parcelsService.update(this.id, parcel)
            .subscribe({
                next: () => {
                    this.alertService.success('Parcel updated', { keepAfterRouteChange: true });
                    this.router.navigate([this.route.url], { relativeTo: this.route });
                }
            });
    }

    getParcelFromForm(): IParcelCreateEdit {
        return {
            id: this.id,
            parcelNumber: this.parcelForm.controls['parcelNumber'].value,
            recipientName: this.parcelForm.controls['recipientName'].value,
            destinationCountry: this.parcelForm.controls['destinationCountry'].value,
            weight: Number(this.parcelForm.controls['weight'].value),
            price: Number(this.parcelForm.controls['price'].value),
            bagWithParcelsId: this.bagWithParcelsId
        }
    }

    onClose() {
        this.modalService.dismissAll({ ariaLabelledBy: 'modal-basic-title' });
        this.submitted = false;

        if (this.isAddMode) {
            this.parcelForm.reset();
        }
    }

}
