using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace AppInsightsWebApi.Controllers
{
    //[Authorize]
    public class LoggingController : ApiController
    {
        // GET api/values
        public async Task<IEnumerable<string>> Get()
        {
            LogWithNLog();

            return new string[] { "i", "have", "logged" };
        }

        private static void LogWithNLog()
        {
// Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var aiTarget = new ApplicationInsightsTarget();
            config.AddTarget("file", aiTarget);

            // Step 3. Set target properties 
            consoleTarget.Layout = @"${date:format=HH\\:MM\\:ss} ${logger} ${message}";
            //fileTarget.FileName = "${basedir}/file.txt";
            //fileTarget.Layout = "${message}";
            
            aiTarget.InstrumentationKey = "82ac5480-f257-4768-abfe-63c694cd9cc0";

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, aiTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            // Example usage
            Logger logger = LogManager.GetLogger("Example");
            logger.Trace("trace log message");
            logger.Debug("debug log message");
            logger.Info("info log message");
            logger.Warn("warn log message");
            logger.Error("error log message");
            logger.Fatal("fatal log message");
            
            
            
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
