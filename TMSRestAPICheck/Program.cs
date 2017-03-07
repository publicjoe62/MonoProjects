using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;

namespace harman.com.TMSRestAPICheck
{
	class TMSRestAPICheck
	{
		private int intProjectID = 347; // BMW_IUK_MGU
		private string tms_server = "https://tms-api.harman.com/RestServiceV10.svc/";  // produktiv
		//private string tms_server = "https://tmstest-api.harman.com/RestServiceV10.svc/";  // test
		private bool taskIsRunning;

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
					Credentials = new System.Net.NetworkCredential ("jvelmans", "defg45678!")
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

		private void ReleasesOfProjectsAsyncRestSharp(string strOptions)
		{
			taskIsRunning = true;
			o (string.Format("ReleasesOfProjectsAsyncRestSharp. strOptions='{0}'", strOptions));

			string strAddress = tms_server + "/project/" + intProjectID + "/release" + strOptions;
			o ("address = " + strAddress);					

			var client = new RestClient(strAddress);
			client.Authenticator = new HttpBasicAuthenticator ("jvelmans", "defg45678!");

			/*
			var request = new RestRequest("resource/{id}", Method.POST);
			request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
			request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

			// easily add HTTP Headers
			request.AddHeader("header", "value");

			// add files to upload (works with compatible verbs)
			request.AddFile(path);

			// execute the request
			IRestResponse response = client.Execute(request);
			*/
			var request = new RestRequest ();
			IRestResponse response = client.Execute(request);
			var content = response.Content; // raw content as string

			o ("response content : " + response.Content);
			taskIsRunning = false;

			/*
			// or automatically deserialize result
			// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
			RestResponse<Person> response2 = client.Execute<Person>(request);
			var name = response2.Data.Name;

			// easy async support
			client.ExecuteAsync(request, response => {
				Console.WriteLine(response.Content);
			});

			// async with deserialization
			var asyncHandle = client.ExecuteAsync<Person>(request, response => {
				Console.WriteLine(response.Data.Name);
			});

			// abort the request on demand
			asyncHandle.Abort();
			*/
		}

		private void ReleasesOfProjects(string strOptions = "?state=Testing")
		{
			
			Task.Run( () => ReleasesOfProjectsAsync(strOptions));
			while (true)
				Thread.Sleep (500);
		}



		private void ReleasesOfProjectsRestSharp (string strOptions = "?state=Closed")
		{
			taskIsRunning = true;
			Task.Run (() => ReleasesOfProjectsAsyncRestSharp (strOptions));
			while (taskIsRunning)
				Thread.Sleep (500);
		}

		private void execChecking()
		{
			o ("execChecking");
			// this.ReleasesOfProjects ();
			while (true) {
				this.ReleasesOfProjectsRestSharp ();
				Thread.Sleep (1000);
			}
		}

		public static void Main (string[] args)
		{
			o("TMS RestAPI check");
			TMSRestAPICheck TMSRestAPICheck_inst = new TMSRestAPICheck ();
			TMSRestAPICheck_inst.execChecking ();
		}
	}
}
