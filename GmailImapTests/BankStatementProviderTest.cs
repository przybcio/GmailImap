using System.Collections.Generic;
using BankStatementProvider;
using Castle.Windsor;
using GmailImap.Abstract;
using GmailImap.Implementation;
using NUnit.Framework;
using GmailImap.DAL;

namespace GmailImapTests
{
    [TestFixture]
    public class BankStatementProviderTest
    {
        private BankStatementProvider.BankStatementProvider _bsp;
        private IGmailRepository _gmailRepository;
        private ITransactionRepository _transactionRepository;
        [SetUp]
        public void Start()
        {

            _gmailRepository = new GmailFactoryRepository().GetRepository(GmailRepositoryType.MimeKit);
            var container = new WindsorContainer();
            container.Install(new FluentNhConfiguration());
            _transactionRepository = container.Resolve<ITransactionRepository>();
             
            _bsp = new BankStatementProvider.BankStatementProvider(_gmailRepository, _transactionRepository);
        }

        [Test]
        public void Should_return_not_empty_list()
        {
            IEnumerable<TransactionDto> transactinsForAllBankStatements = _bsp.GetTransactinsForAllBankStatements();
            Assert.NotNull(transactinsForAllBankStatements);
        }

        [Test]
        public void Sync_should_return_true()
        {
            bool syncBankStatements = _bsp.SyncBankStatements("2014-09-16");
            Assert.IsTrue(syncBankStatements, "Nie zsynchronizowano wyciągów.");
        }
    }

}