import { Model } from './../models/model';
import * as _ from 'underscore';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

import { Vehicle } from './../models/vehicle';
import { SaveVehicle } from './../models/saveVehicle';
import { VehicleService } from '../services/vehicle.service';
import { Make } from '../models/make';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any[];
  models: any[];
  features: any[];
  v: any;
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    features: [],
    contact: {
      name: '',
      phone: '',
      email: ''
    },
    isRegistered: false
  };
  vehicles: any[];
  model: any;
  vehicleId: number;
  editMode = false;

  constructor(private route: ActivatedRoute, private router: Router,
    private vehicleService: VehicleService, private toastr: ToastrService) {
      route.params.subscribe(p => {
        this.vehicleId = +p['id'];
      });
    }

  ngOnInit() {
    const sources = [
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ];
    const getvehicle = this.vehicleService.getVehicle(this.vehicleId);
    if (this.vehicleId) {
      this.editMode = true;
      sources.push(getvehicle.toPromise());
    }

    Promise.all(sources).then(data => {
      this.makes = data[0] as any[];
      this.features = data[1] as any[];
      if (this.vehicleId) {
        this.setVehicle(data[2] as Vehicle);
        this.populateModels();
      }
    }, err => {
      if (err.status === 404) {
        this.router.navigate(['']);
      }
    });
  }
  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.contact = v.contact;
    this.vehicle.isRegistered = v.isRegistered;
    v.feature.forEach(obj => {
      this.vehicle.features.push(obj.featureId);
    });
    // this.vehicle.features = _.pluck(v.feature, 'featureId');
  }
  onChange() {
      this.populateModels();
      delete this.vehicle.modelId;
    }
  private populateModels() {
    const selectedMake = this.makes.find(make => make.id === Number(this.vehicle.makeId));
      this.models = selectedMake ? selectedMake.models : [];
  }
  onFeatureToggle(FeatureId: number, $event) {
      if ($event.target.checked) {
        this.vehicle.features.push(FeatureId);
        // console.log(this.vehicle.features);
      } else {
        const index = this.vehicle.features.indexOf(FeatureId);
        this.vehicle.features.splice(index, 1);
      }
    }
    submit() {
      if (this.editMode) {
        this.vehicleService.update(this.vehicle).subscribe(data => {
          this.toastr.success('Vehicle was updated Successfully', 'Success');
        });
      } else {
        this.vehicleService.createVehicle(this.vehicle).subscribe(vehicle => {
          this.toastr.success('Vehicle was saved Successfully', 'Saved');
        });
      }
      this.vehicle.id = 0;
      this.vehicle.contact = {name: '', email: '', phone: ''};
      this.vehicle.features = [];
      this.vehicle.isRegistered = false;
      this.vehicle.makeId = 0;
      this.vehicle.modelId = 0;
    }
    deleteVehicle() {
      if (confirm('Are you sure?')) {
      this.vehicleService.delete(this.vehicleId).subscribe(vehicle => {
        this.router.navigate(['']);
        });
      }
    }
  }
