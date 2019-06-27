using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using TianCheng.BaseService;
using TianCheng.Model;

namespace TianCheng.EmailSender
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    [Produces("application/json")]
    [Route("api/System/SettingEmail")]
    public class EmailConfigController : DataController
    {
        #region 构造方法
        private readonly EmailConfigService _Service;
        private readonly ILogger<EmailConfigController> _logger;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>          
        public EmailConfigController(EmailConfigService service, ILogger<EmailConfigController> logger)
        {
            _Service = service;
            _logger = logger;
        }
        #endregion

        #region 新增修改数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="view">请求体中放置新增对象的信息</param>
        /// <power>保存</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Save")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [HttpPost("")]
        public ResultView Create([FromBody]EmailConfigView view)
        {
            view.IsOpen = true;
            //保存数据
            return _Service.Create(view, LogonInfo);
        }

        /// <summary>
        /// 修改 
        /// </summary>
        /// <param name="view">请求体中带入修改对象的信息</param>
        /// <power>保存</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Save")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [HttpPut("")]
        public ResultView Update([FromBody]EmailConfigView view)
        {
            //保存数据
            return _Service.Update(view, LogonInfo);
        }

        #endregion

        #region 数据删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">要逻辑删除的对象ID</param>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Remove")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [Route("Remove/{id}")]
        [HttpDelete]
        public ResultView Remove(string id)
        {
            return _Service.Remove(id, LogonInfo);
        }
        #endregion

        #region 数据查询
        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="id">要获取的对象ID</param>
        /// <returns></returns>
        /// <power>查询</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [HttpGet("{id}")]
        public EmailConfigView SearchById(string id)
        {
            return _Service.SearchById(id);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <remarks> 
        ///     排序规则包含： 
        /// 
        ///         localMailSMTP           : SMTP   
        ///         localMailFromName       : 发送邮箱的邮箱标题   
        ///         localMailUserName       : 发送邮件邮箱的用户名    
        ///         updaterName             : 更新人名称 
        ///         date                    : 按最后更新时间排列   为默认排序
        ///         
        /// </remarks> 
        /// <param name="queryInfo">查询信息。（包含分页信息、查询条件、排序条件）
        /// 排序规则参见上面的描述
        /// </param>
        /// <power>查询</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Search")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [HttpPost("Search")]
        public PagedResult<EmailConfigView> Search([FromBody]EmailConfigQuery queryInfo)
        {
            return _Service.FilterPage(queryInfo);
        }
        #endregion

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id">要获取的对象ID</param>
        /// <returns></returns>
        /// <power>禁用</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Disable")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [HttpPatch("Disable/{id}")]
        public ResultView Disable(string id)
        {
            return _Service.Disable(id, LogonInfo);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id">要获取的对象ID</param>
        /// <returns></returns>
        /// <power>启用</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Enable")]
        [SwaggerOperation(Tags = new[] { "系统管理-邮件服务" })]
        [HttpPatch("Enable/{id}")]
        public ResultView Enable(string id)
        {
            return _Service.Enable(id, LogonInfo);
        }

        /// <summary>
        /// 测试发送
        /// </summary>
        /// <param name="view"></param>
        /// <power>测试</power>
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "SystemManage.SettingEmail.Test")]
        [SwaggerOperation(Tags = new[] { "项目操作-通知模板信息" })]
        [HttpPost("TestSend/{id}")]
        public void TestSend([FromBody]RequestMailInfo view, string id)
        {
            var config = _Service.SearchById(id);
            EmailConfigInfo emailConfigInfo = new EmailConfigInfo()
            {
                LocalMailFromName = config.LocalMailFromName, //发送邮箱的邮箱标题
                LocalMailFromMail = config.LocalMailFromMail, //发送邮箱的邮箱
                LocalMailSMTP = config.LocalMailSMTP, //smtp 服务器
                LocalMailSMTPPort = config.LocalMailSMTPPort, //smtp 服务器端口
                LocalMailUserName = config.LocalMailUserName, //发送邮件邮箱的用户名
                LocalMailPwd = config.LocalMailPwd //发送邮件邮箱的密码
            };

            EmailSenderService.LocalSendSync(view, emailConfigInfo);
        }
    }
}