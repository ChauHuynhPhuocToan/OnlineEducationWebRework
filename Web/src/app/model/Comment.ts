import { User } from "../model";
export interface Comment {
    commentId:number;
    commentContext:string;
    rating:number;
    datePost:string;
    type:CommentType;
    likeCounter:number;
    user:User;
    userId:number;
    courseId:number;
    lessonId:number;
}
export interface SubComment {
    subCommentId:number;
    subCommentContext:string;
    subDatePost:string;
    likeCounter:number;
    parentCommentId:number;
    user:User;
    userId:number;
}
export enum CommentType
{
    Comment,
    Rating
}
