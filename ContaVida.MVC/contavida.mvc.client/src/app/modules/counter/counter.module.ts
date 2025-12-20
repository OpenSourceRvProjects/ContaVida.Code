import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { AddCounterComponent } from './add-counter/add-counter.component';
import { CounterListComponent } from './counter-list/counter-list.component';
import { FormsModule } from '@angular/forms';
// import { MyCounterComponent } from './my-counter/my-counter.component';
// import { EnvironmentModule } from '../environment/environment.module';
// import { SharedModule } from '../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalRelapsesComponent } from './relapses/modalRelapses';
import { ModalDeleteCounterComponent } from './delete-counter/modalDeleteCounter';
import { ModalEditComponent } from '../edit-counter/modalEditCounter';
import { CounterRoutingModule } from './counter-routing.module';

@NgModule({
  declarations: [
    // AddCounterComponent,
    CounterListComponent,
    // MyCounterComponent,
    ModalEditComponent,
    ModalRelapsesComponent,
    ModalDeleteCounterComponent,
  ],
  imports: [
    CounterRoutingModule,
    FormsModule,
    CommonModule,
    // EnvironmentModule,
    // SharedModule,
    NgbModule
  ]
})
export class CounterModule { }
