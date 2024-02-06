using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;
using System.Collections;

namespace DFlatChain {
    public static class Helpers {
        public static String ByteToString(Byte[] input) {
            return Encoding.ASCII.GetString(input);
        }

        public static Byte[] StringToByte(string input) {
            return Encoding.ASCII.GetBytes(input);
        }

        public static string GetHash(string input, bool Is256) {
            Byte[] hashBytes;
            if (Is256) {
                using SHA256 algorithm = SHA256.Create();
                hashBytes = algorithm.ComputeHash(StringToByte(input));
            } else {
                using SHA1 algorithm = SHA1.Create();
                hashBytes = algorithm.ComputeHash(StringToByte(input));
            }

            return ByteArrayToString(hashBytes);
        }

        private static string ByteArrayToString(byte[] ba) {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public static string SerializeArray(List<Transaction> transactions, bool IsFormatted) {
            //Guard Clause
            if (transactions.All<Transaction>(trans => trans == null)) {
                return "";
            }

            StringBuilder stringBuilder = new();
            foreach (Transaction item in transactions) {
                if (IsFormatted) {
                    stringBuilder.Append(item.ToFormattedString());
                } else {
                    stringBuilder.Append(item.ToMinimalString());
                }
                //Used for split when formatting for saving
                stringBuilder.Append(',');
            }
            stringBuilder.Length -= 2; // Remove the trailing comma and space
            return stringBuilder.ToString();
        }

        public static Byte[] Compress(string data){
            return CompressData(StringToByte(data));
        } 

        public static string Decompress(byte[] data){
            return ByteToString(DecompressData(data));
        }

        private static Byte[] CompressData(byte[] data) {
            Span<byte> buffer = new Span<byte>(data);

            using (MemoryStream memoryStream = new()) {
                using (BrotliStream compress = new(memoryStream, CompressionMode.Compress)) {
                    compress.Write(data);
                };
                return memoryStream.ToArray();
            };
        }

        private static byte[] DecompressData(byte[] compressedData) {
            using (MemoryStream memoryStream = new(compressedData)) {
                using (MemoryStream decompressedStream = new ()) {
                    using (BrotliStream decompress = new(memoryStream, CompressionMode.Decompress)) {
                        decompress.CopyTo(decompressedStream);
                    }
                    return decompressedStream.ToArray();
                }
            }
        }
    }
}
