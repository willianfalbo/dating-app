namespace DatingApp.Core.Interfaces.Mappers
{
    /// <summary>
    /// Class mapper wrapper.
    /// </summary>
    public interface IClassMapper
    {
        TDestination To<TDestination>(object source);
        TDestination FromTo<TSource, TDestination>(TSource source, TDestination destination);
    }
}