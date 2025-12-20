export interface RelapseDetailModel {
  eventName: string;
  id: string;
  eventCounterID: string;
  dateString: string;
  daysSinceLastRelapse: number;
  timeSinceLastRelapseString: string;
}

export interface RelapsesDataModel {
  items: RelapseDetailModel[];
  timeUnit: string;
  timeQuantitySinceLastIssue: number;
  relapsesTimeAverage: number;
}
