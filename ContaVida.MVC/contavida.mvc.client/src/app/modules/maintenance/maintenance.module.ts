import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { MaintenanceRoutingModule } from './maintenance-routing.module';
import { SharedModule } from '../shared/shared.module.';

@NgModule({

  declarations: [
    MaintenanceComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MaintenanceRoutingModule,
    SharedModule,

  ],
  exports: [],
  providers: []

})
export class MaintenanceModule { }
