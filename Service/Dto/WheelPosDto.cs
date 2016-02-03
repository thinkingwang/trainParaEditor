using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using JCModel;

namespace Service.Dto
{
    public class WheelPosDto : Dto
    {
        private static WheelPos _wheelPos;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static object _tempValue;
        private static byte _tempPos;

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
            _dgv.CellValueChanged += _dgv_CellValueChanged;
        }

        private static void _dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表WheelPos的字段：" + column.HeaderText + "，车型为：" +
                          _dgv.Rows[e.RowIndex].Cells["trainType"].Value + "，修改前为：" + _tempValue + "，修改为：" +
                          _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var temp = ThrContext.Set<WheelPos>().FirstOrDefault(m => m.posNo == _wheelPos.posNo);
            if (temp != null && _tempPos != _wheelPos.posNo)
            {
                _wheelPos.posNo = _tempPos;
                MessageBox.Show("修改到目标轮位失败，原因是目标轮位已存在");
            }
            ThrContext.Set<WheelPos>().Add(_wheelPos);
            ThrContext.SaveChanges();
            GetAll();
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var trainType = _dgv.Rows[e.RowIndex].Cells["trainType"].Value.ToString();
            _tempPos = Convert.ToByte(_dgv.Rows[e.RowIndex].Cells["posNo"].Value);
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            _wheelPos =
                ThrContext.Set<WheelPos>().FirstOrDefault(a => a.trainType.Equals(trainType) && a.posNo == _tempPos);
            ThrContext.Set<WheelPos>().Remove(_wheelPos);
            ThrContext.SaveChanges();
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
            var item =
                ThrContext.Set<WheelPos>()
                    .Where(m => m.trainType.Equals(trainType))
                    .OrderByDescending(m => m.posNo)
                    .FirstOrDefault();
            if (item != null)
            {
                var element = item.Copy() as WheelPos;
                if (element == null)
                {
                    return;
                }
                element.trainType = trainType;
                element.posNo = (byte) (item.posNo + 1);
                ThrContext.Set<WheelPos>().Add(element);
            }
            else
            {
                ThrContext.Set<WheelPos>().Add(new WheelPos
                {
                    trainType = trainType,
                    posNo = 0,
                    value = ""
                });
            }
            ThrContext.SaveChanges();
        }

        public static void Copy(string trainType, string name)
        {
            var data = from v in ThrContext.Set<WheelPos>()
                where v.trainType == trainType
                select v;
            var dataSource = from v in ThrContext.Set<WheelPos>()
                where v.trainType == name
                select v;
            foreach (var crh in dataSource)
            {
                ThrContext.Set<WheelPos>().Remove(crh);
            }
            ThrContext.SaveChanges();
            foreach (var crh in data)
            {
                var holds = crh.Copy() as WheelPos;
                if (holds == null)
                {
                    continue;
                }
                holds.trainType = name;
                ThrContext.Set<WheelPos>().Add(holds);
            }
            ThrContext.SaveChanges();
            Nlogger.Trace("复制指定车型数据");
        }

        public static void CreateDataBase(IEnumerable<WheelPos> data, ModelContext context)
        {
            foreach (var thresholdse in data)
            {
                context.Set<WheelPos>().AddOrUpdate(thresholdse);
            }
            context.SaveChanges();
        }

        public static IQueryable<string> GetTrainTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<WheelPos>() select v.trainType).Distinct();
            return trainTypes;
        }
    }
}