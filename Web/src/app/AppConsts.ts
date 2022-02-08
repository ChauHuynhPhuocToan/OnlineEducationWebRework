export const urlAPI = 'https://localhost:5001/api/';
const createId = (length):string =>{
    var result           = [];
    var characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for ( var i = 0; i < length; i++ ) {
      result.push(characters.charAt(Math.floor(Math.random() *
      charactersLength)));
    }
   return result.join('');
}
export default createId;