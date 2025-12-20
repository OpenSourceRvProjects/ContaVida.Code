import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { CommonGuard } from '../../nav-menu/Security/common.guard';
// import { UserGuard } from 'src/app/Security/user.guard';
// import { LoginComponent } from './Views/login/login.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [CommonGuard], data: {name : "Inicio", showInNavBar : true, canShowInAdmin : true}},
]

@NgModule({
    //this is just the same as  this.router.config.push({ path: 'register', component: RegisterComponent})
    imports: [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})
export class HomeRoutingModule { }
