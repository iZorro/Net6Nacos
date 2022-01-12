using Microsoft.AspNetCore.Mvc;
using Nacos.V2;

namespace UpService
{

    /// <summary>
    /// 操作配置信息
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly INacosConfigService _svc;

        public ConfigController(IConfiguration configuration, INacosConfigService svc)
        {
            _configuration = configuration;
            _svc = svc;
        }

        [HttpGet("get")]
        public async Task<string> Get(string dataId= "default-dev.yaml")
        {
            var res = await _svc.GetConfig(dataId, "DEFAULT_GROUP", 3000).ConfigureAwait(false);

            return res ?? "empty config";
        }

        [HttpGet("set")]
        public async Task<bool> Put(string dataId = "default-dev.yaml")
        {
            var res = await _svc.PublishConfig(dataId, "DEFAULT_GROUP", "test:demo-0001").ConfigureAwait(false);

            return res; 
        }

        [HttpGet("listener")]
        public async Task<string> Listen(string dataId = "default-dev.yaml")
        {
            await _svc.AddListener(dataId, "DEFAULT_GROUP", _configListen).ConfigureAwait(false);
            return "ok";
        }

        [HttpGet("unlistener")]
        public async Task<string> UnListen(string dataId = "default-dev.yaml")
        {
            await _svc.RemoveListener(dataId, "DEFAULT_GROUP", _configListen).ConfigureAwait(false);

            return "ok";
        }

        private static readonly CusConfigListen _configListen = new ();

        public class CusConfigListen : Nacos.V2.IListener
        {
            public void ReceiveConfigInfo(string configInfo)
            {
                System.Console.WriteLine("config updating " + configInfo);
            }
        }
    }
}
