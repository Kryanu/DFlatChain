using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Transactions;
using System.Xml;
using DFlatChain;

namespace DFlatChain {
    public class Block {
        private readonly long MaxLength = 98304;
        public string PreviousHash { get; set; }
        public string TimeStamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string? Hash { get; set; }

        public int Nonce { get; set; }

        public Block(string previousHash) {
            Random random = new();
            PreviousHash = previousHash;
            TimeStamp = DateTime.Now.ToString("MM/dd/yyyy H:mm");
            Transactions = new List<Transaction>();
            Nonce = random.Next(100);
        }

        public void SetHash() {
            Hash = Helpers.GetHash(ToMinimalString(), true);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine($"PreviousHash: {PreviousHash}");
            stringBuilder.AppendLine($"TimeStamp: {TimeStamp}");
            stringBuilder.AppendLine("Transactions:");

            foreach (var transaction in Transactions) {
                stringBuilder.AppendLine($"  {transaction}");
                stringBuilder.AppendLine("--------------");
            }

            stringBuilder.AppendLine($"Hash: {Hash}");
            stringBuilder.AppendLine($"Nonce: {Nonce}");

            return stringBuilder.ToString();
        }

        private string ToMinimalString() {
            return $"{PreviousHash}{TimeStamp}{Nonce}{Helpers.SerializeArray(Transactions, false)}";
        }
        /*
            PreviousHash 64:Hash Length
            Hash 64: Hash Length
            TimeStamp 15: Time Length
            Nonce: 3: 100.length == 3: true
            Transactions: 91,136: 178 * 512
            Total 91282 -> Block Length
            Total Space allocated: 98,304 = (2^17 - 2^15)
        */
        public string PrepareBlockFormat() {
            //Block is being serialised and padded to fit the predetermined block size
            return $"{PreviousHash}#{TimeStamp}`{Nonce}%{Hash}[{Helpers.SerializeArray(Transactions, true)}]".PadRight((int)MaxLength);
        }

    }
}
