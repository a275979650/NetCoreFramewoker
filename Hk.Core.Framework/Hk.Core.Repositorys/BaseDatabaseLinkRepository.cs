using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;

namespace Hk.Core.Repositorys
{
    public class BaseDatabaseLinkRepository:BaseRepository<Base_DatabaseLink, string>,IBaseDatabaseLinkRepository
    {
        public BaseDatabaseLinkRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public List<Base_DatabaseLink> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = Get();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        public Base_DatabaseLink GetTheData(string id)
        {
            return GetSingle(id);
        }

        public void AddData(Base_DatabaseLink newData)
        {
            Add(newData);
        }

        public void UpdateData(Base_DatabaseLink theData)
        {
            Update(theData);
        }

        public void DeleteData(List<string> ids)
        {
            ids.ForEach(x => Delete(x));
        }
    }
}