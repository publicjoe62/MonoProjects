using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFMonoService
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			String baseAddress = "http://" + Environment.MachineName + ":8000/MyService";
			ServiceHost host = new ServiceHost (typeof(MyService), new Uri (baseAddress));
			host.AddServiceEndpoint (typeof(IMyService), new WebHttpBinding (), "").Behaviors.Add (new WebHttpBehavior ());
			host.Open ();
			Console.WriteLine ("host '" + baseAddress + "' opened");
			Console.WriteLine ("Press ENTER to close host");
			Console.ReadLine ();
			host.Close ();
		}
	}
}
