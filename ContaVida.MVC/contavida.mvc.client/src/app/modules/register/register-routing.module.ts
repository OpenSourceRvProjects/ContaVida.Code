import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RegisterComponent } from "./register/register.component";
import { ForgotPasswordComponent } from "./password/forgot-password/forgot-password.component";
// import { UserPrivacyComponent } from "./user-privacy/user-privacy.component";
// import { ForgotPasswordComponent } from "./password/forgot-password/forgot-password.component";
// import { AuthRedirectComponent } from "../login/login/AuthRedirectComponent";

const routes: Routes = [

  { path: 'register', component: RegisterComponent, data: { name: "Registro" } },
//   { path: 'privacy', component: UserPrivacyComponent, data: { name: "Aviso de privacidad", showInNavBar: true } },
  { path: 'resetPassword', component: ForgotPasswordComponent, data: { name: "Recuperar contraseña" } },
//   { path: 'auth', component: AuthRedirectComponent }

]

@NgModule({
  //this is just the same as  this.router.config.push({ path: 'register', component: RegisterComponent})
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class RegisterRoutingModule {

  static getRoutes() {
    return routes;
  }
}
