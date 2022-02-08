export interface AccountInventory {
    accountInventoryID:number;
    accountId:number;
    courseId:number;
    invoiceCode:string;
    createdDate:string;
    paymentMethod:string;
    isBought:boolean;
    isCart:boolean;
    getPayment:boolean;
}
export enum PaymentMethod
{
    Paypal,
    CreditCard
}