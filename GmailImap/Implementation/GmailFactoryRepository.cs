using GmailImap.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailImap.Implementation
{
    public class GmailFactoryRepository
    {
        public IGmailRepository GetRepository(GmailRepositoryType type)
        {
            switch (type)
            {
                case GmailRepositoryType.MimeKit:
                    return new GmailKitRepository();
                case GmailRepositoryType.ActiveUp:
                    return new GmailActiveUpRepository();
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }

    public enum GmailRepositoryType
    {
        MimeKit, 
        ActiveUp
    }
}
