namespace Passerino.Utils.Domain.Mapping
{
    public interface IMapper
    {
        T Map<T>(params object[] sources) where T : class;
        void Map(object destination, params object[] sources);
    }
}