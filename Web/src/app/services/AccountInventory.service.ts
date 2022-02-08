import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountInventory } from '../model';
import { urlAPI } from '../AppConsts';
import createId from '../AppConsts';
@Injectable({
  providedIn: 'root'
})
export class AccountInventoryService {
  constructor(private http: HttpClient) { }
  getAccountInventories = async () => {
    try {
      return await this.http.get(`${urlAPI}AccountInventories`).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  getAccountInventoriesByAccountId = async (id,option) =>{
    try {
      return await this.http.get(`${urlAPI}GetAccountInventoriesByAccountid?id=${id}&option=${option}`).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  getAccountInventoriesByInvoiceCode = async (option,invoiceCode) =>{
    try {
      return await this.http.get(`${urlAPI}GetAccountInventoriesByInvoiceCode?option=${option}&invoiceCode=${invoiceCode}`).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  postAccountInventory = async (accountincourse: AccountInventory) => {
    try {
      return await this.http.post(`${urlAPI}AccountInventories`, accountincourse).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  deleteaccountincourse = async (id) =>{
    try {
      return await this.http.delete(`${urlAPI}AccountInventories/${id}`).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  async createInvoice(accountInventory: AccountInventory){
    accountInventory.invoiceCode = (!accountInventory.invoiceCode) ? createId(6) : accountInventory.invoiceCode;
    await this.postAccountInventory(accountInventory);
    return accountInventory;
  }
  
  postCart = async (accountincourse: AccountInCourse) => {
    try {
      return await this.http.post(this.urlAPI+"AddCartItem", accountincourse).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  async getCart(accountId){
    try {
      return await this.http.get(this.urlAPI+"GetCartList?id="+accountId).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  deleteCart = async (accountId,courseId) =>{
    try {
      console.log(accountId+" "+courseId);
      return await this.http.delete(this.urlAPI + "DeleteCartItem?accountId="+accountId+"&courseId="+courseId).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
  emptyCart = async (accountId) =>{
    try {
      console.log(accountId);
      return await this.http.delete(this.urlAPI + "EmptyCartItem?accountId="+accountId).toPromise();
    }
    catch (e) {
      console.log(e);
    }
  }
}
