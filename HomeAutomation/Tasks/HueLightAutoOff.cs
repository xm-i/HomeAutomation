using Microsoft.Extensions.Logging;

using System.Net.NetworkInformation;

namespace HomeAutomation.Tasks;
public class HueLightAutoOff : IAutomationTask
{
    private readonly ILogger<HueLightAutoOff> _logger;

    public HueLightAutoOff(ILogger<HueLightAutoOff> logger)
    {
        this._logger = logger;
    }

    public async Task ExecuteAsync()
    {
        this._logger.LogInformation($"Start");
        var ping = new Ping();
        var httpClient = new HttpClient();
        var contentOn = new StringContent(@"{""on"":true}");
        var contentOff = new StringContent(@"{""on"":false}");
        while (true)
        {
            this._logger.LogInformation($"Check");
            await Task.Delay(new TimeSpan(0, 0, 10));
            var reply = await ping.SendPingAsync("13700k.localnet");
            if (reply.Status != IPStatus.Success)
            {
                await Task.Delay(new TimeSpan(0, 0, 20));
                reply = await ping.SendPingAsync("13700k.localnet");
                if (reply.Status != IPStatus.Success)
                {
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/1/state", contentOff);
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/2/state", contentOff);
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/3/state", contentOff);
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/4/state", contentOff);
                }
            }
            else if (reply.Status == IPStatus.Success)
            {
                await Task.Delay(new TimeSpan(0, 0, 20));
                reply = await ping.SendPingAsync("13700k.localnet");
                if (reply.Status == IPStatus.Success)
                {
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/1/state", contentOn);
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/2/state", contentOn);
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/3/state", contentOn);
                    await httpClient.PutAsync("http://hue-bridge.localnet/api/lfRks8huc0jr-AIaa0l9iFdbatH38yPCsvEqUoh1/lights/4/state", contentOn);
                }
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    void IDisposable.Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}