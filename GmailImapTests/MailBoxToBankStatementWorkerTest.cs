using System.IO;
using BankStatementProvider;
using GmailImap.Abstract;
using MimeKit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GmailImapTests
{
    [TestFixture]
    public class MailBoxToBankStatementWorkerTest
    {

        [Test]
        public void Should_process_one_mailbox_with_one_attachement()
        {
            MailBoxToBankStatementWorker worker = new MailBoxToBankStatementWorker();

            MailMessage message = new MailMessage {Subject = "Test subject"};
            MimeMessage messageToTest = null;
            using (var fileStream = new StreamReader("t3VISA CLASSIC  nr 86.html", Encoding.GetEncoding("ISO-8859-2")))
            {
                message.Attachments.Add(new Attachment(fileStream.BaseStream, "bank statement file"));
                Assert.NotNull(message.Attachments[0], "Nie udało się dodać załącznika");
                messageToTest = MimeMessage.CreateFromMailMessage(message);
            }
            Assert.NotNull(messageToTest, "Nie udało się stworzyć maila do testów");
            Assert.IsTrue(messageToTest.Attachments.Any(), "Nie udało się stworzyć maila do testów z załącznikiem");

            IEnumerable<TransactionDto> processMessages = worker.ProcessMessages(new List<IMailBoxMessage> {(CommonMailBoxMessage) messageToTest});
            Assert.NotNull(processMessages, "Lista transakcji nie została utworzona");
            Assert.IsTrue(processMessages.Any(), "Lista transakcji jest pusta");
            Assert.IsTrue(processMessages.Count() == 19, "Lista transakcji nie posiada odpowiedniej ilości komunikatów");
        }
    }
}
