import { HttpClient, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class PhotoService {

    constructor(private httpClient: HttpClient) { }
  // upload(vehicleId: number, photo) {
  //   const formData = new FormData();
  //   formData.append('file', photo);
  //   return this.httpClient.post(`/api/vehicles/${vehicleId}/photos`, formData);
  // }
  getPhotos(vehicleId: number) {
    return this.httpClient.get(`/api/vehicles/${vehicleId}/photos`);
  }
  uploadPhoto(vehicleId, photo) {
    const formData = new FormData();
    formData.append('file', photo);
    const request = new HttpRequest('POST', `/api/vehicles/${vehicleId}/photos`, formData, { reportProgress: true });
    return this.httpClient.request(request);
}
}
