import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { IBagWithLetters } from 'src/app/domain/bags/IBagWithLetters';
import { AlertService } from 'src/app/services/alert.service';
import { BagWithLettersService } from 'src/app/services/bag-with-letters.service';

@Component({
  selector: 'app-bag-with-letters',
  templateUrl: './bag-with-letters.component.html',
  styleUrls: ['./bag-with-letters.component.css']
})
export class BagWithLettersComponent implements OnInit {
    @Input() disabled!: boolean;
    @Input() shipmentId!: string;

    bags: IBagWithLetters[];

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private bagWithLettersService: BagWithLettersService
    ) { }

    ngOnInit() {
        this.bagWithLettersService.getAllByShipmentId(this.shipmentId)
            .subscribe(data => {
                this.bags = data;
            });
    }

    delete(bagId: string) {
        if (confirm("Are you sure you want to delete this bag?")) {
            this.bagWithLettersService.delete(bagId)
                .subscribe({
                    next: () => {
                        this.alertService.success('Bag deleted', { keepAfterRouteChange: true });
                        this.router.navigate([this.route.url], { relativeTo: this.route });
                    }
                });
        }
    }

}
