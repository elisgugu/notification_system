using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using notification_system.Hubs;
using notification_system.Interfaces;
using notification_system.Models;
using NuGet.Protocol.Core.Types;

namespace notification_system.Services {
    public class ExpirationService : BackgroundService {

        private ICertificateRepository _repository;
        private PeriodicTimer _timer;
        private CancellationTokenSource _cts;
        private IServiceScopeFactory _factory;
        private IHubContext<CertificateHub> _certificateHub;

        public Task StopAsync() {
            _cts?.Cancel();
            _timer.Dispose();
            _timer = null;
            return Task.CompletedTask;
        }
        public Task StartAsync() {
            _cts = new CancellationTokenSource();
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            _= ExecuteAsync(_cts.Token);
            return Task.CompletedTask;
        }
        public ExpirationService(IServiceScopeFactory factory, IHubContext<CertificateHub> hub) {
            _factory = factory;
            _certificateHub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (_timer != null && await _timer!.WaitForNextTickAsync(_cts.Token)) {
                await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                var context = asyncScope.ServiceProvider.GetRequiredService<NotificationCenterContext>();

                _repository = asyncScope.ServiceProvider.GetRequiredService<ICertificateRepository>();
                var expiredCertificates = await _repository.GetExpiredCertificates(context);
                if (expiredCertificates.Any())
                    await _certificateHub.Clients.All.SendAsync("certificatesExpired", expiredCertificates);

            }
        }
    }
}
