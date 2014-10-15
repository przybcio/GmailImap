using System;
using GmailImap.DAL;
using NHibernate;
using NUnit.Framework;

namespace GmailImapTests
{
    [TestFixture]
    public class GmailImapDALTest
    {
        private ISessionFactory _sessionFactory;

        [SetUp]
        public void Create_sessionFactory_setup()
        {
            _sessionFactory = FluentNhConfiguration.CreateSessionFactory();
        }

        [Test]
        public void Should_add_transaction()
        {            
            long id = 0;
            DateTime accntDate = DateTime.Now;
            using (var openSession = _sessionFactory.OpenSession())
            {
                ITransactionRepository repo = new NhTransactionRepository(openSession);
                
                Transaction tran = new Transaction
                    {
                        AccntDate = accntDate,
                        Amount = 213.21M,
                        Amount2 = 123.12M,
                        OperDate = DateTime.Now.AddDays(1),
                        OperDesc = "Desc",
                        OperKindDesc = "Oper kind desc"
                    };
                id = repo.AddTransaction(tran);
            }

            Transaction transaction = null;
            using (var openSession = _sessionFactory.OpenSession())
            {
                ITransactionRepository repo = new NhTransactionRepository(openSession);
                transaction = repo.GetTransaction(id);
            }
            Assert.NotNull(transaction);
            TimeSpan ts = transaction.AccntDate - accntDate;
            Assert.True(ts.Minutes == 0, "Niezgodne daty księgowania");
        }
    }
}