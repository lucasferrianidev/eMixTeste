using AutoMapper;
using EMixApi.Data.Dtos;
using EMixApi.Data.Models;

namespace EMixApi.Profiles
{
    public class CepProfile : Profile
    {
        public CepProfile()
        {
            CreateMap<CreateCepDto, CEP>();
            CreateMap<UpdateCepDto, CEP>();
            CreateMap<CEP, ReadCepDto>();
        }
    }
}
