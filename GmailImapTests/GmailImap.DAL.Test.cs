using System;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GmailImap.DAL;
using NHibernate;
using NUnit.Framework;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Facilities;

namespace GmailImapTests
{
    
    [TestFixture]
    public class GmailImapDALTest
    {
        private ITransactionRepository _transactionRepository;

        [SetUp]
        public void Create_sessionFactory_setup()
        {            
            var container = new WindsorContainer();
            container.Install(new FluentNhConfiguration());
            _transactionRepository = container.Resolve<ITransactionRepository>();
        }

        [Test]
        public void Should_add_transaction()
        {
            long id = 0;
            DateTime accntDate = DateTime.Now;

            Transaction tran = new Transaction
                {
                    AccntDate = accntDate,
                    Amount = 213.21M,
                    Amount2 = 123.12M,
                    OperDate = DateTime.Now.AddDays(1),
                    OperDesc = "Desc",
                    OperKindDesc = "Oper kind desc"
                };

            id = _transactionRepository.AddTransaction(tran);


            Transaction transaction = null;

            transaction = _transactionRepository.GetTransaction(id);

            Assert.NotNull(transaction);
            TimeSpan ts = transaction.AccntDate - accntDate;
            Assert.True(ts.Minutes == 0, "Niezgodne daty księgowania");
        }
    }
}