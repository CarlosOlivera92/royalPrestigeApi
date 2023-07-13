using AutoMapper;
using RoyalPrestige_API.DTO;
using RoyalPrestige_API.Models;

namespace RoyalPrestige_API.Mapper
{
    public class Mapping : Profile
    {
        public Mapping() {
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioDTO, VendedoresDTO>().ReverseMap();
            CreateMap<Demostracione, DemostracionesDTO>().ReverseMap();
            CreateMap<Contrato, ContratoDTO>().ReverseMap();
            CreateMap<EstadosDemostracion, EstadosDemostracionesDTO>().ReverseMap();
            CreateMap<Reprogramacione, ReprogramacionesDTO>().ReverseMap();
            CreateMap<Role, RolDTO>().ReverseMap();
        }
    }
}
