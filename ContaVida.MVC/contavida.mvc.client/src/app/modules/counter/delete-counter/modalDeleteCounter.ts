import { Component, OnInit, Input } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IEventCounterItemModel } from '../../../Models/EventCounter/IEventCounterItemModel';
import { EventService } from '../../../services/Events/event.service';
import { RelapsesDataModel } from '../../../Models/Relapses/IRelapseDetailModel';
@Component({
  selector: 'relapses-modal',
  templateUrl: './modalDeleteCounter.html',
  styleUrls: ['./modalDeleteCounter.css'],
  standalone: false,
})
export class ModalDeleteCounterComponent implements OnInit {
 

  @Input() counterEventToDelete: IEventCounterItemModel = <IEventCounterItemModel>{};

  processing: boolean = false;
  relapsesData: RelapsesDataModel = <RelapsesDataModel>{};
  uiTokenDelete: string = "";
  userTokenUi: string = "";

  constructor(public activeModal: NgbActiveModal, private eventService: EventService) { }
    ngOnInit(): void {
        debugger;
    this.uiTokenDelete = this.makeid(5);

  }

  deleteEventCounter(){
    this.processing = true;
    this.eventService.deleteEventCounter(this.counterEventToDelete.id)
    .subscribe({next: ()=> {
        window.location.href = "/counter/list"
    }, error: (err)=>{
        debugger;
        if (err.status == 404)
            alert("Evento no encontrado");
        this.processing = false;

    }})
  }

    makeid(length : number) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNPQRSTUVWXYZ123456789';
    const charactersLength = characters.length;

    for (var counter = 0; counter < 5; counter++){
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
    }


}