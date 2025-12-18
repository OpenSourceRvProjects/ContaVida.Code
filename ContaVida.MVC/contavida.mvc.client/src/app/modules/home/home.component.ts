import { Component } from '@angular/core';
// import { EventService } from 'src/app/Services/Events/event.service';
// import { LocalStorageService } from 'src/app/Services/Storage/local-storage.service';
// import { AccountService } from '../../Services/Accounts/account.service';

interface IResume {
  eventsCount : number;
  relapsesCount : number;
}
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  standalone: false,
})
export class HomeComponent {


  username?: string;

//   constructor(private localStorage: LocalStorageService, private eventService: EventService, private accountService: AccountService) {
//   }

//   countersResume: IResume = <IResume> { eventsCount : 0, relapsesCount: 0}

//   ngOnInit() {
//     this.accountService.getMaintenancePage();
//     this.localStorage.desactivateCounterView();
//     this.username = "";
//     this.username = this.localStorage.getUserData().userName;
//     this.eventService.getEventsResume()
//     .subscribe({next: (data: any)=>{
//       this.countersResume = data;
//     }, error: (err)=> {

//     }})
//   }
}
