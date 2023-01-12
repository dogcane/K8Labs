using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;
using UIComponent.Models;

namespace UIComponent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ServicesSettings _servicesSettings;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _servicesSettings = configuration.GetSection("Services").Get<ServicesSettings>();
        }

        public IActionResult Index()
        {
            ViewBag.wsvc_uri = _servicesSettings.WeatherEndpoint;
            try
            {
                RestClient client = new RestClient(_servicesSettings.WeatherEndpoint);
                RestRequest request = new RestRequest("WeatherForecast", Method.Get);
                var response = client.Execute<WeatherForecast[]>(request);
                return View(response.Data);
            }
            catch (Exception ex)
            {
                ViewBag.exception = ex;
                return View(Enumerable.Empty<WeatherForecast>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}