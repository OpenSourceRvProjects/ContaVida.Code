export interface IChangePasswordModel {
    oldPassword: string;
    newPassword: string;
    changePassword() : void; 
}