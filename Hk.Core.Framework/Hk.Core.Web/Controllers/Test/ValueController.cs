using System;
using Hk.Core.Web.Test;
using HK.Core.Webs.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hk.Core.Web.Controllers
{
    public class ValueController : WebApiControllerBase
    {
        private readonly ICustomService _service;
        public ValueController(ICustomService service)
        {
            _service = service;
        }
        [HttpGet("GetValue")]
        public Task<IActionResult> GetValue(string id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                _service.Call("123");
                return Success();
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