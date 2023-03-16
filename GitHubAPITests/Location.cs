using System.Text.Json.Serialization;

namespace GitHubAPITests
{
    internal class Location
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}