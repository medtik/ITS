namespace Core.ApplicationService.Business.MailService
{
    public interface IMailService
    {
        bool SendMail(string receiverMail, string subject, string content);
    }
}