using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using JCModel;

namespace Service.Dto
{
    public class EngineLibDto: Dto 
    {
        private static EngineLib _engineLib;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static object _tempValue;
        private EngineLibDto(EngineLib tt)
        {
            _engineLib = tt;
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
            Nlogger.Trace("编辑表EngineLib的字段：" + column.HeaderText + "，修改前为：" + _tempValue + "，修改为：" + _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_engineLib == null)
            {
                return;
            }
            ThrContext.Set<EngineLib>().Add(_engineLib);
            ThrContext.SaveChanges();
            GetAll();
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            _engineLib = ThrContext.Set<EngineLib>().ToList().ElementAt(e.RowIndex);
            ThrContext.Set<EngineLib>().Remove(_engineLib);
            ThrContext.SaveChanges();
        }

        public static IEnumerable<EngineLib> GetAll()
        {
            var data = from d in ThrContext.Set<EngineLib>() select d;
            _source.DataSource = data.ToList();
            return data.ToList();
        }

        public static void CreateDataBase(IEnumerable<EngineLib> data,ModelContext context)
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
        public static void NewEngineLib()
        {
            ThrContext.Set<EngineLib>().Add(new EngineLib(){id=""});
            ThrContext.SaveChanges();
        }

       
    }
}