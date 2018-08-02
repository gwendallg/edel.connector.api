﻿
using AutoMapper;
using Edel.Connector.Models.Cattles;
using WsMdBInterfaces;

namespace Edel.Connector.Consumer.Api.Mappings.Cattles
{
    public class CattleBreederProfile  : Profile
    {
        public CattleBreederProfile()
        {
            CreateMap<Bovin, CattleBreeder>()
                .ForMember(dest => dest.CattleCountryCode, opt => opt.MapFrom(src => src.CodePays))
                .ForMember(dest => dest.CattleIdentifier, opt => opt.MapFrom(src => src.NumeroNationalAnimal.Trim()))
                .ForMember(dest => dest.Identity, opt => opt.MapFrom(src => src.Identite));
        }
    }
}