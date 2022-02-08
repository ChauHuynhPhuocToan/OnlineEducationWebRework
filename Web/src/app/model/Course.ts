import { User } from "../model";
export interface Course {
    courseId:number;
    courseName:string;
    rating:number;
    numberOfRating:number;
    numberOfParticipants:number;
    price:number;
    startDate:Date;
    courseDuration:string;
    description:string;
    thumbnailImage:string;
    hastag:CourseHastag;
    level:CourseLevel;
    lastUpdate:Date;
    lessonCounter:number;
    isActive:boolean;
    numberOfView:number;
    accountId:number;
}
export interface CourseDesktopData {
    course:Course;
    user:User;
}
export interface CourseDataSet{
    course:Course;
    rating:number;
    check:boolean;
    nonFormatedDescription:string;
}
export interface ChartData{
    hasTag:String[];
    rate:number[];
}
export enum CourseHastag
{
  C,
  CSharp, 
  CPlus,
  Java, 
  Htmlcss,
  Python,
  IOSAndroid,
  AI,
  Javascript,
  MachineLearning,
  UXUI, 
  Framework,
  Orther
}
export enum CourseLevel
{
  Basic,
  Tutorial,
  Advance,
  DeepLearning,
  Guide
}