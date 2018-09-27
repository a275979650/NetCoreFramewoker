using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Sessions;
using IdentityModel;

namespace Hk.Core.Security.Sessions
{
    /// <summary>
    /// 用户会话
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// 空用户会话
        /// </summary>
        public static readonly ISession Null = NullSession.Instance;

        /// <summary>
        /// 用户会话
        /// </summary>
        public static readonly ISession Instance = new Session();

        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthenticated => WebHelper.Identity.IsAuthenticated;

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId
        {
            get
            {
                var result = WebHelper.Identity.GetValue(JwtClaimTypes.Subject);
                return string.IsNullOrWhiteSpace(result) ? WebHelper.Identity.GetValue(System.Security.Claims.ClaimTypes.NameIdentifier) : result;
            }
        }
    }
}