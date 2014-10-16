using System.Collections.Generic;
using GmailImap.DAL.Model;
using NHibernate;

namespace GmailImap.DAL
{
    public interface ITransactionRepository
    {
        ICollection<Transaction> GetAllTransactions();
        Transaction GetTransaction(long id);
        long AddTransaction(Transaction transaction);
        void AddTransactions(ICollection<Transaction> transactions);       
    }
}