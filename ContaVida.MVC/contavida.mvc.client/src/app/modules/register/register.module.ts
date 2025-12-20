import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { UserPrivacyComponent } from './user-privacy/user-privacy.component';
import { FormsModule } from '@angular/forms';
import { RegisterRoutingModule } from './register-routing.module';
import { RegisterComponent } from './register/register.component';
import { ForgotPasswordComponent } from './password/forgot-password/forgot-password.component';
// import { EnvironmentModule } from '../environment/environment.module';
// import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    RegisterComponent,
    // UserPrivacyComponent,
    ForgotPasswordComponent
  ],
  imports: [
    RegisterRoutingModule,
    // EnvironmentModule,
    FormsModule,
    CommonModule,
    // SharedModule
  ]
})
export class RegisterModule { }
