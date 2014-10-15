using ActiveUp.Net.Mail;
using GmailImap.Abstract;
using MimeKit;

namespace GmailImap.Implementation
{
    public class MailBoxActiveUpMessageAdapter : IMailBoxMessage
    {
        private readonly Message _message;

        public MailBoxActiveUpMessageAdapter(Message message)
        {
            _message = message;
        }


        public string Subject
        {
            get { return _message.Subject; }
        }
    }
}