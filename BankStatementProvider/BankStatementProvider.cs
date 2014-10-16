using System.Collections;
using AutoMapper;
using GmailImap.Abstract;
using GmailImap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmailImap.DAL.Model;

namespace BankStatementProvider
{
    public class BankStatementProvider
    {
        private readonly IGmailRepository _gmailRepository;
        private readonly MailBoxToBankStatementWorker _worker;

        private readonly ITransactionRepository _transactionRepository;

        public BankStatementProvider(IGmailRepository gmailRepository, ITransactionRepository transactionRepository)
        {
            _gmailRepository = gmailRepository;
            _transactionRepository = transactionRepository;
            _worker = new MailBoxToBankStatementWorker();
        }

        public bool SyncBankStatements(string query)
        {
            //const string query = "2014-09-16";
            var mailBoxMessages = _gmailRepository.GetMessages(query);
            Mapper.CreateMap<TransactionDto, Transaction>();            
            IEnumerable<TransactionDto> processMessages = _worker.ProcessMessages(mailBoxMessages);
            bool isResult = false;
            foreach (var tran in processMessages.Select(Mapper.Map<Transaction>))
            {
                isResult = true;
                _transactionRepository.AddTransaction(tran);
            }

            return isResult;
        }

        public IEnumerable<TransactionDto> GetTransactinsForAllBankStatements()
        {
            Mapper.CreateMap<Transaction, TransactionDto>();            
            ICollection<Transaction> allTransactions = _transactionRepository.GetAllTransactions();
            return allTransactions.Select(Mapper.Map<TransactionDto>);
        }
    }
}
