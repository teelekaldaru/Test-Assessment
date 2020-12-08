import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { IBagWithParcelsView } from 'src/app/domain/bags/IBagWithParcelsView';
import { AlertService } from 'src/app/services/alert.service';
import { BagWithParcelsService } from 'src/app/services/bag-with-parcels.service';

@Component({
  selector: 'app-bags-with-parcels',
  templateUrl: './bags-with-parcels.component.html',
  styleUrls: ['./bags-with-parcels.component.css']
})
export class BagsWithParcelsComponent implements OnInit {
    @Input() disabled!: boolean;
    @Input() shipmentId!: string;

    bags: IBagWithParcelsView[];

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private bagWithParcelsService: BagWithParcelsService
    ) { }

    ngOnInit() {
        this.bagWithParcelsService.getAllByShipmentId(this.shipmentId)
            .subscribe(data => {
                this.bags = data;
            });
    }

    delete(bagId: string) {
        if (confirm("Are you sure you want to delete this bag?")) {
            this.bagWithParcelsService.delete(bagId)
                .subscribe({
                    next: () => {
                        this.alertService.success('Bag deleted', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        }
    }

}
