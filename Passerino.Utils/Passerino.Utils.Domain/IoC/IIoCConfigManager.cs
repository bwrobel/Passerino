namespace Passerino.Utils.Domain.IoC
{
    public interface IIoCConfigManager
    {
        void Initialize();

        void AssertConfigurationIsValid();
    }
}