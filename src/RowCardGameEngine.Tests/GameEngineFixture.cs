using Microsoft.Extensions.DependencyInjection;

namespace RowCardGameEngine.Tests
{
    public class GameEngineFixture
    {
        private readonly ServiceProvider _provider;

        public GameEngineFixture()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddServices();

            _provider = collection.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return _provider.GetService<T>();
        }
    }
}