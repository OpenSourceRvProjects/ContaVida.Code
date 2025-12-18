import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";


const routes: Routes = [
  { path: 'login', component: LoginComponent, data: {name : "Login", showInNavBar : false}}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })

export class LoginRoutingModule {

 }
