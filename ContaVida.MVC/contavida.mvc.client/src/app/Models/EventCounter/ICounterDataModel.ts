export interface ICounterDataModel {
    name: string;
    counterID: string;
    year: number;
    month: number;
    day: number;
    hour: number;
    minutes: number;
    isPublicCounter: boolean;
    minutesToRefresh: boolean;
}
