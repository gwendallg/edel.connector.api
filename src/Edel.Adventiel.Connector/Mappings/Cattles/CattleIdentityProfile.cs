﻿using AutoMapper;
using Edel.Adventiel.Connector.Entities.Cattles;
using WsMdBInterfaces;

namespace Edel.Adventiel.Connector.Mappings.Cattles
{
    /// <inheritdoc />
    public class CattleIdentityProfile : Profile
    {
        public CattleIdentityProfile()
        {
            CreateMap<Identite, CattleIdentity>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateNaissance))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom.Trim()))
                .ForMember(dest => dest.BreedCode, opt => opt.MapFrom(src => src.CodeRaceBovin))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sexe));
        }
    }
}