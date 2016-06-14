using System.Collections.Generic;

namespace CommonModel
{
    public class TrainTypesItem
    {
        public TrainTypesItem(string headStr, string tailStr, IEnumerable<string> trainTypes)
        {
            HeadStr = headStr;
            TailStr = tailStr;
            TrainTypes = trainTypes;
        }

        public string HeadStr { get; set; }
        public string TailStr { get; set; }
        public IEnumerable<string> TrainTypes { get; set; }
    }
}