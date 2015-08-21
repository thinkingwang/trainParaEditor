using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using JCModel;

namespace Service.Dto
{
    public class WheelPosDto:Dto
    {
        private static WheelPos _wheelPos;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
         private WheelPosDto(WheelPos tt)
        {
            _wheelPos = tt;
        }

        public static void SetDgv(DataGridView dgv, BindingNavigator bn)
        {

            _dgv = dgv;
            _source = new BindingSource();
            _bn = bn;
            _dgv.DataSource = _source;
            _bn.BindingSource = _source;
            _dgv.CellBeginEdit += _dgv_CellBeginEdit;
            _dgv.CellEndEdit += _dgv_CellEndEdit;
        }
       
         private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
         {
             if (_wheelPos == null ||
                 !_dgv.Columns[e.ColumnIndex].Name.Equals("posNo"))
             {
                 return;
             }
             ThrContext.Set<WheelPos>().Add(_wheelPos);
             ThrContext.SaveChanges();
             GetAll();
         }

         static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
         {
             if (_dgv.Columns[e.ColumnIndex].Name.Equals("posNo"))
             {
                 _wheelPos = ThrContext.Set<WheelPos>().ToList().ElementAt(e.RowIndex);
                 ThrContext.Set<WheelPos>().Remove(_wheelPos);
                 ThrContext.SaveChanges();
             }
         }

        public static IEnumerable<WheelPos> GetAll()
         {
             var data = from d in ThrContext.Set<WheelPos>() select d;
             return data.ToList();
        }
        public static IEnumerable<string> GetCrhWheelTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<WheelPos>() select v.trainType).Distinct();
            return trainTypes;
        }

        public static void GetCrhWheel(string trainType)
        {
            var data = from v in ThrContext.Set<WheelPos>()
                       where v.trainType == trainType
                select v;
            _source.DataSource = data.ToList();
            //索引9列的单元格的背景色为淡蓝色
            _dgv.Columns[0].DefaultCellStyle.BackColor = Color.Aqua;
        }

        public static void Delete(string trainType)
        {
            var data = from v in ThrContext.Set<WheelPos>()
                       where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                ThrContext.Set<WheelPos>().Remove(crh);
            }
            ThrContext.SaveChanges();
            Nlogger.Trace("删除表WheelPosDto指定车型");
        }

        public static void Delete(string trainType, byte posNo)
        {
            var data = from v in ThrContext.Set<WheelPos>()
                       where v.trainType == trainType && v.posNo == posNo 
                select v;
            foreach (var crh in data)
            {
                ThrContext.Set<WheelPos>().Remove(crh);
            }
            ThrContext.SaveChanges();
        }


        public static void Add(string trainType)
        {
            var item = ThrContext.Set<WheelPos>().Where(m => m.trainType.Equals(trainType)).OrderByDescending(m => m.posNo).FirstOrDefault();
            if (item != null)
            {
                var element = DeepCopy(item);
                element.trainType = trainType;
                element.posNo = (byte)(item.posNo + 1);
                ThrContext.Set<WheelPos>().Add(element);
            }
            else
            {
                ThrContext.Set<WheelPos>().Add(new WheelPos()
                {
                    trainType = trainType,
                    posNo = 0
                });
            }
            ThrContext.SaveChanges();
        }

        public static void Copy(string trainType, string name)
        {
            var data = from v in ThrContext.Set<WheelPos>()
                       where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                var holds = DeepCopy(crh);
                holds.trainType = name;
                ThrContext.Set<WheelPos>().Add(holds);
            }
            ThrContext.SaveChanges();
            Nlogger.Trace("复制指定车型数据");
        }
         public static void CreateDataBase(IEnumerable<WheelPos> data)
        {
            foreach (var thresholdse in data)
            {
                ThrContext.Set<WheelPos>().AddOrUpdate(thresholdse);
            }
            ThrContext.SaveChanges();
            _source.DataSource = GetAll();
        }
    }
}