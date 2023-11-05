using AutoMapper;
using WebApiGames.DTO.Rol;
using WebApiGames.DTO.Usuario;
using WebApiGames.Entidades;

namespace WebApiGames.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Rol, RolViewDTO>();

            CreateMap<UsuarioCreateDTO, Usuario>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Ignorar la asignación de Roles en este paso

            CreateMap<Usuario, UsuarioViewDTO>()
            .ForMember(dest => dest.NombreTienda, opt => opt.MapFrom(src => src.Tienda.Nombre));
        }
    }
}
