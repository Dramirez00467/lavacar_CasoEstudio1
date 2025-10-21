using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarBLL.Dtos;
using lavacarDAL.Entidades;

namespace lavacarBBL.Mapeos
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>();

            CreateMap<Vehiculo, VehiculoDto>();
            CreateMap<VehiculoDto, Vehiculo>();

            // Cita: usa enum
            CreateMap<Cita, CitaDto>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));

            CreateMap<CitaDto, Cita>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => Enum.Parse<EstadoCita>(src.Estado)));
        }
    }
}