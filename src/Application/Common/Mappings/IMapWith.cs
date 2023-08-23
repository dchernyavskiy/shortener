using AutoMapper;

namespace Shortener.Application.Common.Mappings;

public interface IMapWith<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
}
