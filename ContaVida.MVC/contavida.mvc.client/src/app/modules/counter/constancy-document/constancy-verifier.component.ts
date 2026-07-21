import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EventService } from '../../../services/Events/event.service';
import { IEventConstancyVerifierModel } from '../../../Models/EventCounter/IEventConstancyVerifierModel';


@Component({
  selector: 'app-doc-verifier',
  templateUrl: './constancy-verifier.component.html',
  standalone: false
})
export class ConstancyVerifierComponent {

    stamp : string = "";
    processing: boolean =  true;
    verifiCationResult: IEventConstancyVerifierModel = <IEventConstancyVerifierModel>{};
    private sub: any;
    constructor( private route: ActivatedRoute, private eventService: EventService){
        
    }

    ngOnInit(){
      this.sub = this.route.queryParams.subscribe(params => {
        this.stamp = params['stamp'];
        // alert("Stamp reaached " + this.stamp)
        this.veryfyCert();
      });
    }

    veryfyCert(){
      this.eventService.verifyDocumentStamp(this.stamp)
      .subscribe({next: (data: any)=>{
        this.verifiCationResult = data
        this.processing = false;
      }, error: (err)=>{
        alert("Ha ocurrido un error con la validación, intentalo mas tarde.")
        this.processing = false;

      }})
    }
    
}
