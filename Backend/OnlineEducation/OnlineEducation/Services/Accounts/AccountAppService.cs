using AutoMapper;
using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.EntityFramework.Accounts;
using OnlineEducation.Models;
using OnlineEducation.Shared;

namespace OnlineEducation.Services.Accounts
{
    public class AccountAppService : IAccountAppService
    {
        private IAccountRepository _accountRepository;
        private IMapper _iMapper;
        public AccountAppService(IAccountRepository accountRepository) {
            _accountRepository = accountRepository;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _iMapper = config.CreateMapper();
        }

        public async Task<AccountDto> Create(CreateUpdateAccountDto account)
        {
            return await _accountRepository.Create(account);
        }

        public Task<AccountDto> Update(CreateUpdateAccountDto account)
        {
            return _accountRepository.Update(account);
        }

        public Task<AccountDto> Delete(Guid id)
        {
            return _accountRepository.Delete(id);
        }

        public async Task<AccountDto> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetById(id);

            return _iMapper.Map<Account, AccountDto>(account);
        }

        public List<AccountDto> GetListAsync(AccountRequestInput input)
        {
            var account = _accountRepository.GetListAccounts(input);

            return _iMapper.Map<List<Account>, List<AccountDto>>(account);
        }

        public Task<ValidateResultDto> Validate(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
