export interface Account {
    accountId:number;
    username:string;
    password:string;
    role:string;
    isActive:boolean;
    userId:number;
}
export enum AccountRole
{
    Admin, 
    Instructor,
    User
}
export class AccountDTO
{
    accountId:number;
    username:string;
    role:AccountRole;
    isActive:boolean;
    verification:boolean
}