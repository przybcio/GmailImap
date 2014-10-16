using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GmailImap.Abstract;

namespace BankStatementProvider
{
    public class MailBoxToBankStatementWorker
    {       
        public IEnumerable<TransactionDto> ProcessMessages(IEnumerable<IMailBoxMessage> mailBoxMessages)
        {
            HashSet<string> createdFiles = new HashSet<string>();

            foreach (var mailBoxMessage in mailBoxMessages)
            {
                string fileName;
                using (Stream stream = mailBoxMessage.DecodeAttachementFromMessage(out fileName))
                {
                    //zapis pliku      
                    if (stream != null)
                    {
                        stream.Flush();
                        createdFiles.Add(fileName);
                    }
                }
            }

            List<TransactionDto> myTempClassList = new List<TransactionDto>();
            foreach (var createdFile in createdFiles.Where(p=> p.EndsWith("html")))
            {
                myTempClassList.AddRange(ProcessAttachment(createdFile));
            }
            return myTempClassList;
        }

        private IEnumerable<TransactionDto> ProcessAttachment(string createdFile)
        {
            using (var fileStream = new StreamReader(createdFile, Encoding.GetEncoding("ISO-8859-2")))
            using (var reader = XmlReader.Create(fileStream))
            {
                var rawData = GetRawData(reader);
                rawData = rawData.Replace(",", ".");
                var rawDataLines = rawData.Split(new string[] { Environment.NewLine },
                                                 StringSplitOptions.RemoveEmptyEntries);
                var myTempClassList = new List<TransactionDto>(rawDataLines.Count());
                foreach (var rawDataLine in rawDataLines)
                {
                    GetTransationDto(rawDataLine, myTempClassList);
                }
                return myTempClassList;
            }
        }

        private static void GetTransationDto(string rawDataLine, IList<TransactionDto> myTempClassList)
        {
            TransactionDto tempObj = new TransactionDto();

            var dataStrings = rawDataLine.Split('|');

            tempObj.Id = Int32.Parse(dataStrings[0]);
            tempObj.OperDate = DateTime.Parse(dataStrings[1]);
            tempObj.AccntDate = DateTime.Parse(dataStrings[2]);
            tempObj.OperKindDesc = dataStrings[3];
            if (dataStrings.Count() > 6)
            {
                tempObj.OperDesc = dataStrings[4];
                var split = dataStrings[5].Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                if (split.Count() > 1)
                    tempObj.Amount = Decimal.Parse(split[1])*-1;
                tempObj.Amount2 = Decimal.Parse(dataStrings[6].Replace("-", string.Empty))*-1;
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

        private static string GetRawData(XmlReader reader)
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
                        if (++counter%6 == 0)
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
                        if (++counter%7 == 0)
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
            return rawData;
        }
    }
}
