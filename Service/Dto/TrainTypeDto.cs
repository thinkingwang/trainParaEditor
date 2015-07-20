using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommonModel;

namespace Service.Dto
{
    public class TrainTypeDto : Dto
    {
        private readonly TrainType _trainType;
        private TrainTypeDto(TrainType tt)
        {
            _trainType = tt;
        }
        public static IEnumerable<TrainTypeDto> GetAll()
        {
            var result = new List<TrainTypeDto>();
            foreach (var thresholdse in thrContext.Set<TrainType>())
            {
                result.Add(new TrainTypeDto(thresholdse));
            }
            return result;
        }

        public static void Delete(int index)
        {
            var data = thrContext.Set<TrainType>().ToList().ElementAt(index);
            thrContext.Set<TrainType>().Remove(data);
            thrContext.SaveChanges();
        }
        public static void NewTrainType()
        {
            thrContext.Set<TrainType>().Add(new TrainType() { trainType1 = "", format = "", trainNoFrom = 0, trainNoTo = 0 });
            thrContext.SaveChanges();
        }


        [DisplayName(@"车型")]
        public string trainType1
        {
            get { return _trainType.trainType1; }
            set
            {
                _trainType.trainType1 = value;
                thrContext.SaveChanges();
            }
        }

        [DisplayName(@"车号开始数字")]
        public int trainNoFrom
        {
            get { return _trainType.trainNoFrom; }
            set
            {
                thrContext.Set<TrainType>().Remove(_trainType);
                thrContext.SaveChanges();
                _trainType.trainNoFrom = value;
                thrContext.Set<TrainType>().Add(_trainType);
                thrContext.SaveChanges();
            }
        }

        [DisplayName(@"车号结束数字")]
        public int trainNoTo
        {
            get { return _trainType.trainNoTo; }
            set
            {
                thrContext.Set<TrainType>().Remove(_trainType);
                thrContext.SaveChanges();
                _trainType.trainNoTo = value;
                thrContext.Set<TrainType>().Add(_trainType);
                thrContext.SaveChanges();
            }
        }

        [DisplayName(@"车型显示格式")]
        public string format
        {
            get { return _trainType.format; }
            set
            {
                _trainType.format = value;
                thrContext.SaveChanges();
            }
        }

    }
}