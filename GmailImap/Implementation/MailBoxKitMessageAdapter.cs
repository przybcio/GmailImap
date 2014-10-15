﻿using GmailImap.Abstract;
using MimeKit;

namespace GmailImap.Implementation
{
    public class MailBoxKitMessageAdapter : IMailBoxMessage
    {
        private readonly MimeMessage _message;

        public MailBoxKitMessageAdapter(MimeMessage message)
        {
            _message = message;
        }


        public string Subject
        {
            get { return _message.Subject; }
        }
    }
}