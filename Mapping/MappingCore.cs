using AutoMapper;
using CinemaHD.Models.Domains;
using CinemaHD.Models.DTOs;
using System.Data;

namespace CinemaHD.Mapping
{
    public class MappingCore : Profile
    {
        public MappingCore()
        {

            // Cinemas
            CreateMap<Cinemas, CinemaDTO>()
                .ForMember(dest => dest.LocationName, opt =>
                    opt.MapFrom(src => src.Location.NameLocation));
            CreateMap<CinemaDTO, Cinemas>()
                .ForMember(dest => dest.Location, opt => opt.Ignore()); 

            // Users
            CreateMap<Users, UserDTO>()
                .ForMember(dest => dest.RoleName, opt =>
                    opt.MapFrom(src => src.Role.NameRole));
            CreateMap<UserDTO, Users>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Seats
            CreateMap<Seats, SeatDTO>()
                .ForMember(dest => dest.SeatPosition, opt =>
                    opt.MapFrom(src => $"{src.AtRow}{src.AtColumn}"));
            CreateMap<SeatDTO, Seats>()
                .ForMember(dest => dest.AtRow, opt => opt.Ignore()) 
                .ForMember(dest => dest.AtColumn, opt => opt.Ignore());

            // MovieShowtimes
            CreateMap<MovieShowtimes, MovieShowtimeDTO>()
                .ForMember(dest => dest.MovieTitle, opt =>
                    opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.CinemaName, opt =>
                    opt.MapFrom(src => src.Cinema.NameCinema));
            CreateMap<MovieShowtimeDTO, MovieShowtimes>()
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Cinema, opt => opt.Ignore());

            // Shifts
            CreateMap<Shifts, ShiftDTO>()
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User.NameUse))
                .ForMember(dest => dest.LocationName, opt =>
                    opt.MapFrom(src => src.Location.NameLocation));
            CreateMap<ShiftDTO, Shifts>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            // Reservations
            CreateMap<Reservations, ReservationDTO>()
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User.NameUse))
                .ForMember(dest => dest.SeatPosition, opt =>
                    opt.MapFrom(src => $"{src.Seat.AtRow}{src.Seat.AtColumn}"))
                .ForMember(dest => dest.CinemaName, opt =>
                    opt.MapFrom(src => src.Seat.Cinema.NameCinema));
            CreateMap<ReservationDTO, Reservations>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Seat, opt => opt.Ignore());
        }
    }
}