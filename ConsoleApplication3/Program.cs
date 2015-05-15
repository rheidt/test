using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel.Discovery;

namespace EndpointDiscoveryTooly
{
	class Program
	{
		static void Main(string[] args)
		{
			// discover Patient Management (rmi) endpoint with fallback by endpoint contract type
			var dc = new DiscoveryClient(new UdpDiscoveryEndpoint());
			var fcr = new FindCriteria();

			// limit discovery to our configured scope
			var envScope = ConfigurationManager.AppSettings["envScope"];

			int dur = 5;
			Console.Write("How long to wait in seconds? (5s default):");
			string duration = Console.ReadLine();
			if (!Int32.TryParse(duration, out dur))
				dur = 5;

			fcr.Scopes.Add(new Uri(envScope));
			//fcr.Scopes.Add(new Uri(@"http://www.medicity.com/2015/03/MpiService")); // THIS IS HARD CODED
			//fcr.MaxResults = 1;
			fcr.Duration = new TimeSpan(0, 0, dur);

			Console.WriteLine("Environment Scope:\n\r{0}", envScope);
			Console.WriteLine("Finding endpoints for {0} second(s)...", dur.ToString());
			var pt_mgmt_ept = dc.Find(fcr);
			foreach (var found in pt_mgmt_ept.Endpoints)
			{
				Console.WriteLine(found.Address.Uri.ToString());
				Console.WriteLine("\tContractTypeNames:");
				foreach (var c in found.ContractTypeNames)
					Console.WriteLine("\t{0}", c.Name);
				Console.WriteLine("\tScopes:");
				foreach (var d in found.Scopes)
					Console.WriteLine("\t{0}", d.ToString());
			}
			//Console.ReadKey();

		}
	}
}
