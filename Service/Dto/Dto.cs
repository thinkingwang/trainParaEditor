using System;
using System.IO;
using System.Reflection;
using System.Xml;
using CommonModel;
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
        protected static readonly ModelContext thrContext;
        public static readonly Entities bwContext;
        public static string ip = "127.0.0.1";
        public static string serverType = "DC";
        public static XmlDocument Document { get; private set; }
        static Dto()
        {
            if (File.Exists("config.xml"))
            {
                Document = new XmlDocument();
                Document.Load("config.xml");
                var node = Document.SelectSingleNode("/config/add[@key='thresholdsContext']/@value");
                if (node != null)
                {
                    ip = node.Value;
                }
                node = Document.SelectSingleNode("/config/add[@key='serverType']/@value");
                if (node != null)
                {
                    serverType = node.Value;
                }
            }
            string connectString = string.Format(
                "data source={0};initial catalog=tycho_kc;persist security info=True;user id=sa;password=sa123;MultipleActiveResultSets=True;App=EntityFramework",
                ip);
            switch (serverType)
            {
                case "DC":
                    thrContext = new DCContext(connectString);
                    break;
                case "JC":
                    thrContext = new JCContext(connectString);
                    break;
            }

            string connectStringBase = string.Format(
                "metadata=res://*/BaseWhpr.csdl|res://*/BaseWhpr.ssdl|res://*/BaseWhpr.msl;provider=System.Data.SqlClient;provider=System.Data.SqlClient;provider connection string=\"data source={0};initial catalog=BaseWhprP807;persist security info=True;user id=sa;password=sa123;multipleactiveresultsets=True;App=EntityFramework\"",
                ip);
            //Context = new DCContext(ip);
            bwContext = new Entities(connectStringBase);
            ConfigNlog();
        }
        /// <summary>
        /// 深拷贝功能函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected static T DeepCopy<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        private static void ConfigNlog()
        {
            // Step 1. Create configuration object 
            LoggingConfiguration config = new LoggingConfiguration();
            // Step 2. Create targets and add them to the configuration 
            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            // Step 3. Set target properties 
            fileTarget.FileName = "${basedir}/logs/${shortdate}.log";
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
}