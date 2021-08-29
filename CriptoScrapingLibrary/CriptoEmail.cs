using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CriptoScrapingLibrary
{
    public class CriptoEmail
    {
        private string UrlScraping = string.Empty;
        public CriptoEmail(string _urlScraping)
        {
            UrlScraping = _urlScraping;
        }

        public async Task SendEmail(MailMessage message)
        {
            
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "CriptosAlertGL@gmail.com",
                    Password = ".Carac0l5"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                await Task.FromResult(0);
            }

        }
    }
}
