using GmailImap.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankStatementProvider
{
    public class BankStatementProvider
    {
        private readonly IGmailRepository _gmailRepository;
        private readonly MailBoxToBankStatementWorker _worker;

        public BankStatementProvider(IGmailRepository gmailRepository)
        {
            _gmailRepository = gmailRepository;
            _worker = new MailBoxToBankStatementWorker();
        }

        public IEnumerable<TransactionDto> GetTransactinsForAllBankStatements()
        {
            const string query = "2014-09-16";
            var mailBoxMessages = _gmailRepository.GetMessages(query);
            return  _worker.ProcessMessages(mailBoxMessages);
        }
    }
}
