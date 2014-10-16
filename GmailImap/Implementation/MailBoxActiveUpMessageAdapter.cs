using System.IO;
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

        public dynamic Attachements { get; private set; }
        public Stream DecodeAttachementFromMessage(out string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}