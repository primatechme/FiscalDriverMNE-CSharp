using Newtonsoft.Json;
using Primatech.FiscalDriver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Helpers
{
    public static class HttpClientHelper
    {

        public static ExtendedHttpClient WithClient(this string path, ExtendedHttpClient client)
        {
            client.BaseUrl = path;
            client.RequestUri = path;

            return client;
        }


        public static ExtendedHttpClient AppendPathSegment(this string path)
        {
            return new ExtendedHttpClient(path);
        }

        public static ExtendedHttpClient AppendPathSegment(this string path, string uri)
        {
            return AppendPathSegment(path)
                .AppendPathSegment(uri);
        }


        public static ExtendedHttpClient AppendPathSegment(this ExtendedHttpClient client, string uri)
        {
            client.RequestUri += "/" + uri;
            client.RequestUri.Replace("//", "/");
            return client;
        }

        public static ExtendedHttpClient WithHeader(this string path)
        {
            return new ExtendedHttpClient(path);

        }

        public static HttpClient WithHeader(this HttpClient client, string key, string value)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
            client.DefaultRequestHeaders.Add(key, value);
            return client;
        }

        public static HttpClient SetHeaderAccept(this HttpClient client, string value)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(value));
            return client;

        }

        public static HttpClient AcceptXml(this HttpClient client)
        {
            return client.SetHeaderAccept("application/json");
        }

        public static HttpClient AcceptJson(this HttpClient client)
        {
            return client.SetHeaderAccept("application/xml");

        }

        //TODO: change, add to request message
        //Possible solution "application/json; charset=utf-8"
        public static HttpClient SetHeaderContentType(this HttpClient client, string value)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", value);
            return client;
        }

        public static HttpClient WithOAuthBearerToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public static ExtendedHttpClient UseUri(this HttpClient client)
        {
            if (client is ExtendedHttpClient)
                return (ExtendedHttpClient)client;
            throw new Exception("Unsupported cast from ExtendedHttpClient to HttpClient.");
        }

        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient client, string url, T model)// where T : class
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(url, data);
        }

        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this ExtendedHttpClient client, T model)// where T : class
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(client.RequestUri, data);
        }

        public static async Task<T> ReceiveJson<T>(this Task<HttpResponseMessage> response)
        {
            using (var resp = await response.ConfigureAwait(false))
            {
                resp.EnsureSuccessStatusCode();
                var responseString = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);

            }
        }

        public static async Task<T> ReceiveJson<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resp);
        }

        public static async Task<HttpResponseMessage> PostXmlAsync<T>(this HttpClient client, string url, T model)
        {
            var xml = model.XmlSerializeToString();
            var content = new StringContent(xml, Encoding.UTF8, "application/xml");
            return await client.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> PostXmlAsync<T>(this ExtendedHttpClient client, T model)
        {
            string xml = "";
            if (!(model is String) && !(model is string))
            {
                xml = model.XmlSerializeToString();
            }
            else
            {
                xml = model as string;
            }

            var content = new StringContent(xml, Encoding.UTF8, "application/xml");
            return await client.PostAsync(client.RequestUri, content);
        }

        public static async Task<T> ReceiveXml<T>(this Task<HttpResponseMessage> response)
        {
            using (var resp = await response.ConfigureAwait(false))
            {
                resp.EnsureSuccessStatusCode();
                var responseBody = await resp.Content.ReadAsStringAsync();
                return responseBody.XmlDeserializeFromString<T>();
            }
        }

        public static async Task<T> ReceiveXml<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.XmlDeserializeFromString<T>();
        }

        public static async Task<T> PostJsonAsync<T>(this ExtendedHttpClient client, object model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(client.RequestUri, data);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resp);
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient client, string url, object model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resp);
        }

        public static async Task<T> PostXmlAsync<T>(this HttpClient client, string url, object model)
        {
            var xml = model.XmlSerializeToString();
            var content = new StringContent(xml, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.XmlDeserializeFromString<T>();
        }

        public static async Task<T> PostXmlAsync<T>(this ExtendedHttpClient client, object model)
        {
            var xml = model.XmlSerializeToString();
            var content = new StringContent(xml, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(client.RequestUri, content);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.XmlDeserializeFromString<T>();
        }


    }
}
