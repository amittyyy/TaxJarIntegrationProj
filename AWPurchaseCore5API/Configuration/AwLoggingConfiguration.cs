using NLog;
using NLog.AWS.Logger;
using NLog.Config;

namespace AWPurchaseCore5API.Configuration
{
    public static class AwLoggingConfiguration
    {
		public static void ConfigureLoggingTargets(string environment)
		{

			var config = new LoggingConfiguration();
			var logGroup = "";

			if (environment.Equals("dev"))
				logGroup = "awPurchase-api-dev";
			else if (environment.Equals("qa"))
				logGroup = "awPurchase-api-qa";
			else if (environment.Equals("prod"))
				logGroup = "awPurchase-api";
			else
				logGroup = "awPurchase-dev";

			var awsTarget = new AWSTarget()
			{
				LogGroup = logGroup,
				Region = "us-east-1",
				Layout = "${longdate} | ${logger} | ${uppercase:${level}} | ${message} | ${exception:format=toString,Data} | url: ${aspnet-request-url} | action: ${aspnet-mvc-action}",
				Profile = "AWSPayrollMobileApi"
			};
			config.AddTarget("aws", awsTarget);

			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, awsTarget));

			LogManager.Configuration = config;
		}
	}
}
