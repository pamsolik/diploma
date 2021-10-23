using System;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace Cars.Data
{
    public class HttpRequestHandler<T>
    {
        private const string UserName = "admin";
        private const string Password = "Bamboszki1";

        public T GetResponse(string url, Method method = Method.GET)
        {
            var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{UserName}:{Password}"));
            var client = new RestClient(url);
            var request = new RestRequest(method);
            request.AddHeader("Authorization", $"Basic {encoded}");
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}