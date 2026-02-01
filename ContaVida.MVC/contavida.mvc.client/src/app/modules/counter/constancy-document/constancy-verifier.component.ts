import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-doc-verifier',
  templateUrl: './constancy-verifier.component.html',
  standalone: false
})
export class ConstancyVerifierComponent {

    stamp : string = "";
    private sub: any;
    constructor( private route: ActivatedRoute){
        
    }

    ngOnInit(){
    this.sub = this.route.queryParams.subscribe(params => {
      debugger;
      this.stamp = params['stamp'];
      alert("Stamp reaached " + this.stamp)
    });
    }
    
}
