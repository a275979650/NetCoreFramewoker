using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.OrganSet;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;
namespace Hk.Core.IRepositorys.OrganSet
{
    public interface IPbdmOrganRepository:IRepository<PbdmOrgan,string>,IScopeDependency
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        List<PbdmOrgan> GetDataList(string condition, string keyword, Pagination pagination);
        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        PbdmOrgan GetTheData(string id);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        void AddData(PbdmOrgan newData);
        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateData(PbdmOrgan theData);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        void DeleteData(List<string> ids);
    }
}