using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.EmailSender
{
    /// <summary>
    /// 发送邮件基础配置
    /// </summary>
    public class EmailConfigView
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 发送邮箱的邮箱标题
        /// </summary>
        public string LocalMailFromName { get; set; }
        /// <summary>
        /// 发送邮箱的邮箱
        /// </summary>
        public string LocalMailFromMail { get; set; }
        /// <summary>
        /// SMTP 服务器
        /// </summary>
        public string LocalMailSMTP { get; set; }
        /// <summary>
        /// SMTP 服务器端口 (qq邮箱：587)
        /// </summary>
        public string LocalMailSMTPPort { get; set; }
        /// <summary>
        /// 发送邮件邮箱的用户名
        /// </summary>
        public string LocalMailUserName { get; set; }
        /// <summary>
        /// 发送邮件邮箱的密码或授权密钥
        /// </summary>
        public string LocalMailPwd { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
