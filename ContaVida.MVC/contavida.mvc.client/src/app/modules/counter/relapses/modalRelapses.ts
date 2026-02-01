import { Component, OnInit, Input } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IEventCounterItemModel } from '../../../Models/EventCounter/IEventCounterItemModel';
import { RelapsesDataModel } from '../../../Models/Relapses/IRelapseDetailModel';
import { RelapsesService } from '../../../services/Relapses/relapses.service';
import { EventService } from '../../../services/Events/event.service';
@Component({
  selector: 'relapses-modal',
  templateUrl: './modalRelapses.html',
  standalone: false,

})
export class ModalRelapsesComponent implements OnInit {

  @Input() counterEvent: IEventCounterItemModel = <IEventCounterItemModel>{};
  constructor(public activeModal: NgbActiveModal, private relapseService: RelapsesService,
    private eventService: EventService
  ) { }

  processing: boolean = false;
  relapsesData: RelapsesDataModel = <RelapsesDataModel>{};

  ngOnInit() {
    this.relapseService.getRelapses(this.counterEvent.id)
    .subscribe({next: (data : any) =>{
        this.relapsesData = data;
    }, error: (err)=>{

    }})

  }
  
  donwoloadConstancyDocument(){
      this.eventService.getPDF(this.counterEvent.id).
    subscribe((base64: any) => {
      const src = `data:application/pdf;base64,${base64}`;
      const link = document.createElement("a")
      link.href = src

      link.download = "Constancia " + this.counterEvent.eventName;
      link.click()

      link.remove()
    }, (errorEvent: any) => {
    });

  }

}