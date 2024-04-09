using HangFireServiceAPI.Models.Config;
using HangFireServiceAPI.Models.Response;
using Microsoft.Extensions.Options;

namespace HangFireServiceAPI.Services
{
    public interface IConfigService
    {
        GetHangFireServiceSettingsResponse GetBackgroundServiceConfig();
    }

    public class ConfigService : IConfigService
    {
        private readonly IOptionsMonitor<HangFireServiceSettings> _options;

        public ConfigService(IOptionsMonitor<HangFireServiceSettings> options)
        {
            _options = options;
        }

        public GetHangFireServiceSettingsResponse GetBackgroundServiceConfig()
        {
            try
            {
                var bgServiceSettingsResponse = new GetHangFireServiceSettingsResponse
                {
                    CronExpression = _options.CurrentValue.CronExpression,
                    FilePath = _options.CurrentValue.FilePath,
                };

                return bgServiceSettingsResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
