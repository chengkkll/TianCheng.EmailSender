using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TianCheng.EmailSender
{
    public static class EmailSenderService
    {
        #region 发送邮件
        /// <summary>
        /// 同步 发送邮件
        /// </summary>
        /// <param name="requestMailInfo"></param>
        /// <param name="mailConfig"></param>
        /// <returns></returns>
        public static ResponseMailInfo LocalSend(RequestMailInfo requestMailInfo, EmailConfigInfo mailConfig = null)
        {
            return LocalSendMail(requestMailInfo, mailConfig);
        }

        /// <summary>
        /// 异步 发送邮件
        /// </summary>
        /// <param name="requestMailInfo"></param>
        /// <param name="mailConfig"></param>
        public static void LocalSendSync(RequestMailInfo requestMailInfo, EmailConfigInfo mailConfig = null)
        {
            Task task = new Task(() =>
            {
                LocalSendMail(requestMailInfo, mailConfig);
            });
            task.Start();
        }

        private static ResponseMailInfo LocalSendMail(RequestMailInfo requestMailInfo, EmailConfigInfo mailConfig = null)
        {
            ResponseMailInfo responseMailInfo = new ResponseMailInfo();
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(mailConfig.LocalMailFromName, mailConfig.LocalMailFromMail));
                IList<MailboxAddress> mailboxAddresses = new List<MailboxAddress>();
                if (string.IsNullOrWhiteSpace(requestMailInfo.ReceiveMail))
                {
                    responseMailInfo = new ResponseMailInfo
                    {
                        state = false,
                        message = $"邮件发送失败:请填写收件人邮箱"
                    };
                    return responseMailInfo;
                }
                if (requestMailInfo.ReceiveMail != null)
                {
                    requestMailInfo.ReceiveMail = requestMailInfo.ReceiveMail.Substring(requestMailInfo.ReceiveMail.Length - 1, 1) == ";" ? requestMailInfo.ReceiveMail.Remove(requestMailInfo.ReceiveMail.Length - 1, 1) : requestMailInfo.ReceiveMail;
                    requestMailInfo.ReceiveMail.Split(';').All(s =>
                    {
                        mailboxAddresses.Add(new MailboxAddress("", s));
                        return true;
                    });
                }
                message.To.AddRange(mailboxAddresses);
                message.Subject = requestMailInfo.SendTitle;

                IList<MailboxAddress> ccmailboxAddresses = new List<MailboxAddress>();
                if (requestMailInfo.CCMail != null)
                {
                    requestMailInfo.CCMail = requestMailInfo.CCMail.Substring(requestMailInfo.CCMail.Length - 1, 1) == ";" ? requestMailInfo.CCMail.Remove(requestMailInfo.CCMail.Length - 1, 1) : requestMailInfo.CCMail;
                    requestMailInfo.CCMail.Split(';').All(s =>
                    {
                        ccmailboxAddresses.Add(new MailboxAddress("", s));
                        return true;
                    });
                }
                message.Cc.AddRange(ccmailboxAddresses);

                var builder = new BodyBuilder();
                if (requestMailInfo.FormatType == FormatTypeEnum.TextBody)
                {
                    builder.TextBody = requestMailInfo.SendContent;
                }
                else if (requestMailInfo.FormatType == FormatTypeEnum.HtmlBody)
                {
                    builder.HtmlBody = requestMailInfo.SendContent;
                }

                //为了处理 中文乱码和 名称太长的问题...
                List<FileStream> list = new List<FileStream>(requestMailInfo.AttachmentList.Count());
                foreach (var path in requestMailInfo.AttachmentList)
                {
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("文件未找到", path);
                    }
                    var fileName = Path.GetFileName(path);
                    var fileType = MimeTypes.GetMimeType(path);
                    var contentTypeArr = fileType.Split('/');
                    var contentType = new ContentType(contentTypeArr[0], contentTypeArr[1]);
                    MimePart attachment = null;
                    var fs = new FileStream(path, FileMode.Open);
                    list.Add(fs);
                    attachment = new MimePart(contentType)
                    {
                        Content = new MimeContent(fs),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                    };
                    var charset = "GB18030";
                    attachment.ContentType.Parameters.Add(charset, "name", fileName);
                    attachment.ContentDisposition.Parameters.Add(charset, "filename", fileName);
                    foreach (var param in attachment.ContentDisposition.Parameters)
                    {
                        param.EncodingMethod = ParameterEncodingMethod.Rfc2047;
                    }
                    foreach (var param in attachment.ContentType.Parameters)
                    {
                        param.EncodingMethod = ParameterEncodingMethod.Rfc2047;
                    }
                    builder.Attachments.Add(attachment);
                }
                message.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(mailConfig.LocalMailSMTP, Convert.ToInt32(mailConfig.LocalMailSMTPPort), false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(mailConfig.LocalMailUserName, mailConfig.LocalMailPwd);

                    client.Send(message);
                    client.Disconnect(true);
                }
                foreach (var fs in list)
                {
                    fs.Dispose();//手动高亮
                }
                responseMailInfo = new ResponseMailInfo
                {
                    state = true,
                    message = "发送成功"
                };
            }
            catch (Exception ex)
            {
                responseMailInfo = new ResponseMailInfo
                {
                    state = false,
                    message = $"邮件发送失败:{ex.Message}"
                };
            }
            return responseMailInfo;
        }
        #endregion
        private static string ToLink(Dictionary<string, string> Dictionary)
        {
            string buff = "";
            foreach (KeyValuePair<string, string> pair in Dictionary)
            {
                buff += pair.Value + "";
            }
            return buff;
        }
        private static string ToUrl(Dictionary<string, string> Dictionary)
        {
            string buff = "";
            foreach (KeyValuePair<string, string> pair in Dictionary)
            {
                if (pair.Key != "Key")
                {
                    buff += pair.Key + "=" + pair.Value + "&"; ;
                }
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}
