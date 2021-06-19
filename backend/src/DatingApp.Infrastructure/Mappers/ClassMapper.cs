using System;
using AutoMapper;
using DatingApp.Core.Interfaces.Mappers;

namespace DatingApp.Infrastructure.Mappers
{
    /// <inheritdoc />
    public class ClassMapper : IClassMapper
    {
        private readonly IMapper _mapper;

        public ClassMapper(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public TDestination To<TDestination>(object source) =>
             _mapper.Map<TDestination>(source);

        public TDestination FromTo<TSource, TDestination>(TSource source, TDestination destination) =>
            _mapper.Map(source, destination);
    }
}