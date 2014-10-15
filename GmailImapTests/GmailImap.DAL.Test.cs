using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmailImap.DAL;
using NUnit.Framework;

namespace GmailImapTests
{
    [TestFixture]
    public class GmailImapDALTest
    {
        [Test]
        public void T1()
        {
            var sessionFactory = FluentNhConfiguration.CreateSessionFactory();

            using (var openSession = sessionFactory.OpenSession())
            {

            }
        }
    }
}
