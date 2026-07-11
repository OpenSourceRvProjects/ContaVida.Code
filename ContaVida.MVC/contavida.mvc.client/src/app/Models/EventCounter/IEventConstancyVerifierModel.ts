export interface IEventConstancyVerifierModel {
    isVerified: boolean;
    issuedTo: string;
    originalSetUpDate: Date;
    relapseEpisodes: number;
    userName : string;
}