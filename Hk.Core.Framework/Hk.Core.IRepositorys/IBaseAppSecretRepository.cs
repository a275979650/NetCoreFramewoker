using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;
using Microsoft.AspNetCore.Http;

namespace Hk.Core.IRepositorys
{
    public interface IBaseAppSecretRepository:IRepository<Base_AppSecret,string>,IScopeDependency
    {
        bool IsSecurity(HttpContext context);
        string GetAppSecret(string appId);
    }
}