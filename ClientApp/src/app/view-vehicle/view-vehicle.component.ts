import { PhotoService } from './../services/photo.service';
import { Vehicle } from './../models/vehicle';
import { SaveVehicle } from './../models/saveVehicle';
import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../services/vehicle.service';
import { ToastrService } from 'ngx-toastr';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  vehicleId: number;
  vehicle: Vehicle;
  @ViewChild('fileInput') fileInput: ElementRef;
  photos: any[];
  currentPage = 'details';
  detailsTab = true;
  photosTab = false;
  progress: number;

  constructor (
    private route: ActivatedRoute,
    private router: Router,
    private zone: NgZone,
    private toastr: ToastrService,
    private photoService: PhotoService,
    private vehicleService: VehicleService) {
      route.params.subscribe(p => {
        this.vehicleId = +p['id'];
        if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
          router.navigate(['/vehicles']);
          return;
        }
      });
     }

  ngOnInit() {
    this.vehicleService.getVehicle(this.vehicleId)
      .then(
        v => this.vehicle = v as Vehicle,
        err => {
          if (err.status === 404) {
            this.router.navigate(['/vehicles']);
            return;
          }
        });
    this.photoService.getPhotos(this.vehicleId).subscribe(photos =>
      this.photos = photos as any[]
      );
  }
  changePage(page) {
    this.currentPage = page;
    if (page === 'details') {
      this.detailsTab = true;
      this.photosTab = false;
    }
    if (page === 'photos') {
      this.detailsTab = false;
      this.photosTab = true;
    }
  }
  delete() {
    if (confirm('Are you sure?')) {
      this.vehicleService.delete(this.vehicle.id)
        .subscribe(x => {
          this.router.navigate(['/vehicles']);
        });
    }
  }
  edit() {
    this.router.navigate(['../../form/' + this.vehicleId], {relativeTo: this.route});
  }
  onPhotoUpload() {
    let nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    const file = nativeElement.files[0];
    nativeElement = '' as any;
    this.photoService.uploadPhoto(this.vehicleId, file)
    .subscribe((event: HttpEvent<any>) => {
        switch (event.type) {
          case HttpEventType.Sent:
             console.log('Request sent');
            break;
          case HttpEventType.UploadProgress:
            const percentage = Math.round(100 * event.loaded / event.total);
            this.progress = percentage;
            break;
          case HttpEventType.Response:
            this.photos.push(event.body);
            this.toastr.success('Photo uploaded succesfully', 'Success');
        }
    }, err => {
      console.log('Error', err);
      console.log(err.error);
      this.toastr.error(err.error, 'Error');
    });
  }
}
