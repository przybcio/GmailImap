using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActiveUp.Net.Mail;
using GmailImap.Abstract;

namespace GmailImap.Implementation
{
    public class GmailActiveUpRepository : IGmailRepository
    {
        private Imap4Client client;

        public GmailActiveUpRepository()
        {
            
        }
        public GmailActiveUpRepository(string mailServer, int port, bool ssl, string login, string password)
        {
            if (ssl)
                Client.ConnectSsl(mailServer, port);
            else
                Client.Connect(mailServer, port);
            Client.Login(login, password);
        }

        public IEnumerable<Message> GetAllMails(string mailBox)
        {
            return GetMails(mailBox, "ALL").Cast<Message>();
        }

        public IEnumerable<Message> GetUnreadMails(string mailBox)
        {
            return GetMails(mailBox, "UNSEEN").Cast<Message>();
        }

        protected Imap4Client Client
        {
            get { return client ?? (client = new Imap4Client()); }
        }

        private MessageCollection GetMails(string mailBox, string searchPhrase)
        {
            Mailbox mails = Client.SelectMailbox(mailBox);
            MessageCollection messages = mails.SearchParse(searchPhrase);
            return messages;
        }

        public IEnumerable<IMailBoxMessage> GetMessages(string query)
        {
            using (var client1 = new Imap4Client())
            {
                Setup(client1);
                var selectMailbox = client1.SelectMailbox("inbox");
                var messageCollection = selectMailbox.SearchParse(query).Cast<Message>().Select(message => (CommonMailBoxMessage)message).ToList();
                client1.Disconnect();
                return messageCollection;
            }
        }

        public Stream DecodeAttachementFromMessage(IMailBoxMessage mailBoxMessage, out string fileName)
        {
            throw new System.NotImplementedException();
        }

        private void Setup(Imap4Client client1)
        {
            client1.ConnectSsl("imap.gmail.com", 993);
            client1.Login("@gmail.com", "");
            
        }
    }
}
