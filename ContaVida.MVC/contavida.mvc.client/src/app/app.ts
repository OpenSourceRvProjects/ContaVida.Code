import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { AccountService } from './services/Accounts/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App  {
  public forecasts: any[] = [];
  currentEnvironment: string = "" ;
  public showUnderConstructionPage: boolean = false;

  constructor(private http: HttpClient, public accountService: AccountService) {}

  ngOnInit() {
    var currentDate = new Date();
    debugger;
    if (currentDate <  new Date(2026, 0, 14))
    {
      this.accountService.getSystemStackData()
      .subscribe({next: (data: any)=>{
        debugger;
        this.currentEnvironment = data.environment;
        if (this.currentEnvironment.toLowerCase() == "prod"){
          this.showUnderConstructionPage = true;
        }
      }, error: ()=>{}});
      
    }
  }



  protected readonly title = signal('contavida.mvc.client');
}
