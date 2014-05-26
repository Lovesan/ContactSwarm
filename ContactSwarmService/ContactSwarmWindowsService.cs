using System;
using System.ServiceModel;
using System.ServiceProcess;
using ContactSwarmService.Service;
using NLog;

namespace ContactSwarmService
{
    class ContactSwarmWindowsService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ServiceHost _serviceHost;

        public ContactSwarmWindowsService()
        {
            _serviceHost = new ServiceHost(typeof(DataService));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;
            var disposable = _serviceHost as IDisposable;
            if(disposable != null)
                disposable.Dispose();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Trace("Starting Windows Service");
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            _serviceHost.Close();
        }
    }
}
