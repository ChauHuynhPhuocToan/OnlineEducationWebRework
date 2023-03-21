using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.EntityFramework.Accounts;
using OnlineEducation.Models;
using OnlineEducation.Services.Accounts;

namespace OnlineEducation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountAppService _accountAppService;

        public AccountsController(IAccountAppService accountAppService)
        {
            this._accountAppService = accountAppService;
        }

        // POST: api/Accounts/GetAccountLists
        [HttpPost]
        [Route("GetAccountLists")]
        public List<AccountDto> GetAccounts(AccountRequestInput input)
        {
            return _accountAppService.GetListAsync(input);
        }

        // POST: api/Accounts/CreateAccount
        [HttpPost]
        [Route("CreateAccount")]
        public async Task<AccountDto> CreateAccount(CreateUpdateAccountDto input)
        {
            return await _accountAppService.Create(input);
        }

        // HttpPut: api/Accounts/Update
        [HttpPut]
        [Route("Update")]
        public async Task<AccountDto> Update(CreateUpdateAccountDto input)
        {
            return await _accountAppService.Update(input);
        }

        // HttpPut: api/Accounts/Delete
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<AccountDto> Delete(Guid id)
        {
            return await _accountAppService.Delete(id);
        }

        // HttpPut: api/Accounts/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<AccountDto> GetAccountById(Guid id)
        {
            return await _accountAppService.GetAccountById(id);
        }
    }
}
