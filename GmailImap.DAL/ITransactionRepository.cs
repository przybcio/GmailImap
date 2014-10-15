using System.Collections.Generic;

namespace GmailImap.DAL
{
    public interface ITransactionRepository
    {
        ICollection<Transaction> GetAllTransactions();
        Transaction GetTransaction(long id);
        void AddTransaction(Transaction transaction);
        void AddTransactions(ICollection<Transaction> transactions);
    }
}