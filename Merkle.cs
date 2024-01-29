namespace DFlatChain {
    public class Merkle {
        public Merkle? left { get; set; }
        public Merkle? right { get; set; }
        public string hash { get; set; }

        public Merkle(Merkle? left, Merkle? right) {
            this.left = left;
            this.right = right;
            this.hash = Helpers.GetHash($"{left.hash}{right.hash}", false);
        }

        public Merkle(string hash) {
            this.hash = hash;
        }

        public static Merkle GenerateTree(Transaction[] transactions) {
            return GenerateRoot(CalculateHash(transactions));
        }

        private static Merkle[] CalculateHash(Transaction[] transactions) {
            if (transactions.Length % 2 != 0) {
                throw new Exception("Transaction Length Error");
            }

            Merkle[] result = new Merkle[transactions.Length];
            for (int i = 0; i < transactions.Length; i++) {
                result[i] = new(transactions[i].Hash);
            }

            return result;
        }

        private static Merkle GenerateRoot(Merkle[] nodes) {
            if (nodes.Length == 2) {
                return new Merkle(nodes[0], nodes[1]);
            }
            Merkle[] tempNodes = new Merkle[nodes.Length / 2];
            for (int i = 0; i < nodes.Length; ) {
                tempNodes[i/2] = new(nodes[i], nodes[i + 1]);
                i += 2;
            }

            return GenerateRoot(tempNodes);
        }
    }
}
