using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.Models;

namespace OnlineEducation.EntityFramework.Accounts
{
    public interface IAccountRepository
    {
        Task<AccountDto> Create(CreateUpdateAccountDto account);
        Task<AccountDto> Update(CreateUpdateAccountDto account);
        Task<AccountDto> Delete(Guid id);
        Task<ValidateResultDto> Validate(Guid id);
        List<Account> GetListAccounts(AccountRequestInput input);   
        Task<Account> GetById(Guid Id);
    }
}
