import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ShipmentDetailsComponent } from './components/shipment-details/shipment-details.component';
import { ShipmentsComponent } from './components/shipments/shipments.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'shipments',
        pathMatch: 'full'
    },
    {
        path: 'shipments',
        component: ShipmentsComponent
    },
    {
        path: 'shipments/:id',
        component: ShipmentDetailsComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
