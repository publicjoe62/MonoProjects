using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace harman.com.TMSRestAPICheck
{
	class TMSRestAPICheck
	{
		private int intProjectID = 347; // BMW_IUK_MGU
		//private string tms_server = "https://tms-api.harman.com/RestServiceV10.svc/";  // produktiv
		private string tms_server = "https://tmstest-api.harman.com/RestServiceV10.svc/";  // test

		private static void o(string msg)
		{
			Console.WriteLine(msg);
		}

		private async void ReleasesOfProjectsAsync(string strOptions)
		{
			o (string.Format("ReleasesOfProjectsAsync. strOptions='{0}'", strOptions));

			string strAddress = tms_server + "/project/" + intProjectID + "/release" + strOptions;



			while (true) {
				HttpClientHandler handler = new HttpClientHandler {
					Credentials = new System.Net.NetworkCredential ("xvelmans", "deig45478!")
				};					
				using (HttpClient client = new HttpClient (handler)) {
						o (" ");
						o ("address = " + strAddress);					
						try {
							using (HttpResponseMessage response = await client.GetAsync (strAddress)) {
								var s = await response.Content.ReadAsStringAsync ();
								o ("result " + s);
							}
							
						} catch (Exception ex) {
							o ("exception: " + ex.Message);
						}
				}
			}
		}

		private void ReleasesOfProjects(string strOptions = "?state=Testing")
		{
			Task.Run( () => ReleasesOfProjectsAsync(strOptions));
			while (true)
				Thread.Sleep (500);
		}

		private void execChecking()
		{
			o ("execChecking");
			this.ReleasesOfProjects ();
		}

		public static void Main (string[] args)
		{
			o("TMS RestAPI check");
			TMSRestAPICheck TMSRestAPICheck_inst = new TMSRestAPICheck ();
			TMSRestAPICheck_inst.execChecking ();
		}
	}
}
