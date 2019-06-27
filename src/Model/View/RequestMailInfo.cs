using System.Collections.Generic;

namespace TianCheng.EmailSender
{
    /// <summary>
    /// 邮件发送实体
    /// </summary>
    public class RequestMailInfo
    {
        /// <summary>
        /// 接收邮箱
        /// </summary>
        public string ReceiveMail { get; set; }
        /// <summary>
        /// 抄送邮箱
        /// </summary>
        public string CCMail { get; set; }
        /// <summary>
        /// 发送标题
        /// </summary>
        public string SendTitle { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent { get; set; }

        /// <summary>
        /// 附件 地址
        /// </summary>
        public string AttachmentUrl { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<string> AttachmentList { get; set; } = new List<string>();
        /// <summary>
        /// 发送格式类型
        /// </summary>
        public FormatTypeEnum FormatType { get; set; } = FormatTypeEnum.TextBody;
    }
}
