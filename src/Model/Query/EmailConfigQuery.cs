using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;

namespace TianCheng.EmailSender
{
    public class EmailConfigQuery : QueryInfo
    {
        /// <summary>
        /// 查询的关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool? IsOpen { get; set; }
    }
}
