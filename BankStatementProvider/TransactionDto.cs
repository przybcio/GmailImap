using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankStatementProvider
{
    public class TransactionDto
    {
        public  int Id { get; set; }
        public  int IdOnBankStatement { get; set; }
        public  DateTime OperDate { get; set; }
        public  DateTime AccntDate { get; set; }
        public  string OperKindDesc { get; set; }
        public  string OperDesc { get; set; }
        public  decimal Amount { get; set; }
        public  decimal Amount2 { get; set; }
        public  long BankStatementNo { get; set; }
    }
}
