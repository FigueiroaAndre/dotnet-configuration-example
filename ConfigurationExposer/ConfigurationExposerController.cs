using Microsoft.AspNetCore.Mvc;
using ConfigurationExample.Configuration;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.ConfigurationExposer;

[ApiController]
[Route("[controller]")]
public class ConfigurationExposerController : ControllerBase
{
    [HttpGet("/general", Name = "GetGeneralConfiguration")]
    public ActionResult<GeneralConfigurationDTO> GetGeneralConfiguration(IOptions<GeneralConfiguration> options)
    {
        var config = new GeneralConfigurationDTO
        {
            Settings1 = options.Value.Settings1,
            Settings2 = options.Value.Settings2,
            Settings3 = options.Value.Settings3,
            Settings4 = options.Value.Settings4,
            Settings5 = options.Value.Settings5,
            Settings6 = options.Value.Settings6,
            Settings7 = options.Value.Settings7,
        };

        return Ok(config);
    }


    [HttpGet("/general-snapshot", Name = "GetGeneralConfigurationSnapshot")]
    public ActionResult<GeneralConfigurationDTO> GetGeneralConfigurationSnapshot(IOptionsSnapshot<GeneralConfiguration> options)
    {
        var config = new GeneralConfigurationDTO
        {
            Settings1 = options.Value.Settings1,
            Settings2 = options.Value.Settings2,
            Settings3 = options.Value.Settings3,
            Settings4 = options.Value.Settings4,
            Settings5 = options.Value.Settings5,
            Settings6 = options.Value.Settings6,
            Settings7 = options.Value.Settings7,
        };

        return Ok(config);
    }


    [HttpGet("/general-monitor", Name = "GetGeneralConfigurationMonitor")]
    public ActionResult<GeneralConfigurationDTO> GetGeneralConfigurationMonitor(IOptionsMonitor<GeneralConfiguration> options)
    {
        var config = new GeneralConfigurationDTO
        {
            Settings1 = options.CurrentValue.Settings1,
            Settings2 = options.CurrentValue.Settings2,
            Settings3 = options.CurrentValue.Settings3,
            Settings4 = options.CurrentValue.Settings4,
            Settings5 = options.CurrentValue.Settings5,
            Settings6 = options.CurrentValue.Settings6,
            Settings7 = options.CurrentValue.Settings7,
        };

        return Ok(config);
    }
}
