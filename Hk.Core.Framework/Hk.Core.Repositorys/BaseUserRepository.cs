using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Hk.Core.Business.Cache;
using Hk.Core.Business.Common;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hk.Core.Repositorys
{
    public class BaseUserRepository:BaseRepository<Base_User,string>,IBaseUserRepository
    {
        public BaseUserRepository(IDbContextCore dbContext) : base(dbContext)
        {
           
        }  
    }
}