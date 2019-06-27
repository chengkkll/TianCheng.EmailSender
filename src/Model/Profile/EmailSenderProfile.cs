using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;

namespace TianCheng.EmailSender.Model
{
    /// <summary>
    /// 邮件服务的AutoMapper转换
    /// </summary>
    public class EmailSenderProfile : Profile, IAutoProfile
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public EmailSenderProfile()
        {
            Register();
        }
        /// <summary>
        /// 注册需要转换的对象
        /// </summary>
        public void Register()
        {
            CreateMap<EmailConfigInfo, EmailConfigView>();
            CreateMap<EmailConfigView, EmailConfigInfo>();

        }
    }
}
