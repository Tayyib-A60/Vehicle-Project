import { SaveVehicle } from './../models/saveVehicle';
import { Vehicle } from './../models/vehicle';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class VehicleService {

  constructor(private httpClient: HttpClient) { }

  getMakes() {
    return this.httpClient.get('/api/makes').toPromise();
  }
  getFeatures() {
    return this.httpClient.get('/api/features').toPromise();
  }
  getVehicles(filter) {
    return this.httpClient.get('api/vehicles' + '?' + this.toQueryString(filter)).toPromise();
  }
  toQueryString(obj) {
    const parts = [];
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
      }
    }
    return parts.join('&');
  }
  createVehicle(vehicle) {
    return this.httpClient.post('/api/vehicles', vehicle);
  }
  getVehicle(id?) {
    return this.httpClient.get('/api/vehicles/' + id).toPromise();
  }
  update(vehicle: SaveVehicle) {
    return this.httpClient.put('/api/vehicles/' + vehicle.id, vehicle);
  }
  delete(id: number) {
    return this.httpClient.delete('/api/vehicles/' + id);
  }
}

