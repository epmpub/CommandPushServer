using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetMQPUSHServ
{

    public static class MQExtensions
    {
        public static void SendT<T>(this PublisherSocket socket, T src)
        {
            var json = JsonConvert.SerializeObject(src);
            socket.SendFrame(json);
        }

        public static T ReceiveT<T>(this PublisherSocket socket)
        {
            var json = socket.ReceiveFrameString();
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        //public static void SendT<T>(this SubscriberSocket socket, T src)
        //{
        //    var json = JsonConvert.SerializeObject(src);
        //    socket.Send(json);
        //}

        //public static T ReceiveT<T>(this SubscriberSocket socket)
        //{
        //    var json = socket.ReceiveString();
        //    T obj = JsonConvert.DeserializeObject<T>(json);
        //    return obj;
        //}

    }


    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:PushServ.exe <Scheduler> <SpanTime>");
                return;
            }
            else
            {
                
            }

            if (args[0] == "Now" || args[0] == "now")
            {
                using (var pubSocket = new PublisherSocket())
                {
                    Console.WriteLine("Binding");
                    pubSocket.Options.SendHighWatermark = 1000;
                    pubSocket.Bind("tcp://*:12345");
                    Console.WriteLine("Push...");

                    int Inteval = -1;
                    bool b = int.TryParse(args[1], out Inteval);
                    if (!b)
                    {
                        return;
                    }

                    int times = 0;

                    b = int.TryParse(args[2], out times);

                    if (!b)
                    {
                        return;
                    }

                    for (int i = 0; i < times; i++)
                    {
                        var command = new CommandX { Id = 1, Name = "TestABCD", Url = "www.baidu.com/index.html" };

                        pubSocket.SendT(command);

                        Console.Write(".");

                        Thread.Sleep(Inteval * 1000);
                    }
                }
            }
            else
            {
                Console.WriteLine("Usage:PushServ.exe <Scheduler> <SpanTime> <Times>");
            }

            
        }
    }
}
