using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppInsightsWebApi.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var request = Request;
            if (request.Headers.Contains("aiSession"))
            {
                var aiSession = request.Headers.GetValues("aiSession").First();
                TelemetryConfiguration.Active.InstrumentationKey = aiSession;

                //var context = new TelemetryContext();
                //var config = new TelemetryConfiguration();
                //var client = new TelemetryClient(config);
                var client = new TelemetryClient();
                if (client.Context.Properties.ContainsKey("Session"))
                {
                    var ses = client.Context.Properties["Session"];                    
                }
                var ci = TelemetryConfiguration.Active.ContextInitializers.First();
                var ti = TelemetryConfiguration.Active.TelemetryInitializers.First();
                var tm = TelemetryConfiguration.Active.TelemetryModules.First();
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            throw new InvalidOperationException("sorry, no implementation of values/get{id}");
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
