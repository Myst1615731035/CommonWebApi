using Google.Authenticator;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utils
{
    public class TotpAuth
    {
        private static readonly DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan DefaultClock = TimeSpan.FromSeconds(30);
        private static readonly string issuer = "PSR-IT";
        private static readonly int QRPixelsPerModule = 5;

        public static SetupCode Create(string account, string secret)
        {
            if(account.IsEmpty() || secret.IsEmpty())
            {
                throw new Exception("Parameter error");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(secret);
            account = new string(account.Where(c => !Char.IsWhiteSpace(c)).ToArray());

            string encodedSecret = Base32Encoding.ToString(bytes);
            string url = $"otpauth://totp/{UrlHelper.UrlEncode(issuer)}:{account}?secret={encodedSecret.Replace("=", "")}&issuer={UrlHelper.UrlEncode(issuer)}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(QRPixelsPerModule);
            return new SetupCode(account, encodedSecret, String.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCodeAsPngByteArr)));
        }

        public static bool VerifyCode(string secret,string code)
        {
            var localCode = GetCurrentPINs(secret, DefaultClock);

            return localCode.Any(c => c == code);
        }

        #region 校验方法的辅助方法
        private static string[] GetCurrentPINs(string secret, TimeSpan timeTolerance)
        {
            List<string> codes = new List<string>();
            long iterationCounter = (long)(DateTime.UtcNow - origin).TotalSeconds / 30;
            int iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30)
            {
                iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);
            }

            long iterationStart = iterationCounter - iterationOffset;
            long iterationEnd = iterationCounter + iterationOffset;

            byte[] key = Encoding.UTF8.GetBytes(secret);

            for (long counter = iterationStart; counter <= iterationEnd; counter++)
            {
                codes.Add(GenerateHashedCode(key, counter));
            }

            return codes.ToArray();
        }

        private static string GenerateHashedCode(byte[] key, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            HMACSHA1 hmac = new HMACSHA1(key);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            int binary =
              ((hash[offset] & 0x7f) << 24)
              | (hash[offset + 1] << 16)
              | (hash[offset + 2] << 8)
              | (hash[offset + 3]);

            int password = binary % (int)Math.Pow(10, digits);
            return password.ToString(new string('0', digits));
        }
        #endregion
    }
}
