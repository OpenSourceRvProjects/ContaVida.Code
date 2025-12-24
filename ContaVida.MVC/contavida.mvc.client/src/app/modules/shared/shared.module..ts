import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
//import { CopyLinkComponent } from './copy-link/copy-link.component';
//import { RewardCoinComponent } from './reward-coin/reward-coin.component';
import { FormLogoComponent } from './form-logo/form-logo.component';
import { RewardCoinComponent } from './reward-coin/reward-coin.component';



@NgModule({
  declarations: [
    //CopyLinkComponent,
    RewardCoinComponent,
    FormLogoComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
  ],
  exports: [RewardCoinComponent,  FormLogoComponent]
})
export class SharedModule { }
