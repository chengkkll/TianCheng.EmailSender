
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.BaseService;
using TianCheng.DAL.MongoDB;
using TianCheng.Model;

namespace TianCheng.EmailSender
{
    /// <summary>
    /// 通知模板对象
    /// </summary>
    public class EmailConfigService : MongoBusinessService<EmailConfigInfo, EmailConfigView, EmailConfigQuery>
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dal"></param>
        public EmailConfigService(EmailConfigDAL dal) : base(dal)
        {
        }
        #endregion

        /// <summary>
        /// 保存的前置校验
        /// </summary>
        /// <param name="info"></param>
        /// <param name="logonInfo"></param>
        protected override void SavingCheck(EmailConfigInfo info, TokenLogonInfo logonInfo)
        {
            #region 验证非空
            if (string.IsNullOrWhiteSpace(info.LocalMailFromName))
            {
                throw ApiException.BadRequest("邮箱标题不允许为空");
            }
            if (string.IsNullOrWhiteSpace(info.LocalMailFromMail))
            {
                throw ApiException.BadRequest("邮箱不能为空");
            }
            if (string.IsNullOrWhiteSpace(info.LocalMailSMTP))
            {
                throw ApiException.BadRequest("SMTP 服务器不能为空");
            }
            if (string.IsNullOrWhiteSpace(info.LocalMailSMTPPort))
            {
                throw ApiException.BadRequest("SMTP 服务器端口不能为空");
            }
            if (string.IsNullOrWhiteSpace(info.LocalMailUserName))
            {
                throw ApiException.BadRequest("邮箱的用户名不能为空");
            }
            if (string.IsNullOrWhiteSpace(info.LocalMailPwd))
            {
                throw ApiException.BadRequest("邮箱的密码或授权密钥不能为空");
            }
            #endregion
        }

        #region 查询
        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override IQueryable<EmailConfigInfo> _Filter(EmailConfigQuery input)
        {
            var query = _Dal.Queryable();

            #region 查询条件
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(e => e.LocalMailFromName.Contains(input.Keyword)
                || e.LocalMailFromMail.Contains(input.Keyword)
                || e.LocalMailSMTP.Contains(input.Keyword)
                || e.LocalMailUserName.Contains(input.Keyword));
            }
            if (input.IsOpen != null)
            {
                query = query.Where(e => e.IsOpen == input.IsOpen);
            }
            #endregion

            #region 设置排序规则
            //设置排序方式
            switch (input.Sort.Property)
            {
                case "localMailSMTP": { query = input.Sort.IsAsc ? query.OrderBy(e => e.LocalMailSMTP) : query.OrderByDescending(e => e.LocalMailSMTP); break; }
                case "localMailFromName": { query = input.Sort.IsAsc ? query.OrderBy(e => e.LocalMailFromName) : query.OrderByDescending(e => e.LocalMailFromName); break; }
                case "localMailUserName": { query = input.Sort.IsAsc ? query.OrderBy(e => e.LocalMailUserName) : query.OrderByDescending(e => e.LocalMailUserName); break; }
                case "date": { query = input.Sort.IsAsc ? query.OrderBy(e => e.UpdateDate) : query.OrderByDescending(e => e.UpdateDate); break; }
                case "updaterName": { query = input.Sort.IsAsc ? query.OrderBy(e => e.UpdaterName) : query.OrderByDescending(e => e.UpdaterName); break; }
                default: { query = query.OrderBy(e => e.UpdateDate); break; }
            }
            #endregion

            //返回查询结果
            return query;
        }
        #endregion

        /// <summary>
        /// 启用配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public ResultView Enable(string id, TokenLogonInfo logonInfo)
        {
            var info = _SearchById(id);
            info.IsOpen = true;
            return Update(info, logonInfo);
        }

        /// <summary>
        /// 禁用配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logonInfo"></param>
        /// <returns></returns>
        public ResultView Disable(string id, TokenLogonInfo logonInfo)
        {
            var info = _SearchById(id);
            info.IsOpen = false;
            return Update(info, logonInfo);
        }
    }
}
