using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Dependency;
using Hk.Core.Util.Model;

namespace Hk.Core.IRepositorys
{
    public interface IRapidDevelopmentRepository:IRepository<Base_DatabaseLink,string>,IScopeDependency
    {
        List<Base_DatabaseLink> GetAllDbLink();
        List<DbTableInfo> GetDbTableList(string linkId);
        void BuildCode(string linkId, string areaName, string tables, string buildType);
    }
}