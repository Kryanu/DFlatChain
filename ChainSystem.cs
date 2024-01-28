using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace DFlatChain
{
    public static class ChainSystem
    {
        private static readonly String current = Directory.GetCurrentDirectory();
        public static void StartUp()
        {
            if (!Directory.Exists($"{current}\\Storage"))
            {
                Directory.CreateDirectory($"{current}\\Storage");
            }
            if(File.Exists(GetMasterPath())){
                File.Create(GetMasterPath());    
            }
        }

        public static string GetMasterPath(){
            return $"{current}\\Storage\\master.dat";
        }

        public static Block MineBlock(ref Block current, ref Blockchain master)
        {
            current.SetHash();
            string prev = "";
            if (current.Hash != null)
            {
                prev = current.Hash;
            }
            master.AppendChain(current);
            Block mined = current;
            current = new(prev);
            return mined;
        }

        
    }
}
