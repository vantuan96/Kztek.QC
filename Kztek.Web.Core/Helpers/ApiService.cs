using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Helpers
{
    public class ApiService<T> where T : class
    {
        public static List<T> GetList(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObjectAsync<List<T>>(response.Result).Result;
            }
        }

        public static T GetObj(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObjectAsync<T>(response.Result).Result;
            }
        }

        //public static T PutObj(string uri)
        //{
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        Task<String> response = httpClient.PutAsJsonAsync(uri);
        //        return JsonConvert.DeserializeObjectAsync<T>(response.Result).Result;
        //    }
        //}
    }
}
