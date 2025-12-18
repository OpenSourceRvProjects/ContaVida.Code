import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  saveUserData(user: any) {
    localStorage.setItem("contaVida.ObjectInfo", JSON.stringify(user));
  }

  swapToLoginImpersonate (user: any){
    let originalUser = localStorage.getItem("contaVida.ObjectInfo");
    if (originalUser !== null){
      localStorage.setItem("contaVida.ObjectInfo.admin", originalUser);
      localStorage.setItem("contaVida.ObjectInfo", JSON.stringify(user));
    }

  }

  avtiveCounterView(){
    localStorage.setItem("contaVida.IsCounterViwe", "1")
  }

  desactivateCounterView(){
    localStorage.removeItem("contaVida.IsCounterViwe");
  }

  removeUserData(){
    localStorage.removeItem("contaVida.ObjectInfo");
    localStorage.removeItem("contaVida.ObjectInfo.admin");
  }

  getUserData(){
    let data = localStorage.getItem("contaVida.ObjectInfo");
    if (data !== null)
      return JSON.parse(data);
    else 
      return false;
  }

  isCounterActive() {
    let data = localStorage.getItem("contaVida.IsCounterViwe");
    if (data !== null)
    return true;
      else 
      return false;
  }
}
