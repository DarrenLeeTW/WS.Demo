using System.Text;
using System.Threading;
using WebSocketSharp.Server;

namespace WS.Demo.Controller {
    public class BroadcastBehavior : WebSocketBehavior {
        public BroadcastBehavior() { }
        protected override void OnOpen() {

            while (true) {
                string msg = null;
                MessageQueueSingleton.Instance().GetMsg(out msg);
                if (msg != null) {
                    Sessions.BroadcastAsync(Encoding.UTF8.GetBytes(msg), null);
                }
                Thread.Sleep(1);
            }
        }
    }
}
