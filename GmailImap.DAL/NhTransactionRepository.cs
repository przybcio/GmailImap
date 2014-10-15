﻿using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace GmailImap.DAL
{
    public class NhTransactionRepository : ITransactionRepository
    {
        public ICollection<Transaction> GetAllTransactions()
        {
            throw new NotImplementedException();
        }

        public Transaction GetTransaction(long id)
        {
            return (from t in _session.Query<Transaction>() where t.Id == id select t).SingleOrDefault();
        }

        public long AddTransaction(Transaction transaction)
        {
            var savedId = _session.Save(transaction);            
            try
            {
                  return Convert.ToInt64(savedId);
            }
            catch (Exception ex )
            {
                //log error
                return -1;
            }
             

        }

        public void AddTransactions(ICollection<Transaction> transactions)
        {
            throw new NotImplementedException();
        }

        private readonly ISession _session;

        public NhTransactionRepository(ISession session)
        {
            _session = session;
        }
    }
}
