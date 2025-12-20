import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
// import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LoginModule } from './modules/login/login.module';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { RegisterModule } from './modules/register/register.module';
import { HomeModule } from './modules/home/home.module';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CounterModule } from './modules/counter/counter.module';
import { NgModule } from '@angular/core';
import { AuthInterceptorService } from './services/AuthInterceptorService';

@NgModule({
  declarations: [
    App,
    NavMenuComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    LoginModule,
    RegisterModule,
    HomeModule,
    CounterModule
  ],
  providers: [
   {
   provide: HTTP_INTERCEPTORS,
   useClass: AuthInterceptorService,
   multi: true
 }],
  bootstrap: [App]
})
export class AppModule { }
