using System;
using ActiveUp.Net.Mail;

namespace GmailImap.Abstract
{
    public interface IMailBoxMessage
    {
        string Subject { get; }
     
    }

    public class CommonMailBoxMessage : IMailBoxMessage
    {

        public static explicit operator CommonMailBoxMessage(MimeKit.MimeMessage message)
        {
            return new CommonMailBoxMessage {Subject = message.Subject};
        }

        public static explicit operator CommonMailBoxMessage(Message message)
        {
            return new CommonMailBoxMessage {Subject = message.Subject};
        }

        public string Subject { get; set; }


    }
}
