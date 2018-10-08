using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;
using Microsoft.AspNetCore.Http;

namespace Hk.Core.IRepositorys
{
    public interface IBaseAppSecretRepository:IRepository<Base_AppSecret,string>,IScopeDependency
    {
        bool IsSecurity(HttpContext context);
        string GetAppSecret(string appId);
        List<Base_AppSecret> GetDataList(string condition, string keyword, Pagination pagination);
        Base_AppSecret GetTheData(string id);
        void AddData(Base_AppSecret newData);
        void UpdateData(Base_AppSecret theData);
        void DeleteData(List<string> ids);
        void SavePermission(string appId, List<string> permissions);
    }
}