using EasyMicroservices.Configuration.Interfaces;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.Cores.Database.Managers;
using EasyMicroservices.Database.Interfaces;
using EasyMicroservices.Mapper.Interfaces;

namespace EasyMicroservices.CommentsMicroservice.Interfaces
{
    public interface IDependencyManager
    {
        IDatabase GetDatabase();
        IMapperProvider GetMapper();
        IConfigProvider GetConfigProvider();
        IUniqueIdentityManager GetUniqueIdentityManager();
    }
}
