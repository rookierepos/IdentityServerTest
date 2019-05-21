using System;

namespace Global
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 身份模式
        /// </summary>
        public static readonly IdentityMode IdentityMode = IdentityMode.AuthorizationCode;
    }

    /// <summary>
    /// 身份模式
    /// </summary>
    public enum IdentityMode
    {
        /// <summary>
        /// 自带身份验证模式
        /// </summary>
        OriginalIdentity,
        /// <summary>
        /// 客户端模式
        /// </summary>
        ClientCredentials,
        /// <summary>
        /// 密码模式
        /// </summary>
        OwnerPassword,
        /// <summary>
        /// 简化模式
        /// </summary>
        Implicit,
        /// <summary>
        /// 授权码模式
        /// </summary>
        AuthorizationCode
    }
}
