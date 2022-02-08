import { Subtopic } from "../model";
export interface Lesson {
    lessonId:number;
    lessonTitle:string;
    videoURL:string;
    lessonNo:number;
    subTopic:Subtopic;
    subTopicId:number;
    videoQuizTime:string;
    quizId:string;
    lastUpdate:Date;
}
