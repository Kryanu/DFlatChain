using System.Text;

namespace DFlatChain
{
    public class Transaction
    {
        public DateTime Time { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal Amount { get; set;  }
        public string Description { get; set; }
        public string Hash { get; set; }

        public Transaction(string senderAddress, string receiverAddress, decimal amount, string description)
        {
            Time = DateTime.Now;
            SenderAddress = senderAddress;
            ReceiverAddress = receiverAddress;
            Amount = amount;
            Description = description;
            Hash = Helpers.GetHash(Serialize(this));
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Time: {Time}");
            stringBuilder.AppendLine($"SenderAddress: {SenderAddress}");
            stringBuilder.AppendLine($"ReceiverAddress: {ReceiverAddress}");
            stringBuilder.AppendLine($"Amount: {Amount}");
            stringBuilder.AppendLine($"Description: {Description}");
            stringBuilder.AppendLine($"Hash: {Hash}");

            return stringBuilder.ToString();
        }

        public string ToMinimalString()
        {
            return $"{Time}{SenderAddress}{ReceiverAddress}{Amount}{Description}{Hash}";
        }

        private static Byte[] Serialize(Transaction transaction)
        {
            return Helpers.StringToByte(transaction.ToMinimalString());
        }
    }
}
