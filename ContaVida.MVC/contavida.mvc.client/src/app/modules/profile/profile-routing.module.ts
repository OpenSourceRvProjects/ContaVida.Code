import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PersonalProfileComponent } from "./personal-profile/personal-profile.component";
import { PasswordChangeComponent } from "./password-change/password-change.component";
import { ImageCollectionComponent } from "./image-collection/image-collection.component";

const routes: Routes = [

    { path: 'profile', component: PersonalProfileComponent, data: {name : "Perfil y ajustes", showInNavBar : true}},
    { path: 'profile/changePassword', component: PasswordChangeComponent, data: {name : "Cambio de password", showInNavBar : false}},
    { path: 'profile/imageCollection', component: ImageCollectionComponent, data: {name : "Imahge", showInNavBar : false}},

]

@NgModule({
    //this is just the same as  this.router.config.push({ path: 'register', component: RegisterComponent})
    imports: [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})

export class ProfileRoutingModule{ 

    static getRoutes() {
        return routes;
      }
}