import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { IBagWithLettersCreateEdit } from 'src/app/domain/bags/IBagWithLettersCreateEdit';
import { IBagWithParcelsCreateEdit } from 'src/app/domain/bags/IBagWithParcelsCreateEdit';
import { AlertService } from 'src/app/services/alert.service';
import { BagWithLettersService } from 'src/app/services/bag-with-letters.service';
import { BagWithParcelsService } from 'src/app/services/bag-with-parcels.service';

@Component({
  selector: 'app-add-update-bag',
  templateUrl: './add-update-bag.component.html'
})
export class AddUpdateBagComponent implements OnInit {
    @Input() shipmentId!: string;
    @Input() id!: string;
    @Input() disabled!: boolean;
    @Input() containsParcels = true;

    submitted: boolean;
    bagWithParcelsForm: FormGroup
    bagWithLettersForm: FormGroup;
    bagForm: FormGroup;
    intMaxValue: number;
    isAddMode: boolean;

    constructor(
        private modalService: NgbModal,
        private formBuilder: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private bagWithParcelsService: BagWithParcelsService,
        private bagWithLettersService: BagWithLettersService
    ) {
        this.submitted = false;
        this.intMaxValue = Number.MAX_VALUE;
    }

    ngOnInit() {
        this.isAddMode = !this.id;

        this.bagWithParcelsForm = this.formBuilder.group({
            bagNumber: ['', [
                Validators.required,
                Validators.maxLength(15),
                Validators.pattern(/^[a-zA-Z0-9 ]+$/)
            ]]
        })

        this.bagWithLettersForm = this.formBuilder.group({
            bagNumber: ['', [
                Validators.required,
                Validators.maxLength(15),
                Validators.pattern(/^[a-zA-Z0-9 ]+$/)
            ]],
            countOfLetters: [0, [
                Validators.required,
                Validators.min(1),
                Validators.max(this.intMaxValue),
                Validators.pattern(/^\d+$/)
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
            if (this.containsParcels) {
                this.bagWithParcelsService.get(this.id)
                    .subscribe(x => this.bagWithParcelsForm.patchValue(x));
            } else {
                this.bagWithLettersService.get(this.id)
                    .subscribe(x => this.bagWithLettersForm.patchValue(x));
            }
        }

        this.bagForm = this.containsParcels ? this.bagWithParcelsForm : this.bagWithLettersForm;
    }

    get f() {
        return this.bagForm.controls;
    }

    open(content: TemplateRef<any>) {
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
    }

    changeActiveForm() {
        this.bagForm = this.containsParcels ? this.bagWithParcelsForm : this.bagWithLettersForm;
    }

    onSubmit() {
        this.submitted = true;

        if (this.bagForm.invalid) {
            return;
        }

        if (this.isAddMode) {
            this.createBag();
        } else {
            this.updateBag();
        }
        this.onClose();
    }

    createBag() {
        const bag = this.getBagFromForm();

        if (this.containsParcels) {
            this.bagWithParcelsService.create(bag)
                .subscribe({
                    next: () => {
                        this.alertService.success('Bag created', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        } else {
            this.bagWithLettersService.create(bag)
                .subscribe({
                    next: () => {
                        this.alertService.success('Bag created', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        }
    }

    updateBag() {
        const bag = this.getBagFromForm();

        if (this.containsParcels) {
            this.bagWithParcelsService.update(this.id, bag)
                .subscribe({
                    next: () => {
                        this.alertService.success('Bag updated', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        } else {
            this.bagWithLettersService.update(this.id, bag)
                .subscribe({
                    next: () => {
                        this.alertService.success('Bag updated', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        }
    }

    getBagFromForm(): IBagWithParcelsCreateEdit | IBagWithLettersCreateEdit {
        if (this.containsParcels) {
            return {
                bagNumber: this.bagForm.controls['bagNumber'].value,
                shipmentId: this.shipmentId,
                id: this.id
            }
        }
        return {
            bagNumber: this.bagForm.controls['bagNumber'].value,
            countOfLetters: Number(this.bagForm.controls['countOfLetters'].value),
            weight: Number(this.bagForm.controls['weight'].value),
            price: Number(this.bagForm.controls['price'].value),
            shipmentId: this.shipmentId,
            id: this.id
        }
    }

    onClose() {
        this.modalService.dismissAll({ ariaLabelledBy: 'modal-basic-title' });
        this.submitted = false;

        if (this.isAddMode) {
            this.bagForm.reset();
        }
    }
}
