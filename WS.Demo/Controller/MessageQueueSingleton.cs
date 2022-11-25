using System;
using System.Collections.Concurrent;

namespace WS.Demo.Controller {
    public class MessageQueueSingleton {
        private static volatile MessageQueueSingleton _instance;
        private static readonly object _lockObj = new object();
        private static ConcurrentQueue<string> _msgQueue;
        private MessageQueueSingleton() { }
        public static MessageQueueSingleton Instance() {
            if (_instance == null) {
                lock (_lockObj) {
                    if (_instance == null) {
                        _instance = new MessageQueueSingleton();
                        _msgQueue = new ConcurrentQueue<string>();
                    }
                }
            }
            return _instance;
        }

        public void AddMsg() {
            try {
                _msgQueue.Enqueue(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            } catch (Exception ex) {
                //do something
            }
        }

        public void GetMsg(out string msg) {
            msg = null;
            try {
                lock (_lockObj) {
                    if (!_msgQueue.TryDequeue(out msg)) {
                        msg = null;
                    }
                }
            } catch (Exception ex) {
                //do something
            }
        }
        public void FreeQueue() {
            lock (_lockObj) {
                while (_msgQueue.TryDequeue(out string temp))
                {
                }
            }
        }
    }
}
