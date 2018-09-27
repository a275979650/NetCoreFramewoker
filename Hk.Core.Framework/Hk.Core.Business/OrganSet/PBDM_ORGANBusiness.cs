using Hk.Core.Entity.OrganSet;
using Hk.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Hk.Core.Business.BaseBusiness;

namespace Hk.Core.Business.OrganSet
{
    public class PBDM_ORGANBusiness : BaseBusiness<PBDM_ORGAN>
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<PBDM_ORGAN> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

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
        public PBDM_ORGAN GetTheData(string id)
        {
            return GetEntity(id);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(PBDM_ORGAN newData)
        {
            Insert(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(PBDM_ORGAN theData)
        {
            Update(theData);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            Delete(ids);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}