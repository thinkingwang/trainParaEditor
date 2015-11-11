using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CommonModel;
using CommonModel.Common;
using DCModel;
using JCModel;
using NLog;
using NLog.Config;
using NLog.Targets;
using trainTypeEditor;

namespace Service.Dto
{
    public class Dto
    {
        public static Logger Nlogger;
        public static ModelContext ThrContext;

        protected static Entities BwContext
        {
            get { return _bwContext ?? (_bwContext = new Entities(connectStringBase)); }
        }

        private static string connectStringBase;
        public static readonly ZoneConfig ServerConfig = new ZoneConfig();
        public static string serverConnnectioString;
        public static string connectionstring;
        public static string CenterConnectionstring;
        public static XmlDocument Document { get; private set; }
        public static AutoResetEvent AtEvent = new AutoResetEvent(false);
        public static readonly List<ZoneConfig> ServerList = new List<ZoneConfig>();
        private static Entities _bwContext;

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
            Assembly asm = Assembly.GetExecutingAssembly(); //读取嵌入式资源
            Stream sm = asm.GetManifestResourceStream("Service.config.xml");
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
                        Name = node1.Attributes["name"].Value,
                        Ip = node1.Attributes["ip"].Value,
                        Type = node1.Attributes["type"].Value,
                        FlawOrRawPath = node1.Attributes["flawOrRawPath"].Value,
                        ProfilePath = node1.Attributes["profilePath"].Value
                    };
                    ServerList.Add(config);
                }
            }

            if (File.Exists("config.xml"))
            {
                Document.Load("config.xml");
            var node = Document.SelectSingleNode("/config/server");
                if (node != null)
                {
                    var config = new ZoneConfig
                    {
                        Name = node.Attributes["name"].Value,
                        Ip = node.Attributes["ip"].Value,
                        Type = node.Attributes["type"].Value,
                        FlawOrRawPath = node.Attributes["flawOrRawPath"].Value,
                        ProfilePath = node.Attributes["profilePath"].Value
                    };
                    ServerConfig = config;
                }
            }
            else
            {
                ZoneConfig server = null;
                //获取本地的IP地址
                foreach (System.Net.IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
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
            CenterConnectionstring =
                string.Format(
                    "Data Source={0};Initial Catalog=aspnet-DeviceMonitor-20141104153122;Persist Security Info=True;User ID=sa;Password=sa123;multipleactiveresultsets=true",
                    ServerConfig.Ip);
            connectStringBase = string.Format(
                "metadata=res://*/BaseWhpr.csdl|res://*/BaseWhpr.ssdl|res://*/BaseWhpr.msl;provider=System.Data.SqlClient;provider=System.Data.SqlClient;provider connection string=\"data source={0};initial catalog=BaseWhprP807;persist security info=True;user id=sa;password=sa123;multipleactiveresultsets=True;App=EntityFramework\"",
                ServerConfig.Ip);
            //Context = new DCContext(ip);
        }



        private static void ConfigNlog()
        {
            // Step 1. Create configuration object 
            LoggingConfiguration config = new LoggingConfiguration();
            // Step 2. Create targets and add them to the configuration 
            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            // Step 3. Set target properties 
            fileTarget.FileName = @"D:\Tycho_log\${date:format=yyyy}\${date:format=yyyyMMdd}\参数修改器\${shortdate}.log";
            fileTarget.Layout = "${longdate} ${message}";
            // Step 4. Define rules 
            LoggingRule rule2 = new LoggingRule("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule2);
            // Step 5. Activate the configuration 
            LogManager.Configuration = config;
            // Example usage 
            Nlogger = LogManager.GetLogger("Example");
        }


    }
    public static class DeepCopyHelper
    {
        public static object Copy(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //值类型  
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            //引用类型   
            else
            {
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象   
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, Copy(fieldValue));
                        }

                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
                            }
                        }

                    }
                }
            }
            return targetDeepCopyObj;
        }
    }
}