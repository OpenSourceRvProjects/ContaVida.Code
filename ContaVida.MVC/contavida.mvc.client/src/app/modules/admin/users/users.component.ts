import { Component, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { AccountService } from '../../../services/Accounts/account.service';
import { LocalStorageService } from '../../../services/Storage/local-storage.service';
import { IUsersModel } from '../../../Models/Account/IUsersModel';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
  standalone: false
})
export class UsersComponent {

  constructor(private accountService: AccountService, private localStorageService: LocalStorageService, private modalService: NgbModal, private sanitizer: DomSanitizer) {
  }

  users: IUsersModel[] = [];
  selectedUser: IUsersModel | null = null;
  processing = false;
  feedbackMessage: SafeHtml | string = "";

  @ViewChild('confirmModal') confirmModalRef!: TemplateRef<any>;

  ngOnInit() {
    this.accountService.getAllUsers()
      .subscribe({
        next: (data: any) => {
          this.users = data
        }, error: (err) => {
        }
      })
  }

  openConfirmModal(user: IUsersModel) {
    this.selectedUser = user;
    this.feedbackMessage = "";
    this.modalService.open(this.confirmModalRef, { centered: true });
  }

  confirmResetPassword(modal: any) {
    if (!this.selectedUser || this.processing) return;

    this.processing = true;
    this.accountService.sendRecoveryEmail(this.selectedUser.email)
      .subscribe({
        next: () => {
          this.feedbackMessage = this.sanitizer.bypassSecurityTrustHtml(
            `<p style="color:green">Correo enviado a ${this.selectedUser?.email}</p>`
          );
          this.processing = false;
        },
        error: (err) => {
          modal.close();
          this.feedbackMessage = this.sanitizer.bypassSecurityTrustHtml(
            `<p style="color:red">ERROR! No se pudo enviar el correo</p>`
          );
          this.processing = false;

        }
      });
  }

  impersonate(userID: string) {
    this.accountService.loginImpersonate(userID)
      .subscribe({
        next: (data: any) => {
          var response = data;
          this.localStorageService.swapToLoginImpersonate(response);
          window.location.href = "/";

        }
      })
  }
}
