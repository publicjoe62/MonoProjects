using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WCFMonoService
{
	[ServiceContract]
	public interface IMyService
	{
		[OperationContract, WebGet]
		String GetData();
	}
}

