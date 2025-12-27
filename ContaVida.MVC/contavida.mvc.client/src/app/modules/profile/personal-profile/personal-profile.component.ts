import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PasswordChangeComponent } from '../password-change/password-change.component';
import { AccountService } from '../../../services/Accounts/account.service';

@Component({
  selector: 'app-personal-profile',
  templateUrl: './personal-profile.component.html',
  standalone: false
})
export class PersonalProfileComponent {


  constructor(private modalService: NgbModal, public router: Router, private accountService: AccountService) {
    
  }

  ngOnInit() {
    this.accountService.getMaintenancePage();

  }


  openChangePassword(){
    const modalRef = this.modalService.open(PasswordChangeComponent, {ariaLabelledBy: 'modal-basic-title', size: 'lg' });
  }
}
