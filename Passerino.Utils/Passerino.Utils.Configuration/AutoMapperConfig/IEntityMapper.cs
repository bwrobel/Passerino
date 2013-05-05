namespace Passerino.Utils.Configuration.AutoMapperConfig
{
    public interface IEntityMapper
    {
        T Map<T>(params object[] sources) where T : class;
        void Map(object destination, params object[] sources);
    }
}