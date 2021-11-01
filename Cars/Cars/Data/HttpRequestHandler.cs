using System;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace Cars.Data
{
    public class HttpRequestHandler
    {
        private const string UserName = "admin";
        private const string Password = "Bamboszki1";

        public static T GetResponse <T>(string url, Method method = Method.GET)
        {
            try
            {
                var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{UserName}:{Password}"));
                var client = new RestClient(url);
                var request = new RestRequest(method);
                request.AddHeader("Authorization", $"Basic {encoded}");
                var response = client.Execute(request);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception e)
            {
                return default;
            }
           
        }
    }
}