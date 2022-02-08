export interface Quiz {
    quizId:number;
    question:string;
    questionType:QuestionType;
    quizImage?:string;
    quizImageLink?:string;
    time?:number;
    topicId:string;
    questionpoolId:number;
}
export enum QuestionType
{
    MultipleChoice
}
