import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from '../../../services/Storage/local-storage.service';
import { AccountService } from '../../../services/Accounts/account.service';

@Component({
  selector: 'app-maintenance',
  templateUrl: './maintenance.component.html',
  //styleUrls: ['./login.component.css']
})
export class MaintenanceComponent implements OnInit {

  constructor(private localStorageService: LocalStorageService, private accountService: AccountService) { }

  img: string = "maintenancePageImg.jpeg"
  ngOnInit() {
    debugger;
    this.accountService.blockMaintenancePageIfNotApplicable();
    this.localStorageService.avtiveCounterView();
  }

}
