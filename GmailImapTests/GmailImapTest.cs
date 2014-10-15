using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ActiveUp.Net.Mail;
using GmailImap;
using GmailImap.Implementation;
using MimeKit;
using NUnit.Framework;
using MimePart = ActiveUp.Net.Mail.MimePart;
using System.Xml.Serialization;

namespace GmailImapTests
{
    [TestFixture]
    public class GmailImapTest
    {
        [Test]
        public void Test1()
        {
            object o = null;
            Assert.IsNull(o);
        }

        [Test]
        public void ReadImap()
        {
            var mailRepository = new GmailActiveUpRepository(
                                    "imap.gmail.com",
                                    993,
                                    true,
                                    "@gmail.com",
                                    ""
                                );

            var emailList = mailRepository.GetUnreadMails("inbox");

            Debug.WriteLine("Number of unread messages: " + emailList.Count());
            foreach (Message email in emailList)
            {
                Console.WriteLine("<p>{0}: {1}</p><p>{2}</p>", email.From, email.Subject, email.BodyHtml.Text);
                if (email.Attachments.Count > 0)
                {
                    foreach (MimePart attachment in email.Attachments)
                    {
                        Console.WriteLine("<p>Attachment: {0} {1}</p>", attachment.ContentName, attachment.ContentType.MimeType);
                        attachment.StoreToFile("test" + attachment.Filename);
                    }
                }
            }
        }

        [Test]
        public void ReadImap_withKit()
        {
            var rep = new GmailKitRepository();
            var emailList = rep.GetUnreadMails();
            Debug.WriteLine("Number of unread messages: " + emailList.Count());
            foreach (var mimeMessage in emailList)
            {
                Console.WriteLine("<p>{0}: {1}</p><p>{2}</p>", mimeMessage.From, mimeMessage.Subject, mimeMessage.Body);
                if (mimeMessage.Attachments.Any())
                {
                    foreach (MimeKit.MimePart attachment in mimeMessage.Attachments)
                    {
                        Console.WriteLine("<p>Attachment: {0} {1}</p>", attachment.ContentType, attachment.ContentType.MimeType);
                        var fileStream = new FileStream("t3" + attachment.FileName, FileMode.CreateNew);
                        attachment.ContentTransferEncoding = ContentEncoding.Base64;
                        attachment.ContentObject.DecodeTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
        }

        [Test]
        public void ReadMailsAfterSomePointInTime_withMimeKit()
        {
            string query = "2014-09-23";

            var mailBoxMessages =
                new GmailFactoryRepository().GetRepository(GmailRepositoryType.MimeKit).GetMessages(query);
            Assert.NotNull(mailBoxMessages);
            Assert.That(mailBoxMessages.Count(), Is.GreaterThan(0));
            foreach (var result in mailBoxMessages)
            {
                Debug.WriteLine(result.Subject);
            }
        }

        [Test]
        public void ReadMailsAfterSomePointInTime_withActionUp()
        {
            string query = "SINCE 2014-09-23";

            var mailBoxMessages =
                new GmailFactoryRepository().GetRepository(GmailRepositoryType.ActiveUp).GetMessages(query);
            Assert.NotNull(mailBoxMessages);
            Assert.That(mailBoxMessages.Count(), Is.GreaterThan(0));
            foreach (var result in mailBoxMessages)
            {
                Debug.WriteLine(result.Subject);
            }
        }

        private class MyClass
        {
            public int Id { get; set; }
            public DateTime OperDate { get; set; }
            public DateTime AccntDate { get; set; }
            public string OperKindDesc { get; set; }
            public string OperDesc { get; set; }
            public decimal Amount { get; set; }
            public decimal Amount2 { get; set; }
            public override string ToString()
            {
                return
                    string.Format(
                        "Id: {0}, OperDate: {1}, AccntDate: {2}, OperKindDesc: {3}, OperDesc: {4}, Amount: {5}, Amount2: {6}",
                        Id, OperDate, AccntDate, OperKindDesc, OperDesc, Amount, Amount2);
            }
        }

        [Test]
        public void T2()
        {
            using (var fileStream = new StreamReader("t3VISA CLASSIC  nr 86.html", Encoding.GetEncoding("ISO-8859-2")))
            using (var reader = XmlReader.Create(fileStream))
            {
                StringBuilder sb = new StringBuilder();
                bool startSaving = false;
                bool switching = false;
                bool startSaving2 = false;
                int counter = 0;
                while (reader.Read())
                {
                    var trim = reader.ReadString().Trim();
                    if (!string.IsNullOrEmpty(trim))
                    {
                        if (trim.StartsWith("Kwota w PLN") && !switching)
                        {
                            startSaving = true;
                            continue;
                        }
                        if (trim.StartsWith("Kwota w PLN") && switching)
                        {
                            startSaving2 = true;
                            counter = 0;
                            continue;
                        }
                        if (startSaving && trim.StartsWith("HISTORIA TRANSAKCJI DLA KARTY"))
                        {
                            startSaving = false;
                            switching = true;
                            continue;
                        }
                        if (startSaving2 && trim.StartsWith("SALDO KOŃCOWE:"))
                        {
                            startSaving2 = false;
                            continue;
                        }
                        if (startSaving)
                        {                            
                            if (++counter %6 == 0)
                            {
                                sb.Append(trim);
                                sb.AppendLine();
                            }
                            else
                            {
                                sb.Append(trim);
                                sb.Append("|");
                            }
                        }
                        if (startSaving2)
                        {
                            if (++counter % 7 == 0)
                            {
                                sb.Append(trim);
                                sb.AppendLine();
                            }
                            else
                            {
                                sb.Append(trim);
                                sb.Append("|");
                            }
                        }
                    }                
                }
                var rawData = sb.ToString();
                //Debug.WriteLine(rawData);

                rawData = rawData.Replace(",", ".");
                var rawDataLines = rawData.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                IList<MyClass> myTempClassList = new List<MyClass>(rawDataLines.Count());
                foreach (var rawDataLine in rawDataLines)
                {
                    MyClass tempObj = new MyClass();

                    var dataStrings = rawDataLine.Split('|');
                    
                    tempObj.Id = Int32.Parse(dataStrings[0]);
                    tempObj.OperDate = DateTime.Parse(dataStrings[1]);
                    tempObj.AccntDate = DateTime.Parse(dataStrings[2]);
                    tempObj.OperKindDesc = dataStrings[3];
                    if (dataStrings.Count() > 6)
                    {
                        tempObj.OperDesc = dataStrings[4];
                        var split = dataStrings[5].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (split.Count() > 1)
                            tempObj.Amount = Decimal.Parse(split[1]) * -1;
                        tempObj.Amount2 = Decimal.Parse(dataStrings[6].Replace("-", string.Empty)) * -1;

                    }
                    else
                    {
                        var split = dataStrings[4].Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                        if (split.Any())
                            tempObj.Amount = Decimal.Parse(split[0]);
                        tempObj.Amount2 = Decimal.Parse(dataStrings[5]);
                    }
                    myTempClassList.Add(tempObj);
                }

                foreach (var myClass in myTempClassList)
                {
                    Debug.WriteLine(myClass.ToString());
                }
            }
        }
    }
}
