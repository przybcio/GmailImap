using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GmailImap.Abstract;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace GmailImap.Implementation
{
    public class GmailKitRepository : IGmailRepository
    {
        public IList<MimeMessage> GetUnreadMails()
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH");

                client.Authenticate("@gmail.com", "");

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var query = SearchQuery.NotSeen;
                IList<MimeMessage> messages =
                    inbox.Search(query).Select(uniqueId => inbox.GetMessage(uniqueId)).ToList();
                client.Disconnect(true);
                return messages;
            }
        }

        public IEnumerable<IMailBoxMessage> GetMessages(string query)
        {
            using (var client = new ImapClient())
            {
                Setup(client);
                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var searchQuery = Parse(query);
                var messages =
                    inbox.Search(searchQuery)
                         .Select(uniqueId => inbox.GetMessage(uniqueId))
                         .Select(mimeMessage => (CommonMailBoxMessage) mimeMessage).ToList();
                
                client.Disconnect(true);
                
                return messages;

            }
        }

        private static SearchQuery Parse(string query)
        {
            return SearchQuery.DeliveredAfter(DateTime.Parse(query));
        }

        private static void Setup(MailService client)
        {
            if (client == null) throw new ArgumentNullException("client");
            client.Connect("imap.gmail.com", 993, true);
            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH");
            client.Authenticate("@gmail.com", "");
        }
    }
}