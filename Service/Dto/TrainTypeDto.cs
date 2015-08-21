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
        }


        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_trainType == null ||
                (!_dgv.Columns[e.ColumnIndex].Name.Equals("trainNoFrom") &&
                 !_dgv.Columns[e.ColumnIndex].Name.Equals("trainNoTo")))
            {
                return;
            }
            ThrContext.Set<TrainType>().Add(_trainType);
            ThrContext.SaveChanges();
            GetAll();
        }

        static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_dgv.Columns[e.ColumnIndex].Name.Equals("trainNoFrom") || _dgv.Columns[e.ColumnIndex].Name.Equals("trainNoTo"))
            {
                _trainType = ThrContext.Set<TrainType>().ToList().ElementAt(e.RowIndex);
                ThrContext.Set<TrainType>().Remove(_trainType);
                ThrContext.SaveChanges();
            }
        }

        public static IEnumerable<TrainType> GetAll()
        {
            var data = from d in ThrContext.Set<TrainType>() select d;
            _source.DataSource = data.ToList();
            return data.ToList();
        }

        public static void CreateDataBase(IEnumerable<TrainType> data)
        {
            foreach (var thresholdse in data)
            {
                ThrContext.Set<TrainType>().AddOrUpdate(thresholdse);
            }
            ThrContext.SaveChanges();
            _source.DataSource = GetAll();
        }
        public static void Delete(int index)
        {
            var data = ThrContext.Set<TrainType>().ToList().ElementAt(index);
            ThrContext.Set<TrainType>().Remove(data);
            ThrContext.SaveChanges();
        }
        public static void NewTrainType()
        {
            var random = new Random();
            ThrContext.Set<TrainType>().Add(new TrainType() { trainType1 = "CRH", format = "CRH", trainNoFrom = random.Next(0, 10000), trainNoTo = random.Next(0, 10000) });
            ThrContext.SaveChanges();
        }
       
    }
}