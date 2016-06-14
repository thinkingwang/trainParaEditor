using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml;
using CommonModel;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Service.Dto
{
    public class Dto
    {
        public static bool ToCustomer = false;
        public static Logger Nlogger;
        public static ModelContext ThrContext;
        protected static readonly string connectStringBase;
        public static readonly ZoneConfig ServerConfig = new ZoneConfig();
        public static string connectionstring;
        public static readonly List<ZoneConfig> ServerList = new List<ZoneConfig>();

        static Dto()
        {
            ServerConfig.Type = "JC";
            ServerConfig.Ip = "127.0.0.1";
            ServerConfig.Name = "本地";
            ServerConfig.FlawOrRawPath = @"D:\tycho\data";
            ServerConfig.ProfilePath = @"D:\Tycho\外形数据";
            ConfigNlog();
            Document = new XmlDocument();
            //如果不存在 则从嵌入资源内读取 BlockSet.xml 
            var asm = Assembly.GetExecutingAssembly(); //读取嵌入式资源
            var sm = asm.GetManifestResourceStream("Service.config.xml");
            if (sm != null)
            {
                Document.Load(sm);
            }
            var nodes = Document.SelectNodes("/config/server");
            if (nodes != null)
            {
                foreach (XmlElement node1 in nodes)
                {
                    var config = new ZoneConfig
                    {
                        Name = node1.GetAttribute("name"),
                        Ip = node1.GetAttribute("ip"),
                        Type = node1.GetAttribute("type"),
                        FlawOrRawPath = node1.GetAttribute("flawOrRawPath"),
                        ProfilePath = node1.GetAttribute("profilePath"),
                        RemoteLoginName = node1.GetAttribute("remoteLoginName"),
                        RemotePwd = node1.GetAttribute("remotePwd")
                    };
                    ServerList.Add(config);
                }
            }

            if (File.Exists("config.xml"))
            {
                Document.Load("config.xml");
                var node = Document.SelectSingleNode("/config/server") as XmlElement;
                if (node != null)
                {
                    var config = new ZoneConfig
                    {
                        Name = node.GetAttribute("name"),
                        Ip = node.GetAttribute("ip"),
                        Type = node.GetAttribute("type"),
                        FlawOrRawPath = node.GetAttribute("flawOrRawPath"),
                        ProfilePath = node.GetAttribute("profilePath"),
                        RemoteLoginName = node.GetAttribute("remoteLoginName"),
                        RemotePwd = node.GetAttribute("remotePwd")
                    };
                    ServerConfig = config;
                }
            }
            else
            {
                ZoneConfig server = null;
                //获取本地的IP地址
                foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                    {
                        server = ServerList.FirstOrDefault(m => m.Ip.Equals(ipAddress.ToString()));
                        if (server != null)
                        {
                            break;
                        }
                    }
                }
                if (server == null)
                {
                    server = ServerList.FirstOrDefault(m => m.Ip.Equals("127.0.0.1"));
                }
                if (server != null)
                {
                    ServerConfig = server;
                }
            }
            connectionstring = string.Format(
                "data source={0};initial catalog=tycho_kc;persist security info=True;user id=sa;password=sa123;MultipleActiveResultSets=True;App=EntityFramework",
                ServerConfig.Ip);
            connectStringBase = string.Format(
                "Data Source={0};Initial Catalog=BaseWhprP807;Persist Security Info=True;User ID=sa;Password=sa123;multipleactiveresultsets=true",
                ServerConfig.Ip);
            
            //Context = new DCContext(ip);
        }


        public static XmlDocument Document { get; private set; }

        private static void ConfigNlog()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();
            // Step 2. Create targets and add them to the configuration 
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            // Step 3. Set target properties 
            fileTarget.FileName = @"D:\Tycho_log\${date:format=yyyy}\${date:format=yyyyMMdd}\参数修改器\${shortdate}.log";
            fileTarget.Layout = "${longdate} ${message}";
            // Step 4. Define rules 
            var rule2 = new LoggingRule("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule2);
            // Step 5. Activate the configuration 
            LogManager.Configuration = config;
            // Example usage 
            Nlogger = LogManager.GetLogger("Example");
        }
    }
}