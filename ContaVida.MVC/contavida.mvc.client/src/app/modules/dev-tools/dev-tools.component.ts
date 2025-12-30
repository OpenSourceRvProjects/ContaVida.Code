import { Component, VERSION } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountService } from '../../services/Accounts/account.service';

@Component({
  selector: 'app-dev-tools',
  templateUrl: './dev-tools.component.html',
  standalone: false
})
export class DevToolsComponent {
  angularVersion = VERSION.full;
  systemData: any = {};
constructor(private modalService: NgbModal, public router: Router, private accountService: AccountService) {
    
  }

  ngOnInit() {
    this.accountService.getMaintenancePage();
    this.accountService.getSystemStackData().
      subscribe({
        next: (data) => {
          debugger;
          this.systemData = data;
        }, error: (err) => { }
      });

  }
}
