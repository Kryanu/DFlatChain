using System.Text;

namespace DFlatChain
{
    public class Transaction
    {
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal Amount { get; set;  }
        public string Description { get; set; }
        public string Hash { get; set; }

        public Transaction(string senderAddress, string receiverAddress, decimal amount, string description)
        {
            SenderAddress = Helpers.GetHash(senderAddress, false);
            ReceiverAddress = Helpers.GetHash(receiverAddress, false);
            Amount = amount;
            Description = description;
            Hash = Helpers.GetHash(ToMinimalString(),true);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"SenderAddress: {SenderAddress}");
            stringBuilder.AppendLine($"ReceiverAddress: {ReceiverAddress}");
            stringBuilder.AppendLine($"Amount: {Amount}");
            stringBuilder.AppendLine($"Description: {Description}");
            stringBuilder.AppendLine($"Hash: {Hash}");

            return stringBuilder.ToString();
        }

        public string ToMinimalString()
        {
            return $"{SenderAddress}{ReceiverAddress}{Amount}{Description}{Hash}";
        }
        /* 
            Length: 4: seperator char
            Amount 10: 9999999999 <- Max Value for amount
            Description 20: Keep it short
            Hash 64: Hash Length
            Sender & Receiver 80: Hash Length
            Total 178
        */
        public string ToFormattedString() {
            return  $"{SenderAddress}-{ReceiverAddress}-{Amount}-{Description}-{Hash}";   
        }
    }
}
