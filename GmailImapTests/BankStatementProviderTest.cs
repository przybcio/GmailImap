using System.Collections.Generic;
using BankStatementProvider;
using GmailImap.Abstract;
using GmailImap.Implementation;
using NUnit.Framework;

namespace GmailImapTests
{
    [TestFixture]
    public class BankStatementProviderTest
    {
        [Test]
        public void Should_return_not_empty_list()
        {
            IGmailRepository gmailRepository = new GmailFactoryRepository().GetRepository(GmailRepositoryType.MimeKit);

            BankStatementProvider.BankStatementProvider bsp = new BankStatementProvider.BankStatementProvider(gmailRepository);

            IEnumerable<TransactionDto> transactinsForAllBankStatements = bsp.GetTransactinsForAllBankStatements();
            Assert.NotNull(transactinsForAllBankStatements);
        }
    }
}