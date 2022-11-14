using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebAppInsights.Models;
using Microsoft.Net.Http.Headers;
using System.Data.SqlClient;

namespace WebAppInsights.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        Task.Delay(5000).Wait();
        return View();
    }

    public IActionResult HighCPU()
    {


        var datea = System.DateTime.Now;
        ViewBag.Message = $"Start CPU Time: {datea}";

        int result = 1;
        for (int x = 1; x < 20; x++)
        {
            for (int i = 1; i < 2147483647; i++)
            {
                result = result * i;
            }
        }

        var dateb = System.DateTime.Now;
        ViewBag.Message2 = $"End CPU Time: {dateb}";


        return View();
    }

    public IActionResult DBConnection()
    {


        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "webapp1insightsdemosqlsrv.database.windows.net";
        builder.UserID = "aadmin";
        builder.Password = "Welcome#123";
        builder.InitialCatalog = "testDB";

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            var datea = System.DateTime.Now;
            _logger.LogInformation("Opening DB Connection: {time}", datea);

            connection.Open();

            var dateb = System.DateTime.Now;
            _logger.LogInformation("DB Connection established: {time}", dateb);
            System.TimeSpan diff = dateb.Subtract(datea);

            _logger.LogInformation("DB Connection took: {time}", diff);

            String sql = "SELECT Name, Email FROM Users";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new System.Text.StringBuilder();
                    while (reader.Read())
                    {
                        result.AppendLine("\n" + reader.GetString(0) + " - " + reader.GetString(1));
                    }

                    ViewBag.Message3 = result.ToString();
                }
            }
        }


        return View();
    }

    public IActionResult Crash()
    {
        ViewBag.Message = "Crash";

        var fn = 10;
        var sn = 0;
        var a = fn / sn;

        return View();
    }

    public async Task<IActionResult> HttpRequest()
    {
        ViewBag.Message = "Http";

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "https://localhost:7004/api/SlowAPIGet")
        {
            Headers =
            {
                { HeaderNames.Accept, "application/vnd.github.v3+json" },
                { HeaderNames.UserAgent, "HttpRequestsSample" }
            }
        };

        var httpClient = _httpClientFactory.CreateClient("HttpClientWithSSLUntrusted");

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            Console.WriteLine("Test");

            var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);


            //GitHubRepo myDeserializedClass = JsonConvert.DeserializeObject<GitHubRepo>(jsonString);            
            //var json = JsonConvert.SerializeObject(myDeserializedClass, Formatting.Indented);

            ViewBag.Message = jsonString;


            //Console.WriteLine(System.Text.Encoding.UTF8.GetString(contentStream.Read()));
        }

        return View();
    }

    public IActionResult CatchException()
    {
        var id = 3;
        try
        {
            if (id == 3)
            {
                throw new Exception("Test exception");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception Catching TestExp({Id})", id);
            return NotFound();
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
/*    public class GitHubRepoOwner
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class GitHubRepo
    {
        public int id { get; set; }
        public string node_id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public bool @private { get; set; }
        public GitHubRepoOwner owner { get; set; }
        public string html_url { get; set; }
        public object description { get; set; }
        public bool fork { get; set; }
        public string url { get; set; }
        public string forks_url { get; set; }
        public string keys_url { get; set; }
        public string collaborators_url { get; set; }
        public string teams_url { get; set; }
        public string hooks_url { get; set; }
        public string issue_events_url { get; set; }
        public string events_url { get; set; }
        public string assignees_url { get; set; }
        public string branches_url { get; set; }
        public string tags_url { get; set; }
        public string blobs_url { get; set; }
        public string git_tags_url { get; set; }
        public string git_refs_url { get; set; }
        public string trees_url { get; set; }
        public string statuses_url { get; set; }
        public string languages_url { get; set; }
        public string stargazers_url { get; set; }
        public string contributors_url { get; set; }
        public string subscribers_url { get; set; }
        public string subscription_url { get; set; }
        public string commits_url { get; set; }
        public string git_commits_url { get; set; }
        public string comments_url { get; set; }
        public string issue_comment_url { get; set; }
        public string contents_url { get; set; }
        public string compare_url { get; set; }
        public string merges_url { get; set; }
        public string archive_url { get; set; }
        public string downloads_url { get; set; }
        public string issues_url { get; set; }
        public string pulls_url { get; set; }
        public string milestones_url { get; set; }
        public string notifications_url { get; set; }
        public string labels_url { get; set; }
        public string releases_url { get; set; }
        public string deployments_url { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime pushed_at { get; set; }
        public string git_url { get; set; }
        public string ssh_url { get; set; }
        public string clone_url { get; set; }
        public string svn_url { get; set; }
        public object homepage { get; set; }
        public int size { get; set; }
        public int stargazers_count { get; set; }
        public int watchers_count { get; set; }
        public string language { get; set; }
        public bool has_issues { get; set; }
        public bool has_projects { get; set; }
        public bool has_downloads { get; set; }
        public bool has_wiki { get; set; }
        public bool has_pages { get; set; }
        public int forks_count { get; set; }
        public object mirror_url { get; set; }
        public bool archived { get; set; }
        public bool disabled { get; set; }
        public int open_issues_count { get; set; }
        public object license { get; set; }
        public bool allow_forking { get; set; }
        public bool is_template { get; set; }
        public List<object> topics { get; set; }
        public string visibility { get; set; }
        public int forks { get; set; }
        public int open_issues { get; set; }
        public int watchers { get; set; }
        public string default_branch { get; set; }
        public object temp_clone_token { get; set; }
        public int network_count { get; set; }
        public int subscribers_count { get; set; }
    }
*/

