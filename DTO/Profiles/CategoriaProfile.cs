namespace ApiHortifruti.DTO.Profiles;

using ApiHortifruti.Domain;
using AutoMapper;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<PostCategoriaDTO, Categoria>().ReverseMap();
        CreateMap<PutCategoriaDTO, Categoria>().ReverseMap();
        CreateMap<Categoria, GetCategoriaDTO>();
    }
}

    

