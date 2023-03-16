using RestSharp;
using RestSharp.Authenticators;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RestSharpDemoProject
{
    internal class Program
    {


        static void Main(string[] args)
        {

            RestClient client = new RestClient("https://api.github.com");
            client.Authenticator = new HttpBasicAuthenticator("botevad", "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");

            //RestRequest request = new RestRequest("/repos/{user}/{repoName}/issues/{id}/labels", Method.Get);
            RestRequest request = new RestRequest("/repos/{user}/{repoName}/issues", Method.Post);

            var issueBody = new
            {
                title = "Test issue from RestSharp " + DateTime.Now.Ticks,
                body = "some body for my issue",
                labels = new string[] {"bug", "critical", "release"}
            };

            request.AddJsonBody(issueBody);

            request.AddUrlSegment("user", "botevad");
            request.AddUrlSegment("repoName", "postman");
            //request.AddUrlSegment("id", "1");

            var response = client.Execute(request);

            //var issue = new SystemTextJsonSerializer().Deserialize<Issue>(response);
            //var issue = JsonConvert.DeserializeObject<Issue>(response.Content);
            //var issues = JsonSerializer.Deserialize < List <Issue> > (response.Content);
            /*            foreach (var issue in issues )
            {
                Console.WriteLine("Issue name: " + issue.title);
                Console.WriteLine("Issue number: " + issue.number);
            }*/

            //var labels = JsonSerializer.Deserialize < List <Label> > (response.Content);
            var issue = JsonSerializer.Deserialize < Issue > (response.Content);

            Console.WriteLine("Issue name: " + issue.title);
            Console.WriteLine("Issue number: " + issue.number);

            /*            foreach (var label in labels)
                        {
                            Console.WriteLine("Label name: " + label.name);
                            Console.WriteLine("Label id: " + label.id);
                            Console.WriteLine();
                        }*/



            Console.WriteLine("Status code: " + response.StatusCode);
            //Console.WriteLine("Response: " + response.Content);
        }
    }
}