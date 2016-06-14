using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonModel;

namespace Service.Dto
{
    public class CRH_wheelDto : Dto
    {
        private static CRH_wheel _crhWheel;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static object _tempValue;

        private CRH_wheelDto()
        {
        }

        public static void SetDgv(DataGridView dgv, BindingNavigator bn)
        {
            _dgv = dgv;
            _source = new BindingSource();
            _bn = bn;
            _dgv.DataSource = _source;
            _bn.BindingSource = _source;
            _dgv.CellBeginEdit += _dgv_CellBeginEdit;
            _dgv.CellValueChanged += _dgv_CellValueChanged;
            _dgv.CellEndEdit += _dgv_CellEndEdit;
        }

        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_crhWheel == null)
            {
                return;
            }
            ThrContext.Set<CRH_wheel>().Add(_crhWheel);
            ThrContext.SaveChanges();
            GetAll();
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            _crhWheel = ThrContext.Set<CRH_wheel>().ToList().ElementAt(e.RowIndex);
            ThrContext.Set<CRH_wheel>().Remove(_crhWheel);
            ThrContext.SaveChanges();
        }

        private static void _dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表CRH_wheel的字段：" + column.HeaderText + "，车型为：" +
                          _dgv.Rows[e.RowIndex].Cells["trainType"].Value + ",修改前为：" + _tempValue + "，修改为：" +
                          _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        public static IEnumerable<CRH_wheel> GetAll()
        {
            var data = from d in ThrContext.Set<CRH_wheel>() select d;
            return data.ToList();
        }

        public static IEnumerable<string> GetCrhWheelTypes()
        {
            //获取所有车型
            var trainTypes = ThrContext.Database.SqlQuery<string>("select distinct trainType from CRH_wheel");
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
                where v.trainType == trainType && v.axleNo == axelNo && v.wheelNo == wheelNo
                select v;
            foreach (var crh in data)
            {
                ThrContext.Set<CRH_wheel>().Remove(crh);
            }
            ThrContext.SaveChanges();
        }

        public static void Add(string trainType)
        {
            var item =
                ThrContext.Set<CRH_wheel>()
                    .Where(m => m.trainType.Equals(trainType))
                    .OrderByDescending(m => m.axleNo)
                    .FirstOrDefault();
            if (item != null)
            {
                var element = item.Copy() as CRH_wheel;
                if (element == null)
                {
                    return;
                }
                element.axleNo = (byte) (item.axleNo + 1);
                element.wheelNo = 0;
                ThrContext.Set<CRH_wheel>().Add(element);
            }
            else
            {
                ThrContext.Set<CRH_wheel>().Add(new CRH_wheel
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
            var dataSource = from v in ThrContext.Set<CRH_wheel>()
                where v.trainType == name
                select v;
            foreach (var crh in dataSource)
            {
                ThrContext.Set<CRH_wheel>().Remove(crh);
            }
            ThrContext.SaveChanges();
            foreach (var crh in data)
            {
                var holds = crh.Copy() as CRH_wheel;
                if (holds == null)
                {
                    continue;
                }
                holds.trainType = name;
                ThrContext.Set<CRH_wheel>().Add(holds);
            }
            ThrContext.SaveChanges();
        }

        public static void CreateDataBase(IEnumerable<CRH_wheel> data, ModelContext context)
        {
            foreach (var thresholdse in data)
            {
                context.Set<CRH_wheel>().AddOrUpdate(thresholdse);
            }
            context.SaveChanges();
        }
    }
}