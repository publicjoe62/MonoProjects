using System;

namespace WCFMonoService
{
	public class MyService : IMyService
	{
		public String GetData()
		{
			return "TMS RestApi call!";
		}
	}
}

