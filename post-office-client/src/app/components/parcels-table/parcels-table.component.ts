import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { IParcel } from 'src/app/domain/parcels/IParcel';
import { AlertService } from 'src/app/services/alert.service';
import { ParcelsService } from 'src/app/services/parcels.service';

@Component({
  selector: 'app-parcels-table',
  templateUrl: './parcels-table.component.html'
})
export class ParcelsTableComponent implements OnInit {
    @Input() parcels!: IParcel[];
    @Input() disabled!: boolean;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private parcelsService: ParcelsService
    ) { }

    ngOnInit() {
    }

    delete(parcelId: string) {
        if (confirm("Are you sure you want to delete this parcel?")) {
            this.parcelsService.delete(parcelId)
                .subscribe({
                    next: () => {
                        this.alertService.success('Parcel deleted', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        }
    }

}
