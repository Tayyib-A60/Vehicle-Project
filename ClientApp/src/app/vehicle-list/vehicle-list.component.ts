import { QueryResult } from './../models/queryResult';
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
  private readonly _pageSize = 10;
  queryResult: QueryResult = {
    totalItems: 0,
    vehicles: []
  };
  makes: KeyVeluePair[];
  query: any = {
    pageSize: this._pageSize
  };
  columns = [
    {title: 'Id'},
    {title: 'Make', key: 'make', isSortable: true},
    {title: 'Model', key: 'model', isSortable: true},
    {title: 'Contact Name', key: 'contactName', isSortable: true},
    {title: ''}
  ];
  anything: any;

  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.populateVehicles();
    this.vehicleService.getMakes().then(makes => this.makes = makes as KeyVeluePair[]);
  }
  private populateVehicles() {
    this.vehicleService.getVehicles(this.query).then(result => {
      this.queryResult = result as QueryResult;
    });
  }
  onFilterChange() {
    this.query.page = 1;
    this.populateVehicles();
    // this.vehicles = this.allVehicles;
    // const filteredVehicles = this.allVehicles;
    // if (this.filter.makeId) {
    //   this.vehicles = filteredVehicles.filter(v => v.make.id === Number(this.filter.makeId));
    // }
  }
  onPageChange(page: number) {
    this.query.page = page;
    this.populateVehicles();
  }
  resetFilter() {
    this.query = {
      page: 1,
      pageSize:  this._pageSize
    };
    this.populateVehicles();
  }
  sort (columnName: string) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }

}
