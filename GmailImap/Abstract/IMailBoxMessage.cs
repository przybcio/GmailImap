using System;
using System.Collections.Generic;
using System.IO;
using ActiveUp.Net.Mail;
using MimeKit;

namespace GmailImap.Abstract
{
    public interface IMailBoxMessage
    {
        string Subject { get; }
        dynamic Attachements { get; }
        Stream DecodeAttachementFromMessage(out string fileName);
    }

    public class CommonMailBoxMessage : IMailBoxMessage
    {

        public static explicit operator CommonMailBoxMessage(MimeKit.MimeMessage message)
        {
            return new CommonMailBoxMessage {Subject = message.Subject, Attachements = message.Attachments};
        }

        public static explicit operator CommonMailBoxMessage(Message message)
        {
            return new CommonMailBoxMessage {Subject = message.Subject, Attachements = message.Attachments};
        }

        public string Subject { get; set; }
        public dynamic Attachements { get; private set; }

        public Stream DecodeAttachementFromMessage(out string fileName)
        {
            var attachements = Attachements as IEnumerable<MimeKit.MimePart>;
            if (attachements != null)
            {
                foreach (MimeKit.MimePart attachment in attachements)
                {
                    fileName = DateTime.Now.ToFileTimeUtc() + attachment.FileName;
                    var fileStream = new FileStream(fileName, FileMode.CreateNew);
                    attachment.ContentTransferEncoding = ContentEncoding.Base64;
                    attachment.ContentObject.DecodeTo(fileStream);
                    return fileStream;
                }
            }
            fileName = string.Empty;
            return null;
        }
    }
}
