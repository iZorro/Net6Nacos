using Microsoft.AspNetCore.Mvc;

namespace BaseApi
{
    /// <summary>
    /// 请求下游服务
    /// </summary>
    [Route("api/[controller]", Name ="请求下游服务")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Nacos.V2.INacosNamingService _svc;

        public ValuesController(Nacos.V2.INacosNamingService svc)
        {
            _svc = svc;
        }


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

            //using 新语法
            using var client = new HttpClient();
            var result = await client.GetAsync(url);
            return await result.Content.ReadAsStringAsync();
        }
       
    }
}
