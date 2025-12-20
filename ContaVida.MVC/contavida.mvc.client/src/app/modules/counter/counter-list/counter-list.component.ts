import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountService } from '../../../services/Accounts/account.service';
import { LocalStorageService } from '../../../services/Storage/local-storage.service';
import { ICounterPrivacySetModel } from '../../../Models/EventCounter/ICounterPrivacySetModel';
import { IEventCounterItemModel } from '../../../Models/EventCounter/IEventCounterItemModel';
import { ModalEditComponent } from '../../edit-counter/modalEditCounter';
import { ModalDeleteCounterComponent } from '../delete-counter/modalDeleteCounter';
import { EventService } from '../../../services/Events/event.service';
import { ModalRelapsesComponent } from '../relapses/modalRelapses';

@Component({
  selector: 'app-counter-list',
  templateUrl: './counter-list.component.html',
  standalone: false,
})
export class CounterListComponent {
  constructor(private modalService: NgbModal, private eventService: EventService, 
    private localStorageService: LocalStorageService, public router: Router,
    private accountService: AccountService) {
  }

  counterList : IEventCounterItemModel[] = [];
  filteredCounterList : IEventCounterItemModel[] = [];
  counterSetting : ICounterPrivacySetModel = <ICounterPrivacySetModel>{}; 
  processing =  false;
  searchText: string = "";

  ngOnInit() {
    this.accountService.getMaintenancePage();

    this.localStorageService.desactivateCounterView();
    this.getCountersList();
  }

  openEditPopUp(counterEvent : IEventCounterItemModel){
    const modalRef = this.modalService.open(ModalEditComponent, {ariaLabelledBy: 'modal-basic-title', size: 'lg' });
    modalRef.componentInstance.counterEvent = counterEvent;
  }

  openRelapsesPopUp(counterEvent : IEventCounterItemModel){
    const modalRef = this.modalService.open(ModalRelapsesComponent, {ariaLabelledBy: 'modal-basic-title', size: 'lg' });
    modalRef.componentInstance.counterEvent = counterEvent;
  }

  openDeletePopUp(counterEvent : IEventCounterItemModel){
    const modalRef = this.modalService.open(ModalDeleteCounterComponent, {ariaLabelledBy: 'modal-basic-title', size: 'lg' });
    modalRef.componentInstance.counterEventToDelete = counterEvent;
  }

  checkboxChange(eventItem: IEventCounterItemModel){
    this.counterSetting.isPublicCounter = eventItem.isPublic;

    this.eventService.changeEventPrivacySetting(eventItem.id, this.counterSetting)
    .subscribe({next: ()=> {
      this.counterSetting = <ICounterPrivacySetModel>{}
    }, error : (err) =>{

    }})
  }

  searchKey(data: string){
    debugger
    this.searchText = data;

    if (this.searchText == '')
      this.filteredCounterList = this.counterList;
    else
      this.filteredCounterList = this.counterList.filter(f=> f.eventName.includes(this.searchText))
  }

  getCountersList(){
    this.eventService.getEvents()
    .subscribe({next: (data : any)=> {
      this.counterList =  data;
      this.filteredCounterList = data
    }, 
    error : (err)=> {

    }})
  }
}
