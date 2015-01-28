using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppInsightsWebApi.Controllers
{
    //[Authorize]
    public class SlowMethodController : ApiController
    {
        // GET api/values
        public async Task<IEnumerable<string>> Get()
        {
            var delayInMs = new Random((int) DateTime.Now.Ticks).Next(500, 5000);
            await Task.Delay(delayInMs);
            return new string[] { "i", "am", "very", "slow.", "slept", "for", delayInMs.ToString(CultureInfo.InvariantCulture), "ms" };
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
