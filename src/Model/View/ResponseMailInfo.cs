namespace TianCheng.EmailSender
{
    /// <summary>
    /// 发送邮件返回实体
    /// </summary>
    public class ResponseMailInfo
    {
        /// <summary>
        /// 0: 表示请求成功
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// true 请求成功
        /// </summary>
        public bool state { get; set; }
    }
}
