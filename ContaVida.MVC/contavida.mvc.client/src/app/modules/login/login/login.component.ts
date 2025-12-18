import { Component, NgZone, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../../services/Accounts/account.service';
import { LocalStorageService } from '../../../services/Storage/local-storage.service';


// import { NavMenuComponent } from 'src/app/nav-menu/nav-menu.component';

declare const google: any;
import * as msal from '@azure/msal-browser';
import { ILoginModel } from '../../../Models/Account/ILoginModel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false,
})
export class LoginComponent implements OnInit {


  loginModel: ILoginModel = <ILoginModel>{ userName: "", password: "" };
  userName: string;
  password: string;
  errorMessage: string;
  processing: boolean = false;
  isEnableProviderLogin = false;
  private msalInstance!: msal.PublicClientApplication;
  constructor(private router: Router, private accountService: AccountService, private localStorage: LocalStorageService, private ngZone: NgZone) {
    this.userName = "";
    this.password = "";
    this.errorMessage = "";
  }

  async ngOnInit() {
    debugger;
    if (this.localStorage.getUserData())
      this.router.navigate(['/'])

    this.accountService.getMaintenancePage();
    await this.accountService.initGoogleAuth(this.loginWithGoogle.bind(this));
    await this.accountService.initMicrosoftAuth();
  }

  //#region externalLoginAuth
  loginWithGoogle(response: any) {
    this.ngZone.run(() => {
      this.errorMessage = "";
      this.processing = true;
      const credential = response.credential;
      this.accountService.googleLogin(credential).subscribe({
        next: (data) => {
          debugger;
          this.localStorage.saveUserData(data);
          this.processing = false;
          this.router.navigate(['/']);

        }, error: (err) => {
          debugger;
          this.processing = false;
          this.errorMessage = "Hubo un problema al conectarte con tu cuenta de google!, Probablemente aún no te registras con tu cuenta de google";
        }
      });
    });
  }

  loginWithMicrosoft() {
    this.processing = true;
    this.errorMessage = "";

    const msalInstance = this.accountService.getMsalInstance();

    msalInstance.loginPopup({
      scopes: ["openid", "email", "profile"],
    }).then((response: any) => {
      const idToken = response.idToken;

      this.accountService.microsofLogin(idToken).subscribe({
        next: (data: any) => {
          this.localStorage.saveUserData(data);
          this.processing = false;
          window.location.href = "/";
        },
        error: () => {
          this.processing = false;
          this.errorMessage = "Error con el inicio de sesión de Microsoft. Quizá ya estás registrado.";
        }
      });
    }).catch((error: any) => {
      this.processing = false;
      this.errorMessage = "Error al conectar con Microsoft: " + error;
      console.error(error);
    });
  }

  //#endregion

  enableProviderLogin() {
    this.isEnableProviderLogin = true;

    const button = document.createElement("button");
    button.innerText = "Regístrate con Outlook";
    button.classList.add("btn", "btn-outline-primary", "w-100");
    button.onclick = () => this.loginWithMicrosoft();

    setTimeout(() => {
      this.accountService.renderGoogleButton('google-signin-button');
    });
  }

  triggerGoogleLogin() {
    this.accountService.triggerGoogleLoginPrompt();
  }
  login() {

    if (this.loginModel.userName.trim() === '' || this.loginModel.password.trim() === '')
      this.errorMessage = "El usuario y contraseña son obligatorios";

    this.processing = true;
    this.accountService.login(this.loginModel)
      .subscribe({
        next: (data: any) => {
          debugger;
          if (data.token != null) {
            this.localStorage.saveUserData(data);
            this.processing = false;
            window.location.href = "/"
            // this.router.navigate(['/']);
          }
          else {
            this.errorMessage = "Contraseña no valida";
            this.processing = false;
          }
        }
        , error: (err) => {
          debugger;
          if (err.status == 429) {
            alert("Demasiados intentos, intentalo en unos momentos mas");
          }
          else
            alert("Error! Usuario no existe o fuera de servicio");

          this.processing = false;

        }
      })
  }
}
