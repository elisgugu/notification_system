using Microsoft.Identity.Client;
using notification_system.Hubs;
using notification_system.Repository;
using NuGet.Protocol.Core.Types;

namespace notification_system.Services {
    public class ExpirationService  {

        private ICertificateRepository _repository;
        private CertificateHub _hub;
        private PeriodicTimer _timer;
        private CancellationTokenSource _cts;
        
        public Task StopAsync() {
            _cts?.Cancel();
            _timer.Dispose();
            _timer = null;
            return  Task.CompletedTask;
        }
        public Task StartAsync() {
            _cts = new CancellationTokenSource();
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            TimerCallback();
            return Task.CompletedTask;
        }
        public ExpirationService(CertificateHub certificateHub, ICertificateRepository repository) {
            _hub = certificateHub;
            _repository = repository;
        }

        private async void TimerCallback() {

            while (_timer != null && await _timer!.WaitForNextTickAsync(_cts.Token)) {
                var expiredCertificates = _repository.GetExpiredCertificates();
                if (expiredCertificates.Any())
                    await _hub.SendRequests(expiredCertificates);
            }
        }
    }
}
