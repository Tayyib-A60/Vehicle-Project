import { KeyVeluePair } from './../models/keyValuePair';
import { VehicleService } from './../services/vehicle.service';
import { Vehicle } from './../models/vehicle';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[];
  makes: KeyVeluePair[];
  filter: any = {};

  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.populateVehicles();
    this.vehicleService.getMakes().then(makes => this.makes = makes as KeyVeluePair[]);
  }
  private populateVehicles() {
    this.vehicleService.getVehicles(this.filter).then(vehicles => {
      this.vehicles = vehicles as Vehicle[];
    });
  }
  onFilterChange() {
    this.populateVehicles();
    // this.vehicles = this.allVehicles;
    // const filteredVehicles = this.allVehicles;
    // if (this.filter.makeId) {
    //   this.vehicles = filteredVehicles.filter(v => v.make.id === Number(this.filter.makeId));
    // }

  }
  resetFilter() {
    this.filter = {};
    this.onFilterChange();
  }

}
