using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.OrganSet;
using Hk.Core.IRepositorys.OrganSet;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
namespace Hk.Core.Repositorys.OrganSet
{
    public class PbdmOrganRepository:BaseRepository<PbdmOrgan,string>,IPbdmOrganRepository
    {
        public PbdmOrganRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<PbdmOrgan> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = Get();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public PbdmOrgan GetTheData(string id)
        {
             return GetSingle(id);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(PbdmOrgan newData)
        {
            Add(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(PbdmOrgan theData)
        {
            Update(theData);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            ids.ForEach(x => Delete(x));
        }

        #endregion

        #region 私有成员

        #endregion
    }
}