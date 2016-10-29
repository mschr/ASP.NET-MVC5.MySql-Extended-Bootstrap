using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.IdentityConfig
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // less-secure setup google apps:
        // https://www.google.com/settings/security/lesssecureapps?rfn%3D27%26rfnc%3D1%26et%3D1%26asae%3D2%26anexp%3Dlbe4-R2_B


        private string smtpProvider;

        private string sendgridUser;
        private string sendgridPass;
        private string sendgridApiName;
        private string sendgridApikey;
        private string sendgridApikeyId;

        private string fromAddress;
        private string fromName;

        public EmailService()
        {

            this.smtpProvider = ConfigurationManager.AppSettings["smtp-provider"];

            this.fromAddress = ConfigurationManager.AppSettings["default-from-address"];
            this.fromName = ConfigurationManager.AppSettings["default-from-name"];
        }

        public Task SendAsync(IdentityMessage message)
        {
            try
            {
                switch (smtpProvider)
                {
                    case "sendgrid":
                        this.sendgridUser = ConfigurationManager.AppSettings["sendgrid-user"];
                        this.sendgridPass = ConfigurationManager.AppSettings["sendgrid-pass"];
                        this.sendgridApiName = ConfigurationManager.AppSettings["sendgrid-api-name"];
                        this.sendgridApikey = ConfigurationManager.AppSettings["sendgrid-apikey"];
                        this.sendgridApikeyId = ConfigurationManager.AppSettings["sendgrid-apikey-id"];
                        return null; //sendgridMail(message);
                    default:
                        var smtpHost = ConfigurationManager.AppSettings["smtp-host"];
                        var smtpUser = ConfigurationManager.AppSettings["smtp-user"];
                        var smtpPass = ConfigurationManager.AppSettings["smtp-password"];
                        return sendGenericSmtp(message, smtpHost, smtpUser, smtpPass);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return Task.FromResult(0);
            }
        }

        private Task sendGenericSmtp(IdentityMessage message, string host, string user, string pass)
        {
            var _message = new MailMessage
            {
                From = new MailAddress(fromAddress, fromName),
                To = { new MailAddress(message.Destination) },
                Subject = message.Subject,
                Body = message.Body,
                BodyEncoding = Encoding.UTF8
            };
            log.Info(string.Format("Sending mail SMTP[host:{0}, user:{1}] from <{2}> {3} to {4}", host, user, fromAddress, fromName, message.Destination));
            using (var smtp = new SmtpClient
            {
                Host = host,
                Port = Convert.ToInt32(587),
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass)
            })
            {
                return smtp.SendMailAsync(_message);
            }
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
        }
        /* SendGrid needed 
           Install-Package Sendgrid
        */
        /*
        private Task sendgridMail(IdentityMessage message)
        {
            //var personalization = new Personalization();
            //personalization.AddTo(to);
            //_message.AddPersonalization(personalization);
            return Task.Run(async () =>
            {
                dynamic sg = new SendGridAPIClient(sendgridApikey);
                Content content = new Content("text/html", message.Body);
                Mail _message = new Mail(
                    new Email(fromAddress, fromName),
                    message.Subject,
                    new Email(message.Destination),
                    content);
                try
                {
                    SendGrid.CSharp.HTTP.Client.Response response = await sg.client.mail.send.post(requestBody: _message.Get());
                    log.Info("SendGrid mail[" + message.Subject + "] to: " + message.Destination);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write(e);
                }
            });

        }
        private void sendgridMailSync(IdentityMessage message)
        {
            dynamic sg = new SendGridAPIClient(sendgridApikey);
            Content content = new Content("text/html", message.Body);
            Mail _message = new Mail(
                new Email(fromAddress, fromName),
                message.Subject,
                new Email(message.Destination),
                content);
            //var personalization = new Personalization();
            //personalization.AddTo(to);
            //_message.AddPersonalization(personalization);
            sg.client.mail.send.post(requestBody: _message.Get());

        }
        */
    }
}
