import { Component, OnInit, Input } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountService } from '../../../services/Accounts/account.service';
import { IChangePasswordModel } from '../../../Models/Profile/IChangePasswordModel';

@Component({
  selector: 'app-password-change',
  templateUrl: './password-change.component.html',
  standalone: false
})
export class PasswordChangeComponent {

  processing: boolean = false;
  passwordChangeModel : IChangePasswordModel = <IChangePasswordModel>{
    oldPassword : "",
    newPassword : "",
  };

  passwordConfirm : string  ="";

  constructor(public activeModal: NgbActiveModal, public acountService: AccountService) { }
  
  changePassword () {
    debugger;
    if (this.passwordConfirm !== this.passwordChangeModel.newPassword){
      alert("La confirmación de tu nuevo password no coincide, intentalo de nuevo");
      return;
    }

    this.acountService.changeMyPassword(this.passwordChangeModel)
    .subscribe({next: ()=>{
      window.location.href = "/";
    }, error: (err)=>{
      debugger;
      alert("Error :" + err.error.message);
    }});

  }

}