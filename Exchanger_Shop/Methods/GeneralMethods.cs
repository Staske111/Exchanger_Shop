using Exchanger_Shop.Extensions;
using Exchanger_Shop.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exchanger_Shop.Methods
{
    public static class GeneralMethods
    {
        public static async Task<string> PostRequestAsync(string url, string data)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST"; 
            
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            
            request.ContentType = "application/x-www-form-urlencoded";
            
            request.ContentLength = byteArray.Length;

            
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await Task.Run(() => reader.ReadToEnd());
                }
            }
        }
        public static string CalcHMACSHA256Hash(string plaintext, string key)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(plaintext),
            baKey = enc.GetBytes(key);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baKey);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }
        public static PayClass JsonToPayObject(string data)
        {
            JObject jObject = JObject.Parse(data);
            return jObject.ToObject<PayClass>();
        }
    }
}
