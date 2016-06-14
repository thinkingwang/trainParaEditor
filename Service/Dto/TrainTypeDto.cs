using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using JCModel;

namespace Service.Dto
{
    public class TrainTypeDto : Dto
    {
        private static TrainType _trainType;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static object _tempValue;
        private TrainTypeDto(TrainType tt)
        {
            _trainType = tt;
        }


        public static void SetDgv(DataGridView dgv,  BindingNavigator bn)
        {

            _dgv = dgv;
            _source = new BindingSource();
            _bn = bn;
            _bn.BindingSource = _source;
            _dgv.DataSource = _source;
            _dgv.CellBeginEdit += _dgv_CellBeginEdit;
            _dgv.CellEndEdit += _dgv_CellEndEdit;
            _dgv.CellValueChanged += _dgv_CellValueChanged;
        }

        static void _dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表TrainType的字段：" + column.HeaderText + "，修改前为：" + _tempValue + "，修改为：" + _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_trainType == null)
            {
                return;
            }
            ThrContext.Set<TrainType>().Add(_trainType);
            ThrContext.SaveChanges();
            GetAll();
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            _trainType = ThrContext.Set<TrainType>().OrderBy(m=>m.trainNoFrom).Skip(e.RowIndex).FirstOrDefault();
            ThrContext.Set<TrainType>().Remove(_trainType);
            ThrContext.SaveChanges();
        }

        public static IEnumerable<TrainType> GetAll(int? from=null,int? to=null)
        {
            var data = from d in ThrContext.Set<TrainType>() orderby d.trainNoFrom select d;
            _source.DataSource = data.ToList();
            if (from != null&&to!=null)
            {
                var index = Enumerable.TakeWhile(data, engineLib => engineLib.trainNoFrom != @from || engineLib.trainNoTo != to).Count();
                _dgv.Rows[index].Selected = true;
            }
            return data.ToList();
        }

        public static void CreateDataBase(IEnumerable<TrainType> data,ModelContext context)
        {
            foreach (var thresholdse in data)
            {
                context.Set<TrainType>().AddOrUpdate(thresholdse);
            }
            context.SaveChanges();
        }
        public static void Delete(int index)
        {
            var data = ThrContext.Set<TrainType>().ToList().ElementAt(index);
            ThrContext.Set<TrainType>().Remove(data);
            ThrContext.SaveChanges();
        }
        public static void NewTrainType(TrainType type=null)
        {
            if (type == null)
            {
                var random = new Random();
                type = new TrainType() { trainType = "CRH", format = "CRH", trainNoFrom = random.Next(0, 10000), trainNoTo = random.Next(0, 10000) };
            }
            ThrContext.Set<TrainType>().Add(type);
            ThrContext.SaveChanges();
            GetAll(type.trainNoFrom,type.trainNoTo);
        }

        public static IQueryable<string> GetTrainTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<TrainType>() select v.trainType).Distinct();
            return trainTypes;
        }

       
    }
}