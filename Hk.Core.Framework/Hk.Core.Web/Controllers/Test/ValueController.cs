using Hk.Core.IRepositorys;
using Hk.Core.Web.Test;
using HK.Core.Webs.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Hk.Core.Entity.OrganSet;
using Hk.Core.Util.Extentions;

namespace Hk.Core.Web.Controllers
{
    public class ValueController : WebApiControllerBase
    {
        private readonly ICustomService _service;
        private readonly ITestRepository _testRepository;
        public ValueController(ICustomService service, ITestRepository testRepository)
        {
            _service = service;
            _testRepository = testRepository;
        }
        [HttpGet("GetValue")]
        public Task<IActionResult> GetValue(string id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                _service.Call("123");      
                
               // _testRepository.Delete<PbdmOrgan, string>("");
                var a =  _testRepository.Get<PbdmOrgan>().FirstOrDefault(x => x.Id == "1");
                DataTable dataTable = _testRepository.GetDataTableWithSql("SELECT * FROM dbo.PBDM_ORGAN");
                return Success(dataTable.ToJson());
            });

        }

        [HttpGet("GetTest")]
        public Task<IActionResult> GetTest(string id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                string a = "失败";
                throw new Exception(a);
            });

        }
    }
}