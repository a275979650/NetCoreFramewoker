using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hk.Core.Web.Common
{
    public static class FilterHelper
    {
        public static List<string> GetFilterList(ActionExecutingContext context)
        {
            return context.Filters.Select(x => x.GetType().FullName).ToList();
        }
    }
}