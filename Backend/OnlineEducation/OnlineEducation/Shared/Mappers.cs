using AutoMapper;
using OnlineEducation.Dtos.AccountDtos;
using OnlineEducation.Models;

namespace OnlineEducation.Shared
{
    //public class Mappers
    //{
    //    public static void CreateMap(IMapperConfigurationExpression config)
    //    {
    //       // Account
    //       config.CreateMap<CreateUpdateAccountDto, Account>();
    //       config.CreateMap<Account, AccountDto>();

    //    }
    //}

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Account
            CreateMap<CreateUpdateAccountDto, Account>();
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>();
        }
    }
}
