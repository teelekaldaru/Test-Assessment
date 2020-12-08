import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { IShipment } from './domain/shipments/IShipment';
import { ShipmentsService } from './services/shipments.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'Post Office App';
    shipments: IShipment[] = [];


    constructor(
        private shipmentService: ShipmentsService,
        private router: Router
    ) {}

    ngOnInit() {
        this.shipmentService.getAll().subscribe((data) => {
            this.shipments = data;
        })

        this.router.routeReuseStrategy.shouldReuseRoute = function(){
            return false;
        };

        this.router.events.subscribe((evt) => {
            if (evt instanceof NavigationEnd) {
                this.router.navigated = false;
                window.scrollTo(0, 0);
            }
        });
    }


}
