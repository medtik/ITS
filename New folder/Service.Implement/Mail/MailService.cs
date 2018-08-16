namespace Service.Implement.Mail
{
    using System;
    using System.Net.Mail;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.MailService;

    public class MailService : IMailService
    {
        private readonly ILoggingService _loggingService;

        public MailService(ILoggingService loggingService)
        {
            this._loggingService = loggingService;
        }

        public bool SendMail(string receiverMail, string subject, string content)
        {
            try
            {
                string emailAddress = "its.international2018@gmail.com";
                string password = "Dulichthongminh!";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(emailAddress, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                MailMessage mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(emailAddress, emailAddress)
                };

                mail.To.Add(new MailAddress(receiverMail));
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                client.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(SendMail), ex);

                return false;
            }
        }
    }
}