import { Component, Input } from '@angular/core';

@Component({
  selector: 'reward-coin',
  templateUrl: './reward-coin.component.html',
  standalone: false,
  styleUrls: ['./goldCoin.css', './plateCoin.css']
})
export class RewardCoinComponent {

  @Input()
  timeUnit: string = "";

  @Input()
  timeQty: number = 0;
  @Input()
  eventName : string ="";

  
  
}
