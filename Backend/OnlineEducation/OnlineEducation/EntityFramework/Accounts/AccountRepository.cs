using AutoMapper;
using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.Models;
using OnlineEducation.Shared;

namespace OnlineEducation.EntityFramework.Accounts
{
    public class AccountRepository: IAccountRepository
    {
        private ApplicationDbContext _context;
        private IMapper _iMapper;
        public AccountRepository(ApplicationDbContext context) { 
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _iMapper = config.CreateMapper();
        }

        public async Task<AccountDto> Create(CreateUpdateAccountDto account)
        {
            var accountCreate = await Account.CreateAccount(account);
            var accountCreateDB = _iMapper.Map<CreateUpdateAccountDto, Account>(accountCreate);
            await _context.AddAsync(accountCreateDB);
            await _context.SaveChangesAsync();
            return _iMapper.Map<Account, AccountDto>(accountCreateDB);
        }

        public async Task<AccountDto> Delete(Guid id)
        {
            var accountFind = await _context.Accounts.FindAsync(id);

            if (accountFind != null)
            {
                accountFind.IsDelete = true;
                _context.Update(accountFind);
                await _context.SaveChangesAsync();
                return _iMapper.Map<Account, AccountDto>(accountFind);
            }

            return null;
        }

        public async Task<AccountDto> Update(CreateUpdateAccountDto account)
        {
            var accountFind = await _context.Accounts.FindAsync(account.Id);

            if (accountFind != null)
            {
                _context.Update(accountFind);
                await _context.SaveChangesAsync();
                return _iMapper.Map<Account, AccountDto>(accountFind);
            }

            return null;
        }

        public async Task<Account> GetById(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public List<Account> GetListAccounts(AccountRequestInput input)
        {
            return _context.Accounts.Where(x => !x.IsDelete)
                           .WhereIf(!string.IsNullOrWhiteSpace(input.FilterText), x => x.UserName.ToLower().Contains(input.FilterText.ToLower()) || x.Role.ToLower().Contains(input.FilterText.ToLower()) || x.Email.ToLower().Contains(input.FilterText.ToLower())
                           || x.FirstName.ToLower().Contains(input.FilterText.ToLower()) || x.LastName.ToLower().Contains(input.FilterText.ToLower()) || x.PhoneNumber.ToLower().Contains(input.FilterText.ToLower()))
                           .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.ToLower().Contains(input.UserName.ToLower()))
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Role), x => x.Role.ToLower().Contains(input.Role.ToLower()))
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Email), x => x.Email.ToLower().Contains(input.Email.ToLower()))
                           .WhereIf(!string.IsNullOrWhiteSpace(input.FirstName), x => x.FirstName.ToLower().Contains(input.FirstName.ToLower()))
                           .WhereIf(!string.IsNullOrWhiteSpace(input.LastName), x => x.LastName.ToLower().Contains(input.LastName.ToLower()))
                           .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumber), x => x.PhoneNumber.ToLower().Contains(input.PhoneNumber.ToLower()))
                           .WhereIf(input.LastLogOnDateMin.HasValue, x => x.LastLogOnDate.Date > input.LastLogOnDateMin.Value.Date)
                           .WhereIf(input.LastLogOnDateMax.HasValue, x => x.LastLogOnDate.Date < input.LastLogOnDateMax.Value.Date)
                           .WhereIf(input.BalanceMin.HasValue, x => x.Balance > input.BalanceMin.Value)
                           .WhereIf(input.BalanceMax.HasValue, x => x.Balance < input.BalanceMax.Value)
                           .WhereIf(input.Gender.HasValue, x => x.Gender < input.Gender.Value)
                           .Skip(input.SkipCount)
                           .Take(input.MaxResultCount)
                           .ToList();
        }

        public Task<ValidateResultDto> Validate(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
