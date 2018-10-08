using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hk.Core.Business.Cache;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;

namespace Hk.Core.Repositorys
{
    public class BaseSysRoleRepository:BaseRepository<Base_SysRole,string>, IBaseSysRoleRepository
    {
        static Base_SysRoleCache _cache { get; } = new Base_SysRoleCache();
        public BaseSysRoleRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<Base_SysRole> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = Get();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        public Base_SysRole GetTheData(string id)
        {
            return GetSingle(id);
        }

        public void AddData(Base_SysRole newData)
        {
            Add(newData);
        }

        public void UpdateData(Base_SysRole theData)
        {
            Update(theData);
            _cache.UpdateCache(theData.RoleId);
        }

        public void DeleteData(List<string> ids)
        {
            var roleIds = Get().Where(x => ids.Contains(x.RoleId)).Select(x => x.RoleId).ToList();
            //删除角色
            ids.ForEach(x => Delete(x));
            _cache.UpdateCache(roleIds);
        }


        public string GetRoleName(string userId)
        {
            return _cache.GetCache(userId)?.RoleName;
        }
    }
}