import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
// import { UserGuard } from 'src/app/Security/user.guard';
// import { CommonGuard } from '../../Security/common.guard';
// import { LoginComponent } from './Views/login/login.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
]

@NgModule({
    //this is just the same as  this.router.config.push({ path: 'register', component: RegisterComponent})
    imports: [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})
export class HomeRoutingModule { }
