import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
// import { SharedModule } from '../shared/shared.module';
import { HomeComponent } from './home.component';
import { SharedModule } from '../shared/shared.module.';



@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    SharedModule
    // SharedModule
  ],
    exports: [],
    providers: []
})
export class HomeModule { }
