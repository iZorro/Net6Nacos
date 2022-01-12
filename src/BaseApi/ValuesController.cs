using Microsoft.AspNetCore.Mvc;

namespace BaseApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Nacos.V2.INacosNamingService _svc;

        public ValuesController(Nacos.V2.INacosNamingService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public string Get()
        {
            return "Ok~" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }

        /// <summary>
        /// 从nacos中获取服务并选一个调用
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public async Task<string> Test()
        {
            // 这里需要知道被调用方的服务名
            var instance = await _svc.SelectOneHealthyInstance("BaseApi", "DEFAULT_GROUP");
            var host = $"{instance.Ip}:{instance.Port}";

            var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                ? $"https://{host}"
                : $"http://{host}";

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return "empty";
            }

            var url = $"{baseUrl}/api/values";

            using var client = new HttpClient();
            var result = await client.GetAsync(url);
            return await result.Content.ReadAsStringAsync();
        }
    }
}
