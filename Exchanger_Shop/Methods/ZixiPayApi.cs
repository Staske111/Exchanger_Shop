using Exchanger_Shop.Extensions;
using Exchanger_Shop.Models;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Exchanger_Shop.Methods
{
    public class ZixiPayApi
    {
        private readonly string url;
        public ZixiPayApi()
        {
            url = "https://api.zixipay.com/apiv2/";
        }
        public async Task<PayClass> CallMethod(string endpoint, NameValueCollection nvc, string apiKey)
        {
            string _url = url + endpoint;
            
            string sig = GeneralMethods.CalcHMACSHA256Hash(nvc.ToQueryString(), apiKey);
            nvc.Add("sig", sig);
            string response = await GeneralMethods.PostRequestAsync(url+endpoint, nvc.ToQueryString());
            return await Task.Run(() => GeneralMethods.JsonToPayObject(response));

        }
    }
}
