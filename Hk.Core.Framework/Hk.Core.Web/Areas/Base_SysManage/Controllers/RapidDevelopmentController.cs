using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace Hk.Core.Web.Areas.Base_SysManage.Controllers
{
    [Area("Base_SysManage")]
    public class RapidDevelopmentController : BaseMvcController
    {
        private readonly IRapidDevelopmentRepository _rapidDevelopmentRepository;

        public RapidDevelopmentController(IRapidDevelopmentRepository rapidDevelopmentRepository)
        {
            _rapidDevelopmentRepository = rapidDevelopmentRepository;
        }

        #region 视图功能

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取所有数据库连接
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDbLink()
        {
            var dataList = _rapidDevelopmentRepository.GetAllDbLink();

            return Content(dataList.ToJson());
        }

        /// <summary>
        /// 获取数据库所有表
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        public ActionResult GetDbTableList(string linkId)
        {
            Pagination pagination = new Pagination
            {
                PageIndex = 1,
                PageRows = int.MaxValue,
                RecordCount = int.MaxValue
            };

            return Content(pagination.BuildTableResult_DataGrid(_rapidDevelopmentRepository.GetDbTableList(linkId)).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="linkId">连接Id</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tables">表列表</param>
        /// <param name="buildType">需要生成类型</param>
        public ActionResult BuildCode(string linkId, string areaName, string tables, string buildType)
        {
            _rapidDevelopmentRepository.BuildCode(linkId, areaName, tables, buildType);

            return Success("生成成功！");
        }

        #endregion
    }
}