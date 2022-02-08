export interface QuizAttempt {
    quizAttemptID:number;
    accountId:string;
    examQuizCode:string;
    isCompleted:boolean;
    result:string;
    lastTaken:Date;
}