import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LocalStorageService } from '../Storage/local-storage.service';
import { ICounterRefreshTimerSetModel } from '../../Models/EventCounter/ICounterRefreshTimerSetModel';
import { INewEventCounterModel } from '../../Models/EventCounter/INewEventCounterModel';
import { ICounterPrivacySetModel } from '../../Models/EventCounter/ICounterPrivacySetModel';
import { TextValueItem, TextValueModel } from '../../Models/TextValueItem';
import { ICounterDataModel } from '../../Models/EventCounter/ICounterDataModel';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private http: HttpClient, private loclStorage: LocalStorageService) { }

  private baseUrl = "/"

  addEvent(newEvent : INewEventCounterModel){
    debugger;
    return this.http.post(this.baseUrl + "api/EventCounter", newEvent)
  }

  getEvents(){
    return this.http.get(this.baseUrl +"api/EventCounter")
  }

  getEventsResume(){
    return this.http.get(this.baseUrl +"api/EventCounter/getCountersResume")
  }

  getEventByID(eventID: string){

    if (this.loclStorage.getUserData()) {
      return this.http.get(this.baseUrl + "api/EventCounter/getById?counterID=" + eventID)
    }
    else{
      return this.http.get(this.baseUrl + "api/EventCounter/getById?counterID=" + eventID)

    }
  }

  changeEventPrivacySetting(eventID: string, setting: ICounterPrivacySetModel ){
    var body = setting;
    return this.http.put(this.baseUrl +"api/EventCounter/changeCounterPrivacy?id=" + eventID, body)
  }

  changeRefreshMode(eventID: string, setting: ICounterRefreshTimerSetModel) {
    var body = setting;
    return this.http.put(this.baseUrl + "api/EventCounter/setTimeRefreshCounterUI?id=" + eventID, body)
  }

  editEventCounter(eventID: string, isRelapse: boolean, eventCounter: ICounterDataModel, selectedRelapseReason: TextValueModel, relapseMessage: string) {
    var body = eventCounter;
    debugger;
    //TODO, fix selectedRelapseReason
    return this.http.put(this.baseUrl + "api/EventCounter/editCounterEvent?id=" + eventID + "&isRelapse=" + isRelapse
      + "&relapseMessage=" + relapseMessage + "&relapseReason=" + (selectedRelapseReason.value == undefined ? 1 : selectedRelapseReason.value), body)
  }

  deleteEventCounter(eventID: string ){
    return this.http.delete(this.baseUrl +"api/EventCounter/deleteCounterByID?counterID=" + eventID)
  }




  getMonths(){
    var monthsList = [];
    monthsList.push(<TextValueItem>{number: 1, text : "Enero"})
    monthsList.push(<TextValueItem>{number: 2, text : "Febrero"})
    monthsList.push(<TextValueItem>{number: 3, text : "Marzo"})
    monthsList.push(<TextValueItem>{number: 4, text : "Abril"})
    monthsList.push(<TextValueItem>{number: 5, text : "Mayo"})
    monthsList.push(<TextValueItem>{number: 6, text : "Junio"})
    monthsList.push(<TextValueItem>{number: 7, text : "Julio"})
    monthsList.push(<TextValueItem>{number: 8, text : "Agosto"})
    monthsList.push(<TextValueItem>{number: 9, text : "Septiembre"})
    monthsList.push(<TextValueItem>{number: 10, text : "Octubre"})
    monthsList.push(<TextValueItem>{number: 11, text : "Noviembre"})
    monthsList.push(<TextValueItem>{number: 12, text : "Diciembre"})
    return monthsList;
  }

  getHours(){
    var hourList = [];
    hourList.push(<TextValueItem>{number: 0, text: "12:00 AM"})
    hourList.push(<TextValueItem>{number: 1, text: "01:00 AM"})
    hourList.push(<TextValueItem>{number: 2, text: "02:00 AM"})
    hourList.push(<TextValueItem>{number: 3, text: "03:00 AM"})
    hourList.push(<TextValueItem>{number: 4, text: "04:00 AM"})
    hourList.push(<TextValueItem>{number: 5, text: "05:00 AM"})
    hourList.push(<TextValueItem>{number: 6, text: "06:00 AM"})
    hourList.push(<TextValueItem>{number: 7, text: "07:00 AM"})
    hourList.push(<TextValueItem>{number: 8, text: "08:00 AM"})
    hourList.push(<TextValueItem>{number: 9, text: "09:00 AM"})
    hourList.push(<TextValueItem>{number: 10, text: "10:00 AM"})
    hourList.push(<TextValueItem>{number: 11, text: "11:00 AM"})
    hourList.push(<TextValueItem>{number: 12, text: "12:00 PM"})
    hourList.push(<TextValueItem>{number: 13, text: "01:00 PM"})
    hourList.push(<TextValueItem>{number: 14, text: "02:00 PM"})
    hourList.push(<TextValueItem>{number: 15, text: "03:00 PM"})
    hourList.push(<TextValueItem>{number: 16, text: "04:00 PM"})
    hourList.push(<TextValueItem>{number: 17, text: "05:00 PM"})
    hourList.push(<TextValueItem>{number: 18, text: "06:00 PM"})
    hourList.push(<TextValueItem>{number: 19, text: "07:00 PM"})
    hourList.push(<TextValueItem>{number: 20, text: "08:00 PM"})
    hourList.push(<TextValueItem>{number: 21, text: "09:00 PM"})
    hourList.push(<TextValueItem>{number: 22, text: "10:00 PM"})
    hourList.push(<TextValueItem>{number: 23, text: "11:00 PM"})

    return hourList;
  }

}
