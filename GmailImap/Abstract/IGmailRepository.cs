using System.Collections.Generic;
using System.IO;

namespace GmailImap.Abstract
{
    public interface IGmailRepository
    {
        IEnumerable<IMailBoxMessage> GetMessages(string query);        
    }
}
