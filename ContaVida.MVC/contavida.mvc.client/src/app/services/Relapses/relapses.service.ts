import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { LocalStorageService } from '../Storage/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class RelapsesService {


  private baseUrl = "/"
  constructor(private http: HttpClient, private localStorage : LocalStorageService) { }

  getRelapses(eventCounterID : string){
    return this.http.get("api/Relapses/getEventCounterRelapses?eventCounterId=" + eventCounterID)
  }

  getRelapseReasons() {
    return this.http.get("api/Relapses/getRelapseReasons")

  }

}
