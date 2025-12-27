import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MaintenanceComponent } from "./maintenance/maintenance.component";


const routes: Routes = [
  //{ path: 'login', component: LoginComponent, data: { name: "Login", showInNavBar: false } }
  { path: 'maintenancePage', component: MaintenanceComponent, data: { name: "maintenancePage", showInNavBar: false } }



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class MaintenanceRoutingModule {

}
