﻿using System.Net;
using System.Net.Http;
using System.Text;
using TaskManager.Client.Models;

namespace TaskManager.Client.Services
{
    public abstract class CommonRequestService
    {
        public const string HOST = "http://localhost:5166/api/";

        protected string GetDataByUrl(HttpMethod method, string url, AuthToken token,
            string userName = null, string password = null, Dictionary<string, string> parametrs = null)
        {
            WebClient client = new WebClient();

            if (userName != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
                client.Headers.Add("Authorization", "Basic " + encoded);
            }
            else if (token != null)
            {
                client.Headers.Add("Authorization", "Bearer  " + token.access_token);
            }

            if (parametrs != null)
                foreach (var key in parametrs.Keys)
                    client.QueryString.Add(key, parametrs[key]);

            byte[] data = Array.Empty<byte>();
            try
            {
            if (method == HttpMethod.Post)
                data = client.UploadValues(url, method.Method, client.QueryString);

            if (method == HttpMethod.Get)
                data = client.DownloadData(url);
            }
            catch { }

            string result1 = UnicodeEncoding.UTF8.GetString(data);
            return result1;

        }

        protected HttpStatusCode SendDataByUrl(HttpMethod method, string url, AuthToken token, string data = null)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            StringContent content;

            content = new StringContent(data, Encoding.UTF8, "application/json");

            if (method == HttpMethod.Post)
                result = client.PostAsync(url, content).Result;

            if (method == HttpMethod.Patch)
                result = client.PatchAsync(url, content).Result;


            return result.StatusCode;
        }

        protected HttpStatusCode DeleteDataByUrl(string url, AuthToken token)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);

            result = client.DeleteAsync(url).Result;

            return result.StatusCode;
        }
    }
}
