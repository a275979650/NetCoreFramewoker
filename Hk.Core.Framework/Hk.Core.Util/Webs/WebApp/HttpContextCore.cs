using Hk.Core.Util.Helper;
using Microsoft.AspNetCore.Http;

namespace Hk.Core.Util.Webs.WebApp
{
    public static class HttpContextCore
    {
        public static HttpContext Current { get => AutofacHelper.GetService<IHttpContextAccessor>().HttpContext; }
    }
}