namespace DFlatChain {
    public class Blockchain {
        private static readonly Blockchain instance = new();
        private List<Block> Chain;

        private Blockchain() {
            Chain = new List<Block>();
        }

        public static Blockchain GetBlockchain() {
            return instance;
        }

        public Boolean AppendChain(Block block) {
            try {
                Chain.Add(block);
                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public Block[] GetChain() {
            return this.Chain.ToArray();

        }

        public Boolean IsChainValid(Block[] chain, string hash, int counter) {
            if (counter == 0) {
                return true;
            } else {
                if (hash == chain[counter].Hash) {
                    return IsChainValid(chain, chain[counter].PreviousHash, counter - 1);
                } else {
                    return false;
                }
            }
        }
    }
}
