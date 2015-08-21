using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using CommonModel.Common;

namespace Service.Dto
{
    public class CRH_wheelDto : Dto
    {
        private readonly CRH_wheel _crhWheel;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;

        private CRH_wheelDto(CRH_wheel crh)
        {
            _crhWheel = crh;
        }

        public static void SetDgv(DataGridView dgv,  BindingNavigator bn)
        {
            _dgv = dgv;
            _source = new BindingSource();
            _bn = bn;
            _dgv.DataSource = _source;
            _bn.BindingSource = _source;
        }

        public static IEnumerable<CRH_wheel> GetAll()
        {
            var data = from d in ThrContext.Set<CRH_wheel>() select d;
            return data.ToList();
        }

        public static IEnumerable<string> GetCrhWheelTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<CRH_wheel>() select v.trainType).Distinct();
            return trainTypes;
        }

        public static void GetCrhWheel(string trainType)
        {
            var data = from v in ThrContext.Set<CRH_wheel>()
                where v.trainType == trainType
                select v;
            _source.DataSource = data.ToList().OrderBy(m => m.axleNo).ThenBy(m => m.wheelPos);
            if (_dgv.Columns.Count == 0)
            {
                return;
            }
            //索引9列的单元格的背景色为淡蓝色
            _dgv.Columns[0].DefaultCellStyle.BackColor = Color.Aqua;
            //索引9列的单元格的背景色为淡蓝色
            _dgv.Columns[1].DefaultCellStyle.BackColor = Color.Aqua;
            //索引9列的单元格的背景色为淡蓝色
            _dgv.Columns[2].DefaultCellStyle.BackColor = Color.Aqua;
        }

        public static void Delete(string trainType)
        {
            var data = from v in ThrContext.Set<CRH_wheel>()
                where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                ThrContext.Set<CRH_wheel>().Remove(crh);
            }
            ThrContext.SaveChanges();
        }

        public static void Delete(string trainType, byte axelNo, byte wheelNo)
        {
            var data = from v in ThrContext.Set<CRH_wheel>()
                       where v.trainType == trainType&&v.axleNo ==axelNo && v.wheelNo == wheelNo
                       select v;
            foreach (var crh in data)
            {
                ThrContext.Set<CRH_wheel>().Remove(crh);
            }
            ThrContext.SaveChanges();
        }


        public static void Add(string trainType)
        {
            var item = ThrContext.Set<CRH_wheel>().Where(m => m.trainType.Equals(trainType)).OrderByDescending(m => m.axleNo).FirstOrDefault();
            if (item != null)
            {
                var element = DeepCopy(item);
                element.axleNo = (byte) (item.axleNo + 1);
                element.wheelNo = 0;
                ThrContext.Set<CRH_wheel>().Add(element);
            }
            else
            {
                ThrContext.Set<CRH_wheel>().Add(new CRH_wheel()
                {
                    trainType = trainType,
                    axleNo = 0,
                    wheelNo = 0
                });
            }
            ThrContext.SaveChanges();
        }
        public static void Copy(string trainType, string name)
        {
            var data = from v in ThrContext.Set<CRH_wheel>()
                where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                var holds = DeepCopy(crh);
                holds.trainType = name;
                ThrContext.Set<CRH_wheel>().Add(holds);
            }
            ThrContext.SaveChanges();
        }

        public static void CreateDataBase(IEnumerable<CRH_wheel> data)
        {
            foreach (var thresholdse in data)
            {
                ThrContext.Set<CRH_wheel>().AddOrUpdate(thresholdse);
            }
            ThrContext.SaveChanges();
            _source.DataSource = GetAll();
        }
    }
}