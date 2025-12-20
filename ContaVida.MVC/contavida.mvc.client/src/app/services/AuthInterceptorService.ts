import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalStorageService } from './Storage/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private localStorage: LocalStorageService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var token  = this.localStorage.getUserData().token;
    let request = req;

    if (token){
        request = req.clone({
            setHeaders: {
              authorization: `Bearer ${ token }`
            }
          });
    }

    return next.handle(request);
  }

}