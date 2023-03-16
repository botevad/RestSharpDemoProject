

using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GitHubAPITests
{
    public class ApiTests
    {
        private RestClient client;
        private const string baseUrl = "https://api.github.com";
        private const string username = "botevad";
        private const string password = "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";

        [SetUp]
        public void Setup() {
            this.client = new RestClient(baseUrl);
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);
            
        }
        

        [Test]
        [Timeout(1000)]
        public void Test_GetSingleIssue()
        {
           
            var request = new RestRequest("/repos/botevad/postman/issues/2", Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            Assert.That(issue.title, Is.EqualTo("Second issue"));
            Assert.That(issue.number, Is.EqualTo(2));

        }

        [Test]
        public void Test_GetSingleIssueWithLabels()
        {

            var request = new RestRequest("/repos/botevad/postman/issues/1", Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            for (int i = 0; i < issue.labels.Count; i++) 
            {
                Assert.That(issue.labels[i].name, Is.Not.Null);
            }

        }

        [Test]
        public void Test_GetAllIssue()
        {
            
            var request = new RestRequest("/repos/botevad/postman/issues", Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            foreach ( var issue in issues)
            {
                Assert.That(issue.title, Is.Not.Empty);
                Assert.That(issue.number, Is.GreaterThan(0));
            }


        }

        [Test]
        public void Test_CreateNewIssue()
        {
            //Arrange
            var request = new RestRequest("/repos/botevad/postman/issues", Method.Post);
            var issueBody = new
            {
                title = "Test issue from RestSharp " + DateTime.Now.Ticks,
                body = "some body for my issue",
                labels = new string[] { "bug", "critical", "release" }
            };

            request.AddBody(issueBody);

            //Act
            var response = this.client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "HTTP Status Code property");
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo(issueBody.title));
            Assert.That(issue.body, Is.EqualTo(issueBody.body));
        }

        [Test]
        public void Test_EditIssue()
        {
            //Arrange
            var request = new RestRequest("/repos/botevad/postman/issues/1", Method.Patch);
            var issueBody = new
            {
                title = "EDITED: Test issue from RestSharp " + DateTime.Now.Ticks,

            };

            request.AddBody(issueBody);

            //Act
            var response = this.client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo(issueBody.title));
          
        }

        [TestCase("us", "90210", "United States")]
        [TestCase("bg", "1000", "Bulgaria")]
        [TestCase("de", "01067", "Germany")]
        public void Test_Zippopotamus_DD(string countryCode, string zipCode, string expectedCountry)
        {
            var restClient = new RestClient("https://api.zippopotam.us");
            var request = new RestRequest(countryCode + "/" + zipCode, Method.Get);
            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");
            var location = JsonSerializer.Deserialize<Location>(response.Content);

            Assert.That(location.Country, Is.EqualTo(expectedCountry));

        }

    }
}