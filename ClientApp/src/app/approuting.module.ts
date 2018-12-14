import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { ViewVehicleComponent } from './view-vehicle/view-vehicle.component';

const appRoutes: Routes = [

  { path: '', redirectTo: '', pathMatch: 'full'},
  { path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'vehicle/form', component: VehicleFormComponent },
  { path: 'vehicle/form/:id', component: VehicleFormComponent },
  { path: 'vehicle/view/:id', component: ViewVehicleComponent },
  { path: 'vehicles', component: VehicleListComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(appRoutes)
  ],
  declarations: [],
  exports: [RouterModule]
})
export class ApproutingModule { }
