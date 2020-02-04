using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    internal static class RandomNumberGenerator
    {
        public static string CreateUniqueId(int length = 64)
        {
            var bytes = PCLCrypto.WinRTCrypto.CryptographicBuffer.GenerateRandom(length);
            return ByteArrayToString(bytes);
        }

        private static string ByteArrayToString(IReadOnlyCollection<byte> array)
        {
            var hex = new StringBuilder(array.Count * 2);
            foreach (var b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}