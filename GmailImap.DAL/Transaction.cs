using System;

namespace GmailImap.DAL
{
    public class Transaction
    {
        public virtual int Id { get; set; }
        public virtual DateTime OperDate { get; set; }
        public virtual DateTime AccntDate { get; set; }
        public virtual string OperKindDesc { get; set; }
        public virtual string OperDesc { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal Amount2 { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Id: {0}, OperDate: {1}, AccntDate: {2}, OperKindDesc: {3}, OperDesc: {4}, Amount: {5}, Amount2: {6}",
                    Id, OperDate, AccntDate, OperKindDesc, OperDesc, Amount, Amount2);
        }
    }
}