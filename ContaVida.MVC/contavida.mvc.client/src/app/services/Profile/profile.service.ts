import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LocalStorageService } from '../Storage/local-storage.service';
import { IImageListModel } from '../../Models/Profile/IImageListModel';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private http: HttpClient, private localStorage: LocalStorageService) { }

  private baseUrl = "/";
  addImagesToProfile(images: IImageListModel) {
    return this.http.post(this.baseUrl + "api/Profile/addImages", images)
  }

  getProfileImages() {
    return this.http.get(this.baseUrl + "api/Profile/getImages")
  }

}
