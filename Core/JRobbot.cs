using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using jrobbot.Commands;
using jrobbot.Configs;

namespace jrobbot.Core
{
    class JRobbot
    {
        static Thread thread;

        public static void Start()
        {
            if (thread != null) return;
            Log.Trace("starting main thread");
            thread = new Thread(MainCode) {IsBackground = true};
            thread.Start();
            Log.Trace("main thread started");
        }

        public static void Stop()
        {
            if (thread == null) return;
            Log.Info("stopping main thread");
            if (thread.IsAlive) thread.Abort();
            if (thread.IsAlive) thread.Join(5000);
            thread = null;
            Log.Info("main thread stopped");
        }

        public static string LastError { get; set; }

        private static void MainCode()
        {
            var stop = false;
            var conn = CreateConnection();

            conn.OnError += delegate(object sender, Exception exception)
                {
                    Log.Error("error in connection: {0}", exception.ToString());
                    LastError = exception.Message;
                    stop = true;
                };

            conn.OnAuthError += (sender, element) =>
                {
                    Log.Error("auth error: {0}", element.ToString());
                    LastError = "Auth error";
                    stop = true;
                };

            conn.OnLogin += sender =>
                {
                    Log.Debug("OnLogin event received");
                    conn.Send(new Presence(ShowType.chat, "Online") { Type = PresenceType.available });
                };

            conn.OnPresence += (sender, pres) =>
                {
                    Log.Debug("OnPresence event received, type: {0}", pres.Type.ToString());
                    if (pres.Type != PresenceType.subscribe) return;
                    Log.Debug("accepting subscription to: {0}", pres.From);
                    (new PresenceManager(conn)).ApproveSubscriptionRequest(pres.From);
                    SendHello(conn, pres.From);
                };

            var cc = new Dictionary<string, List<Message>>();
            conn.OnMessage += (sender, msg) =>
                {
                    if (string.IsNullOrEmpty(msg.Body)) return;
                    Log.Debug("new message from: {0}", msg.From);
                    lock (cc)
                    {
                        var key = msg.From.ToString();
                        if (!cc.ContainsKey(key))
                        {
                            cc[key] = new List<Message>();
                            var thr = new Thread(() => ClientThread(key, cc, conn));
                            thr.IsBackground = true;
                            thr.Start();
                        }
                        // чтобы в это время не отрабатывалось чтение очереди
                        lock (cc[key])
                        {
                            cc[key].Add(msg);
                        }
                    }
                };

            Log.Info("connecting to server...");
            conn.Open();

            while (!stop)
            {
                try
                {
                    Thread.Sleep(100);
                }
                catch(ThreadAbortException ex)
                {
                    stop = true;
                    Log.Info("main thread aborted.");
                    break;
                }
                catch(Exception e)
                {
                    stop = true;
                    Log.Error("exception: {0}", e.ToString());
                    break;
                }
            }

            Log.Info("closing connection to server.");
            conn.Close();
        }

        // процедура работает в разных потоках для разных клиентов
        private static void ClientThread(string ownId, Dictionary<string, List<Message>> clients, 
            XmppClientConnection conn)
        {
            Log.Info("client <{0}> connected...", ownId);
            var last = DateTime.Now;
            var stop = false;
            var cur = new List<Message>();
            var messages = clients[ownId];

            var ctx = new Context();
            var execs = cmds.Select(delegate(Type cmd)
                {
                    var instance = (ICmd) Activator.CreateInstance(cmd);
                    instance.Init(conn, ctx);
                    return instance;
                }).ToList();

            while (!stop)
            {
                try
                {
                    cur.Clear();
                    lock (messages) { cur.AddRange(messages); messages.Clear(); }
                    if (cur.Count > 0)
                    {
                        last = DateTime.Now;
                        foreach (var message in cur)
                        {
                            if (string.IsNullOrEmpty(message.Body)) continue;
                            var ok = execs.Any(exec => exec.Exec(message));
                            if (!ok) Send(conn, message.From, "error: unable to process command");
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                        // 10 минут простоя - удаляем поток
                        if (DateTime.Now.Subtract(last).TotalSeconds > 600) stop = true;
                    }
                }
                catch(ThreadAbortException ex)
                {
                    stop = true;
                }
                catch (ClientStopException ex)
                {
                    stop = true;
                }
                catch (Exception e)
                {
                    Log.Info("client thread {0} error: {1}", ownId,e.ToString());
                    stop = true;
                }
            }
            //
            lock (clients) { clients.Remove(ownId); }
            Log.Info("client {0} thread finished.", ownId);
        }

        //protected static Type[] cmds = new Type[]
        //    {
        //        typeof(HelpCmd),
        //        typeof(QuitCmd),
        //        typeof(AuthCmd),
        //        typeof(ListCmd),
        //        typeof(UpCmd)
        //    };

        protected static object cmdLocker = new object();
        protected static Type[] _xc = null;

        protected static Type[] cmds
        {
            get
            {
                if (_xc == null)
                {
                    lock (cmdLocker)
                    {
                        if (_xc == null) 
                            _xc = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(x => x.BaseType == typeof(BaseCmd))
                                .ToArray();
                    }
                }
                return _xc;
            }
        }

/*
        private static void ProcessMessage(XmppClientConnection conn, Message msg)
        {
            var context = GetContext(msg.From.ToString());
            if (string.IsNullOrEmpty(msg.Body)) return;
            context.RefreshTime();
            var err = true;
            foreach (var cmd in cmds)
            {
                var proc = (ICmd)Activator.CreateInstance(cmd);
                if (!proc.Exec(conn, msg, context)) continue;
                err = false;
                break;
            }
            if (err) Send(conn, msg.From, "unable to process command");
        }
*/

        protected static object slock = new object();

        private static void SendHello(XmppClientConnection conn, Jid to)
        {
            Send(conn, to, "hello, i'm powerbot. enter your auth by cmd: USER login password");
        }

        public static void Send(XmppClientConnection conn, Jid to, string txt)
        {
            var msg = new Message(to, txt) {Type = MessageType.chat};
            lock (slock) { conn.Send(msg); }
        }

        protected static Dictionary<string,Context> contexts = new Dictionary<string, Context>();

/*
        private static Context GetContext(string key)
        {
            if (!contexts.ContainsKey(key))
            {
                lock (contexts)
                {
                    if (!contexts.ContainsKey(key))
                    {
                        contexts[key] = new Context();
                    }
                }
            }
            var ret = contexts[key];
            //
            if (rnd.Next(30000)<1000)
            {
                lock (contexts)
                {
                    var list = new List<string>(contexts.Keys);
                    foreach (var xkey in list) if (contexts[xkey].IsOld) contexts.Remove(xkey);
                }
            }
            //
            return ret;
        }
*/

/*
        static readonly Random rnd = new Random();
*/

        private static XmppClientConnection CreateConnection()
        {
            return new XmppClientConnection
                {
                    Username = JRobotConfig.X.Username,
                    Password = JRobotConfig.X.Password,
                    Server = JRobotConfig.X.Server,
                    ConnectServer = JRobotConfig.X.ConnectionServer
                };
        }
    }

    class ClientStopException : Exception
    {
            
    }
}