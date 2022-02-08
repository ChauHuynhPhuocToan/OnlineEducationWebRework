import { CourseHastag } from "../model";
export interface Questionpool {
    questionpoolId:number;
    questionpoolName:string;
    createdDate:Date;
    lastEdited:Date;
    hastag:CourseHastag;
    questionpoolThumbnailImage?:string;
    questionpoolThumbnailImageURL?:string;
    isActive:boolean;
    accountId:string;
}