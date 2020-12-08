import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ShipmentsComponent } from './components/shipments/shipments.component';
import { HttpClientModule } from '@angular/common/http';
import { ShipmentsService } from './services/shipments.service';
import { AddUpdateShipmentComponent } from './components/add-update-shipment/add-update-shipment.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from "@angular/common";
import { ShipmentDetailsComponent } from './components/shipment-details/shipment-details.component';
import { AddUpdateBagComponent } from './components/add-update-bag/add-update-bag.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddUpdateParcelComponent } from './components/add-update-parcel/add-update-parcel.component';
import { BagWithLettersComponent } from './components/bags-with-letters/bag-with-letters.component';
import { BagWithParcelsService } from './services/bag-with-parcels.service';
import { BagWithLettersService } from './services/bag-with-letters.service';
import { ParcelsService } from './services/parcels.service';
import { BagsWithParcelsComponent } from './components/bags-with-parcels/bags-with-parcels.component';
import { ParcelsTableComponent } from './components/parcels-table/parcels-table.component';
import { AlertComponent } from './components/alert/alert.component';

@NgModule({
  declarations: [
    AppComponent,
    ShipmentsComponent,
    AddUpdateShipmentComponent,
    ShipmentDetailsComponent,
    AddUpdateBagComponent,
    AddUpdateParcelComponent,
    BagWithLettersComponent,
    BagsWithParcelsComponent,
    ParcelsTableComponent,
    AlertComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgbModule,
    CommonModule,
    FormsModule,
    BrowserAnimationsModule
  ],
  providers: [
      ShipmentsService,
      BagWithParcelsService,
      BagWithLettersService,
      ParcelsService
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
}
