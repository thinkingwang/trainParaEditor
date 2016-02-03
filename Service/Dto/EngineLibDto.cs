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
    public class EngineLibDto : Dto
    {
        private static EngineLib _engineLib;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static object _tempValue;
        private static string _key;

        private EngineLibDto(EngineLib tt)
        {
            _engineLib = tt;
        }

        public static void SetDgv(DataGridView dgv, BindingNavigator bn)
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

        private static void _dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表EngineLib的字段：" + column.HeaderText + "，修改前为：" + _tempValue + "，修改为：" +
                          _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_engineLib == null)
            {
                return;
            }
            try
            {
                ThrContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ThrContext.Detach(_engineLib);
                ThrContext.Database.ExecuteSqlCommand(string.Format("update engineLib set id='{0}' where id='{1}'",
                    _engineLib.id,
                    _key));
                //_engineLib.id = _tempValue.ToString();
                //ThrContext.Set<EngineLib>().Add(_engineLib);
                ThrContext.SaveChanges();
            }
            finally
            {
                GetAll(_engineLib.id);
            }
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var key = _dgv.Rows[e.RowIndex].Cells["id"].Value;
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            _key = _dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
            _engineLib = ThrContext.Set<EngineLib>().FirstOrDefault(m=>m.id == (string) key);
            //ThrContext.Set<EngineLib>().Remove(_engineLib);
            //ThrContext.SaveChanges();
        }

        public static IEnumerable<EngineLib> GetAll(string id = null)
        {
            var data = from d in ThrContext.Set<EngineLib>() select d;
            _source.DataSource = data.ToList();
            if (id != null)
            {
                var index = 0;
                foreach (var engineLib in data)
                {
                    if (engineLib.id == id)
                    {
                        break;
                    }
                    index++;
                }
                _dgv.Rows[index].Selected = true;
            }
            //索引9列的单元格的背景色为淡蓝色
            _dgv.Columns[0].DefaultCellStyle.BackColor = Color.Aqua;
            return data.ToList();
        }

        public static void CreateDataBase(IEnumerable<EngineLib> data, ModelContext context)
        {
            foreach (var thresholdse in data)
            {
                context.Set<EngineLib>().AddOrUpdate(thresholdse);
            }
            context.SaveChanges();
        }

        public static void Delete(int index)
        {
            var data = ThrContext.Set<EngineLib>().ToList().ElementAt(index);
            ThrContext.Set<EngineLib>().Remove(data);
            ThrContext.SaveChanges();
        }

        public static void NewEngineLib(EngineLib lib=null)
        {
            if (lib == null)
            {
                lib = new EngineLib {id = ""};
            }
            ThrContext.Set<EngineLib>().Add(lib);
            ThrContext.SaveChanges();
            GetAll(lib.id);
        }

        public static IQueryable<string> GetTrainTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<EngineLib>() select v.name).Distinct();
            return trainTypes;
        }
    }
}