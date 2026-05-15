using System.Security.Cryptography;
using System.Text;

namespace Mentora.Application.Helpers
{
    public static class ShareLinkHelper
    {
        private static readonly string Key = "MentoraSecretKey1234567890123456"; // 32 chars
                                                                                 // ضيفي IV ثابت (لازم يكون 16 حرف)
        private static readonly string FixedIV = "MentoraFixedIV12";

        public static string EncryptProgramId(int programId)
        {
            var keyBytes = Encoding.UTF8.GetBytes(Key);
            var ivBytes = Encoding.UTF8.GetBytes(FixedIV);
            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(programId.ToString());
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(cipherBytes)
                .Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public static int DecryptProgramId(string encryptedToken)
        {
            var base64 = encryptedToken
                .Replace("-", "+")
                .Replace("_", "/");
            var padNeeded = base64.Length % 4;
            if (padNeeded > 0) base64 += new string('=', 4 - padNeeded);
            var cipherBytes = Convert.FromBase64String(base64);
            var keyBytes = Encoding.UTF8.GetBytes(Key);
            var ivBytes = Encoding.UTF8.GetBytes(FixedIV); 
            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes; 
            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return int.Parse(Encoding.UTF8.GetString(plainBytes));
        }
    }
}