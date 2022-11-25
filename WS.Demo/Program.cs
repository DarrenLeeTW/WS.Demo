using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WS.Demo.Controller;
using WebSocketSharp.Server;


namespace WS.Demo {
    class Program {
        static void Main(string[] args) {
            var msgCreater = new Thread(() => {
                while (true) {
                    MessageQueueSingleton.Instance().AddMsg();
                    Thread.Sleep(250);
                }
               });
            msgCreater.Start();

            var wssv = new WebSocketServer(55688);
            wssv.AddWebSocketService<BroadcastBehavior>("/Broadcast");
            wssv.Start();
            if (wssv.IsListening) {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);
                foreach (var path in wssv.WebSocketServices.Paths) {
                    Console.WriteLine("- {0}", path);
                }
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();
            MessageQueueSingleton.Instance().FreeQueue();
            msgCreater.Abort();
            wssv.Stop();

        }
    }
}
