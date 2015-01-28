using System.Collections.Generic;
using System.Runtime.Serialization.Configuration;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace AppInsightsWebApi.Controllers
{
    public class PollController : Controller
    {
        public ActionResult Index(string type)
        {
            var typeOut = type ?? "unknown";//string.IsNullOrWhiteSpace(type) ? "unknown" : type;
            if (typeOut == "dev" || typeOut == "itpro")
            {
                var client = new TelemetryClient();
                client.TrackEvent("poll", new Dictionary<string, string>{{"type", typeOut}}, null);
                client.TrackPageView("poll/" + typeOut);
            }
            ViewBag.PollType = typeOut;

            return View();
        }
    }
}
