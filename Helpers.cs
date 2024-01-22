using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace DFlatChain
{
    public static class Helpers
    {
        public static String ByteToString(Byte[] input)
        {
            return Encoding.ASCII.GetString(input);
        }

        public static Byte[] StringToByte(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }

        public static string GetHash(byte[] input)
        {
            Byte[] hashBytes;
            string test = ByteToString(input);
            using (SHA256 algorithm = SHA256.Create())
            {
                hashBytes = algorithm.ComputeHash(input);
            }
            return ByteArrayToString(hashBytes);
        }

        private static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public static string SerializeArray(List<Transaction> transactions)
        {
            //Guard Clause
            if(transactions.All<Transaction>(trans => trans == null))
            {
                return "";
            }

            StringBuilder stringBuilder = new();
            foreach (Transaction item in transactions)
            {
                stringBuilder.Append(item.ToMinimalString());
                stringBuilder.Append(',');
            }
            stringBuilder.Length -= 2; // Remove the trailing comma and space
            return stringBuilder.ToString();
        }
    }
}
