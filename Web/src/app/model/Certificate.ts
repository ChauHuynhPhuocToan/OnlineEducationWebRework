import { Course } from "../model";
export interface Certificate{
    certificateId:number;
    courseId:string;
    accountId:string;
    getDate:string;
}
export interface CertificateFullInfo{
    course:Course;
    userName:string;
    avatarPath:string;
    getDate:string;
}