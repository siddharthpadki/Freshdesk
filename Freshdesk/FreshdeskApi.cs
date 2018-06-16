using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Nito.AsyncEx;
using Newtonsoft.Json.Linq;

namespace Freshdesk
{
    class FreshdeskApi
    {
        private HttpClient httpClient;

        public FreshdeskApi()
        {
            httpClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpClient.BaseAddress = new Uri("https:///");
            httpClient.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var byteArray = Encoding.ASCII.GetBytes("ysJkz1KFQjlUQkFHfERJ:X");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            Console.WriteLine("Created FreshdeskApi.");
        }

        public async Task<string> createCategory(string name, string description)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "solutions/categories");
            var body = "{\"name\":\"" + name + "\",\"description\":\"" + description + "\"}";
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(data);
            var categoryId = jObject["id"].ToString();

            Console.WriteLine(string.Format("Created Category {0}. Name: {1}; Description: {2}", categoryId, name, description));
            return categoryId;
        }

        public async Task<string> createFolder(string categoryId, string name, string description, int visibility)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, string.Format("solutions/categories/{0}/folders", categoryId));
            var body = "{\"name\":\"" + name + "\",\"description\":\"" + description + "\",\"visibility\":" + visibility + "}";
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(data);
            var folderId = jObject["id"].ToString();

            Console.WriteLine(string.Format("Created Folder {0}. Name: {1}; Description: {2}; Visibility: {3}", folderId, name, description, visibility));
            return folderId;
        }
        public async Task<string> createArticle(string folderId, string title, string description, int type, int status)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, string.Format("solutions/folders/{0}/articles", folderId));
            var body = "{\"title\":\"" + title + "\",\"description\":\"" + description + "\",\"type\":" + type + ",\"status\":" + status + "}";
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(data);
            var articleId = jObject["id"].ToString();

            Console.WriteLine(string.Format("Created Article {0}. Title: {1}; Description: {2}; Type: {3}; Status: {4}", articleId, title, description, type, status));
            return articleId;
        }

        public async Task<List<string>> getCategories()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "solutions/categories");
            var response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            var jToken = JToken.Parse(data);
            Console.WriteLine("Got Categories: " + jToken.ToString());
            var categoryIds = new List<string>();
            foreach (JObject categoryJObject in jToken.Children())
            {
                var categoryId = categoryJObject["id"].ToString();
                categoryIds.Add(categoryId);
            }
            return categoryIds;
        }

        public async Task deleteCategory(string categoryId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, string.Format("solutions/categories/{0}", categoryId));
            Console.WriteLine("Deleted Category " + categoryId);
            await httpClient.SendAsync(request);
        }

        public async Task deleteAllCategories()
        {
            var categoryIds = await getCategories();
            var deleteTasks = new List<Task>();
            foreach (string categoryId in categoryIds)
            {
                deleteTasks.Add(deleteCategory(categoryId));
            }
            foreach (Task deleteTask in deleteTasks)
            {
                await deleteTask;
            }
        }
    }
}
