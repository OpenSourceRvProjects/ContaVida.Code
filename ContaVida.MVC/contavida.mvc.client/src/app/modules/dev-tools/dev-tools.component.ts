import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountService } from '../../services/Accounts/account.service';

@Component({
  selector: 'app-dev-tools',
  templateUrl: './dev-tools.component.html',
  standalone: false
})
export class DevToolsComponent {


constructor(private modalService: NgbModal, public router: Router, private accountService: AccountService) {
    
  }

  ngOnInit() {
    this.accountService.getMaintenancePage();

  }


}
