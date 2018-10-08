using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;

namespace Hk.Core.IRepositorys
{
    public interface IBaseDatabaseLinkRepository:IRepository<Base_DatabaseLink,string>,IScopeDependency
    {
        List<Base_DatabaseLink> GetDataList(string condition, string keyword, Pagination pagination);
        Base_DatabaseLink GetTheData(string id);
        void AddData(Base_DatabaseLink newData);
        void UpdateData(Base_DatabaseLink theData);
        void DeleteData(List<string> ids);
    }
}