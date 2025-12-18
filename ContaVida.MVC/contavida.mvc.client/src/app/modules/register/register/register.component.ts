import { Component, OnInit, NgZone } from '@angular/core';
import * as msal from '@azure/msal-browser';
import { AccountService } from '../../../services/Accounts/account.service';
import { IRegisterModel } from '../../../Models/Account/IRegisterModel';

declare const google: any;


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: false,

})
export class RegisterComponent implements OnInit {

  registerModel: IRegisterModel = <IRegisterModel>{};
  passwordConfirmation?: string;
  errorMessage?: string;
  processing?: boolean;
  isFinishRegister?: boolean;
  isEnableProviderRegister = false;
  private msalInstance!: msal.PublicClientApplication;

  constructor(
    private accountService: AccountService,
    private ngZone: NgZone
  ) { }

  async ngOnInit() {
    this.accountService.getMaintenancePage();
    await this.accountService.initGoogleAuth(this.registerWithGoogle.bind(this));
    await this.accountService.initMicrosoftAuth();

    this.passwordConfirmation = "";
    this.errorMessage = "";
    this.processing = false;
    this.isFinishRegister = false;
  }

  //#region externalProviderRegister
  registerWithGoogle(response: any) {
    // Run inside Angular zone so UI updates properly
    this.ngZone.run(() => {
      this.errorMessage = "";
      this.processing = true;

      const credential = response.credential;
      this.accountService.googleRegister(credential).subscribe({
        next: (data) => {
          this.processing = false;
          this.isFinishRegister = true;
          //this.goToLoginPage();
        },
        error: (err) => {
          this.processing = false;
          this.errorMessage = "Hubo un problema al conectarte con tu cuenta de google!, quizá ya te encuentras registrado";
        }
      });
    });
  }

  registerWithMicrosoft() {
    this.processing = true;
    this.errorMessage = "";

    const msalInstance = this.accountService.getMsalInstance();

    msalInstance.loginPopup({
      scopes: ["openid", "email", "profile"],
    }).then((response: any) => {
      const idToken = response.idToken;

      this.accountService.microsoftRegister(idToken).subscribe({
        next: () => {
          this.processing = false;
          this.isFinishRegister = true;
          //this.goToLoginPage();
        },
        error: () => {
          this.processing = false;
          this.errorMessage = "Error con el inicio de sesión de Microsoft. Quizá ya estás registrado.";
        }
      });
    }).catch((error: any) => {
      this.processing = false;
      this.errorMessage = "Error al conectar con Microsoft";
      console.error(error);
    });
  }

  enableProviderRegister() {
    this.isEnableProviderRegister = true;

    const button = document.createElement("button");
    button.innerText = "Regístrate con Outlook";
    button.classList.add("btn", "btn-outline-primary", "w-100");
    button.onclick = () => this.registerWithMicrosoft();

    setTimeout(() => {
      this.accountService.renderGoogleButton('google-signin-button');
    });
  }

  //#endregion

  registerAccount() {
    this.errorMessage = "";

    if (this.registerModel.name == undefined || this.registerModel.name.trim() === "") {
      this.errorMessage = "Necesitamos tu nombre para crear tu cuenta";
      return;
    }

    if (this.registerModel.lastName1 == undefined || this.registerModel.lastName1.trim() === "") {
      this.errorMessage = "Necesitamos alguno de tus apellidos para crear tu cuenta";
      return;
    }

    if (this.registerModel.password == undefined || this.registerModel.password.trim() === "") {
      this.errorMessage = "Crea una contraseña para ti";
      return;
    }

    if (this.registerModel.password !== this.passwordConfirmation) {
      this.errorMessage = "Ups!, la contraseña que ingresaste no coincide con la confirmación";
      return;
    }

    if (this.registerModel.password.length < 3) {
      this.errorMessage = "Tu contraseña es super insegura! ingresa una con más caracteres";
      return;
    }

    if (!this.registerModel.userName || this.registerModel.userName.length < 3) {
      this.errorMessage = "Tu nombre de usuario es tu identidad, se creativo!";
      return;
    }

    if (this.checkWhitespace(this.registerModel.userName)) {
      this.errorMessage = "Tu nombre de usuario no puede contener espacios en blanco";
      return;
    }

    if (this.checkWhitespace(this.registerModel.password)) {
      this.errorMessage = "No es recomendable que tu contraseña tenga espacios en blanco";
      return;
    }

    if (!this.validateEmail(this.registerModel.email)) {
      this.errorMessage = "El correo no tiene formato válido";
      return;
    }

    // error 400 if I not send this value
    this.registerModel.lastName2 = ".";
    this.processing = true;

    this.accountService.registerAccount(this.registerModel).subscribe({
      next: (data) => {
        this.processing = false;
        this.isFinishRegister = true;
      },
      error: (err) => {
        this.processing = false;
        this.errorMessage = err.error;
      }
    });
  }

  validateEmail(email: string) {
    return String(email)
      .toLowerCase()
      .match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      );
  }

  checkWhitespace(str: string) {
    let whitespace = new Set([" ", "\t", "\n"]);
    for (let i = 0; i < str.length; i++) {
      if (whitespace.has(str[i])) {
        return true;
      }
    }
    return false;
  }

  goToLoginPage() {
    window.location.href = "/login";
  }

}
