using Microsoft.AspNetCore.Mvc;
using Nacos.V2.Utils;

namespace UpService
{

    /// <summary>
    /// 手动操作服务实例-增删监听
    /// </summary>
    public class NamingController : Controller
    {

        private readonly Nacos.V2.INacosNamingService _client;

        public NamingController(Nacos.V2.INacosNamingService client)
        {
            _client = client;
        }

        // GET n/g
        [HttpGet("GetAllInstances")]
        public async Task<string> GetAllInstances()
        {
            var list = await _client.GetAllInstances("MyService1", false).ConfigureAwait(false);

            var res = list.ToJsonString();

            return res ?? "GetAllInstances";
        }

        // GET n/r
        [HttpGet("RegisterInstance")]
        public async Task<string> RegisterInstance()
        {
            // await _client.RegisterInstance("mysvc", "127.0.0.1", 9635);
            var instance = new Nacos.V2.Naming.Dtos.Instance
            {
                Ip = "127.0.0.1",
                Ephemeral = true,
                Port = 5088,
                ServiceName = "mysvc2"
            };

            await _client.RegisterInstance("MyService1", instance).ConfigureAwait(false);

            return "RegisterInstance ok";
        }

        // GET n/r2
        [HttpGet("RegisterInstance2")]
        public async Task<string> RegisterInstance2()
        {
            // await _client.RegisterInstance("mysvc", "127.0.0.1", 9635);
            var instance = new Nacos.V2.Naming.Dtos.Instance
            {
                Ip = "127.0.0.1",
                Ephemeral = true,
                Port = 5089,
                ServiceName = "MyService1"
            };

            await _client.RegisterInstance("MyService1", instance).ConfigureAwait(false);

            return "RegisterInstance ok";
        }

        // GET n/dr
        [HttpGet("DeregisterInstance")]
        public async Task<string> DeregisterInstance()
        {
            // await _client.RegisterInstance("mysvc", "127.0.0.1", 9635);
            var instance = new Nacos.V2.Naming.Dtos.Instance
            {
                Ip = "127.0.0.1",
                Ephemeral = true,
                Port = 9562,
                ServiceName = "mysvc2"
            };

            await _client.DeregisterInstance("mysvc2", instance).ConfigureAwait(false);

            return "DeregisterInstance ok";
        }

        // GET n/si
        [HttpGet("SelectInstances")]
        public async Task<string> SelectInstances()
        {
            var list = await _client.SelectInstances("mysvc2", true, false).ConfigureAwait(false);

            var res = list.ToJsonString();

            return res ?? "SelectInstances ok";
        }

        // GET n/gs
        [HttpGet("GetServicesOfServer")]
        public async Task<string> GetServicesOfServer()
        {
            var list = await _client.GetServicesOfServer(1, 10).ConfigureAwait(false);

            var res = list.ToJsonString();

            return res ?? "GetServicesOfServer";
        }

        // GET n/sub
        [HttpGet("Subscribe")]
        public async Task<string> Subscribe()
        {
            await _client.Subscribe("mysvc2", listener).ConfigureAwait(false);
            return "Subscribe";
        }

        // GET n/unsub
        [HttpGet("Unsubscribe")]
        public async Task<string> Unsubscribe()
        {
            await _client.Unsubscribe("mysvc2", listener).ConfigureAwait(false);
            return "UnSubscribe";
        }

        // NOTE: MUST keep Subscribe and Unsubscribe to use one instance of the listener!!!
        // DO NOT create new instance for each opreation!!!
        private static readonly CusListener listener = new ();

        public class CusListener : Nacos.V2.IEventListener
        {
            public Task OnEvent(Nacos.V2.IEvent @event)
            {
                if (@event is Nacos.V2.Naming.Event.InstancesChangeEvent e)
                {
                    System.Console.WriteLine("CusListener");
                    System.Console.WriteLine("ServiceName" + e.ServiceName);
                    System.Console.WriteLine("GroupName" + e.GroupName);
                    System.Console.WriteLine("Clusters" + e.Clusters);
                    System.Console.WriteLine("Hosts" + e.Hosts.ToJsonString());
                }

                return Task.CompletedTask;
            }
        }
    }
}
