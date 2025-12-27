import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { DevToolsComponent } from "./dev-tools.component";

const routes: Routes = [

    { path: 'developers', component: DevToolsComponent, data: {name : "Programadores", showInNavBar : true}},

]

@NgModule({
    //this is just the same as  this.router.config.push({ path: 'register', component: RegisterComponent})
    imports: [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})

export class DevToolsRoutingModule{ 

    static getRoutes() {
        return routes;
      }
}