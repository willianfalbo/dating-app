namespace DatingApp.Core.Interfaces
{
    public interface IClassMapper
    {
        TDestination To<TDestination>(object source);
        TDestination To<TSource, TDestination>(TSource source, TDestination destination);
    }
}