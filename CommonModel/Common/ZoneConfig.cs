namespace CommonModel.Common
{
    /// <summary>
    /// 站点配置信息
    /// </summary>
    public class ZoneConfig
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 站点IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 站点类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 站点探伤和擦伤的原始文件路径
        /// </summary>
        public string FlawOrRawPath { get; set; }

        /// <summary>
        /// 站点外形原始文件路径
        /// </summary>
        public string ProfilePath { get; set; }
    }
}