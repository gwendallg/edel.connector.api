﻿using AutoMapper;
using Edel.Adventiel.Connector.Entities.Breeders;
using WsMdBInterfaces;

namespace Edel.Adventiel.Connector.Mappings.Breeders
{
    public class BreederProfile : Profile
    {
        public BreederProfile()
        {
            CreateMap<typeIdentifiantExploitation, Breeder>()
                .ForMember(dest => dest.BreederCountryCode, opt => opt.MapFrom(src => src.CodePaysExploitation))
                .ForMember(dest => dest.BreederIdentifier, opt => opt.MapFrom(src => src.NumeroExploitation.Trim()));
        }
    }
}