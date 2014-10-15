using System.Collections.Generic;

namespace GmailImap.Abstract
{
    public interface IGmailRepository
    {
        IEnumerable<IMailBoxMessage> GetMessages(string query);
    }
}
