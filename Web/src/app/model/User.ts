export interface User {
    userId: number;
    firstName: string;
    lastName: string;   
    phoneNumber: string; 
    createdDate: Date;
    lastLogOnDate: Date;
    email: string;
    gender: Gender;
    avatarPath?: string;
    balance?:number;
    description?:string;
}
export interface InstructorProfile
{
    review:number;
    course:number;
    student:number;
    instructorName:string;
    description:string;
    avatarPath:string;
}
export enum Gender
{
    Male,
    Female
}