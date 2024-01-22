using System.Text;
using System.Transactions;
using System.Xml;
using DFlatChain;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace DFlatChain
{
    public class Block
    {
        public string PreviousHash { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string? Hash { get; set; }

        public int Nonce { get; set; }

        public Block(string previousHash)
        {
            Random random = new Random();
            PreviousHash = previousHash;
            TimeStamp = DateTime.Now;
            Transactions = new List<Transaction>();
            Nonce = random.Next(100);
        }

        public void SetHash()
        {
            Hash = Helpers.GetHash(Serialize(this));
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine($"PreviousHash: {PreviousHash}");
            stringBuilder.AppendLine($"TimeStamp: {TimeStamp}");
            stringBuilder.AppendLine("Transactions:");

            foreach (var transaction in Transactions)
            {
                stringBuilder.AppendLine($"  {transaction}");
                stringBuilder.AppendLine("--------------");
            }

            stringBuilder.AppendLine($"Hash: {Hash}");
            stringBuilder.AppendLine($"Nonce: {Nonce}");

            return stringBuilder.ToString();
        }

        private string ToMinimalString()
        {
            //JsonConvert.SerializeObject(Transactions, Formatting.Indented);
            return $"{PreviousHash}{TimeStamp}{Nonce}{Helpers.SerializeArray(Transactions)}";
        }

        private static Byte[] Serialize(Block block)
        {
            return Helpers.StringToByte(block.ToMinimalString());
        }
    }

    
}
