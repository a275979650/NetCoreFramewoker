using Hk.Core.Business.BaseBusiness;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hk.Core.Data.DbContextCore;

namespace Hk.Core.Business.Base_SysManage
{
    public class Base_DatabaseLinkBusiness : BaseBusiness<Base_DatabaseLink,string>
    {
        public Base_DatabaseLinkBusiness(IDbContextCore dbContext) : base(dbContext)
        {
        }
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Base_DatabaseLink> GetDataList(string condition, string keyword, Pagination pagination)
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
        public Base_DatabaseLink GetTheData(string id)
        {
            return GetSingle(id);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(Base_DatabaseLink newData)
        {
            Add(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(Base_DatabaseLink theData)
        {
            Update(theData);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            ids.ForEach(x=>Delete(x));
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion


    }
}