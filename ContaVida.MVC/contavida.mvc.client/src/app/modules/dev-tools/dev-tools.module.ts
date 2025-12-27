import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { SharedModule } from "../shared/shared.module.";
import { DevToolsComponent } from "./dev-tools.component";
import { DevToolsRoutingModule } from "./dev-tools.routing";


@NgModule({
  declarations: [
    DevToolsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    SharedModule,
    DevToolsRoutingModule
  ]
})
export class DevToolsModule { }
