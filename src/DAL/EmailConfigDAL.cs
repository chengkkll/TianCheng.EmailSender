using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.DAL;
using TianCheng.DAL.MongoDB;

namespace TianCheng.EmailSender
{
    /// <summary>
    /// 通知模板信息
    /// </summary>
    [DBMapping("system_email_config")]
    public class EmailConfigDAL : MongoOperation<EmailConfigInfo>
    {
    }
}
