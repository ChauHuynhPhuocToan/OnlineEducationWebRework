using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.EntityFramework.Accounts;
using OnlineEducation.Models;

namespace OnlineEducation.Services.Accounts
{
    public interface IAccountAppService
    {
        List<AccountDto> GetListAsync(AccountRequestInput input);
        Task<AccountDto> GetAccountById(Guid id);
        Task<AccountDto> Create(CreateUpdateAccountDto account);
        Task<AccountDto> Update(CreateUpdateAccountDto account);
        Task<AccountDto> Delete(Guid id);
        Task<ValidateResultDto> Validate(Guid id);
    }
}
