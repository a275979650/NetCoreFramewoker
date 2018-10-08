using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Hk.Core.Util.Datas;

namespace Hk.Core.Repositorys
{
    public class BaseAppSecretRepository:BaseRepository<Base_AppSecret,string>,IBaseAppSecretRepository
    {
        public BaseAppSecretRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public bool IsSecurity(HttpContext context)
        {
            try
            {
                var request = context.Request;
                var allRequestParams = GetAllRequestParams(context);
                string appId = allRequestParams["appId"]?.ToString();
                string appSecret = GetAppSecret(appId);

                return CheckSign(allRequestParams, appSecret);
            }
            catch
            {
                return false;
            }
        }

        public string GetAppSecret(string appId)
        {
            return Get().FirstOrDefault(x => x.AppId == appId)?.AppSecret;
        }

        public List<Base_AppSecret> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = Get();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        public Base_AppSecret GetTheData(string id)
        {
            return GetSingle(id);
        }

        public void AddData(Base_AppSecret newData)
        {
            Add(newData);
        }

        public void UpdateData(Base_AppSecret theData)
        {
            Update(theData);
        }

        public void DeleteData(List<string> ids)
        {
            ids.ForEach(x => Delete(x));
        }

        public void SavePermission(string appId, List<string> permissions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 构建安全的请求参数(默认签名规则)
        /// </summary>
        /// <param name="businessParams">业务参数（不包含校验参数）</param>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        public static Dictionary<string, object> BuildSafeHttpParam(Dictionary<string, object> businessParams, string appId, string appSecret)
        {
            Dictionary<string, object> requestParames = new Dictionary<string, object>();

            if (businessParams != null)
            {
                foreach (var aParam in businessParams)
                {
                    requestParames.Add(aParam.Key, aParam.Value);
                }
            }

            requestParames.Add("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParames.Add("appId", appId);

            string sign = BuildSign(requestParames, appSecret);
            requestParames.Add("sign", sign);

            return requestParames;
        }

        /// <summary>
        /// 获取所有请求的参数（包括get参数和post参数）
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        private Dictionary<string, object> GetAllRequestParams(HttpContext context)
        {
            return HttpHelper.GetAllRequestParams(context);
        }
        /// <summary>
        /// 检验签名是否有效
        /// </summary>
        /// <param name="allRequestParames">所有的请求参数</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        private bool CheckSign(Dictionary<string, object> allRequestParames, string appSecret)
        {
            try
            {
                //检验签名是否过期
                DateTime now = DateTime.Now;
                DateTime requestTime = Convert.ToDateTime(allRequestParames["time"]?.ToString());
                if (requestTime < now.AddMinutes(-5) || requestTime > now.AddMinutes(5))
                    return false;

                //检验签名是否有效
                string oldSign = allRequestParames["sign"]?.ToString();
                Dictionary<string, object> parames = new Dictionary<string, object>();
                foreach (var aParam in allRequestParames)
                {
                    parames.Add(aParam.Key, aParam.Value);
                }

                parames.Remove("sign");
                string newSign = BuildSign(parames, appSecret);

                return oldSign == newSign;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 构造签名
        /// </summary>
        /// <param name="needParames">需要的参数（不包括sign）</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        private static string BuildSign(Dictionary<string, object> needParames, string appSecret)
        {
            var sortedParams = new SortedDictionary<string, object>(new AsciiComparer());
            foreach (var aParam in needParames)
            {
                sortedParams.Add(aParam.Key, aParam.Value);
            }

            StringBuilder signBuilder = new StringBuilder();
            foreach (var aParam in sortedParams)
            {
                var value = aParam.IsNullOrEmpty() ? string.Empty : aParam.Value.ToString();
                signBuilder.Append($@"{aParam.Key}{value}");
            }
            signBuilder.Append(appSecret);
            string sign = signBuilder.ToString().ToMD5String().ToUpper();

            return sign;
        }
    }
}