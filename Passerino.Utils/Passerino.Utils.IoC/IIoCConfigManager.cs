namespace Passerino.Utils.IoC
{
    public interface IIoCConfigManager
    {
        void Initialize();

        void AssertConfigurationIsValid();
    }
}