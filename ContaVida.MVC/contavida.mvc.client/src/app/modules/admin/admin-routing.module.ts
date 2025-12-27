import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { UsersComponent } from "./users/users.component";
import { SysadminGuard } from "../../nav-menu/Security/sysadmin.guard";

const routes: Routes = [

    { path: 'users/list', component: UsersComponent, canActivate: [SysadminGuard], data: {name : "Usuarios", canShowInAdmin : true}},

]

@NgModule({
    //this is just the same as  this.router.config.push({ path: 'register', component: RegisterComponent})
    imports: [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})

export class AdminRoutingModule{ 

    static getRoutes() {
        return routes;
      }
}