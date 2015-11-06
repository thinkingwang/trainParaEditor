using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using DCModel;
using TCModel;

namespace Service.Dto
{
    public class CRH_CarDto : Dto
    {
        private static CRH_Car _crhCar;
        private CRH_Car _crhCarTemp;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static object _tempValue;
        private static string _trainType;
        private static ComboBox comboBox2;
        private static int comboBoxColumnIndex1 = 2; // DataGridView的首列
        private CRH_CarDto(CRH_Car tt)
        {
            _crhCarTemp = tt;
        }


        public static void SetDgv(DataGridView dgv)
        {

            comboBox2 = new ComboBox();
            _dgv = dgv;
            _source = new BindingSource();
            _dgv.DataSource = _source;
            _dgv.CellBeginEdit += _dgv_CellBeginEdit;
            _dgv.CellEndEdit += _dgv_CellEndEdit;
            _dgv.CellValueChanged += _dgv_CellValueChanged;
            _dgv.CellEnter += CRH_Car_dgv_CellEnter;
            _dgv.CellLeave += CRH_Car_dgv_CellLeave;
            _dgv.Controls.Add(comboBox2);
            comboBox2.Items.AddRange(new object[] { "动车", "拖车" });
            comboBox2.AutoCompleteMode = AutoCompleteMode.Suggest; //输入提示
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox2.Visible = false;
        }

        public static IQueryable<string> GetCRH_CarTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<CRH_Car>() select v.trainType).Distinct();
            return trainTypes;
        }
        static void _dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表CRH_Car的字段：" + column.HeaderText + "，修改前为：" + _tempValue + "，修改为：" + _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }
        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_crhCar == null)
            {
                return;
            }
            ThrContext.Set<CRH_Car>().Add(_crhCar);
            ThrContext.SaveChanges();
            GetCRH_Cars(_trainType);
        }

        private static void ComfirmComboBoxValue(ComboBox com, object cellValue)
        {
            com.SelectedIndex = -1;
            if (cellValue == null)
            {
                com.Text = "";
                return;
            }
            com.Text = cellValue.ToString();
            foreach (Object item in com.Items)
            {
                if ((String)item == cellValue.ToString())
                {
                    com.SelectedItem = item;
                }
            }
        }

        private static void CRH_Car_dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != comboBoxColumnIndex1)
            {
                return;
            }
            //此处cell即CurrentCell
            DataGridViewCell cell = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle rect = _dgv.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
            comboBox2.Location = rect.Location;
            comboBox2.Size = rect.Size;
            ComfirmComboBoxValue(comboBox2, cell.Value);
            comboBox2.Visible = true;

        }

        private static void CRH_Car_dgv_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != comboBoxColumnIndex1)
            {
                return;
            }
            //此处cell不为CurrentCell
            DataGridViewCell cell = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Value = comboBox2.Text;
            comboBox2.Visible = false;
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            _trainType = _dgv.Rows[e.RowIndex].Cells["trainType"].Value.ToString();
            _crhCar = ThrContext.Set<CRH_Car>().ToList().ElementAt(e.RowIndex);
            ThrContext.Set<CRH_Car>().Remove(_crhCar);
            ThrContext.SaveChanges();
        }

        public static IQueryable<CRH_Car> GetAll()
        {
            var data = (from d in ThrContext.Set<CRH_Car>() select d).ToList();
            _source.DataSource = data;
            return data.AsQueryable();
        }


        /// <summary>
        /// 获得门限表中指定车型的所有数据
        /// </summary>
        /// <param name="trainType"></param>
        /// <returns></returns>
        public static IQueryable<CRH_Car> GetCRH_Cars(string trainType)
        {
            if (string.IsNullOrEmpty(trainType))
            {
                _dgv.Rows.Clear();
                return null;
            }
            var data = from v in ThrContext.Set<CRH_Car>()
                                         where v.trainType == trainType
                                         select v;
            if (data.Any())
            {
                _source.DataSource = data.ToList().Select(m => new CRH_CarDto(m));
                //索引0,1,2列的单元格的背景色为淡蓝色
                _dgv.Columns[0].DefaultCellStyle.BackColor = Color.Aqua;
                //索引1列的单元格的背景色为淡蓝色
                _dgv.Columns[1].DefaultCellStyle.BackColor = Color.Aqua;
                return data;
            }
            return null;
        }

        public static void CreateDataBase(IQueryable<CRH_Car> data, ModelContext context)
        {
            foreach (var thresholdse in data)
            {
                context.Set<CRH_Car>().AddOrUpdate(thresholdse);
            }
            context.SaveChanges();
        }
        public static void Clear()
        {
            _dgv.Rows.Clear();
        }

        /// <summary>
        /// 在门限表中删除指定车型
        /// </summary>
        /// <param name="trainType"></param>
        public static void Delete(string trainType)
        {
            if (string.IsNullOrEmpty(trainType))
            {
                return;
            }
            var data = from v in ThrContext.Set<CRH_Car>()
                       where v.trainType == trainType
                       select v;
            foreach (var thresholdse in data)
            {
                ThrContext.Set<CRH_Car>().Remove(thresholdse);
            }
            ThrContext.SaveChanges();
            Nlogger.Trace("在车厢型号表中删除指定车型");
        }
        public static void NewCRH_Car(string trainType,string num)
        {
            int count;
            if (int.TryParse(num, out count))
            {
                for (int i = 0; i < count; i++)
                {
                    var car = new CRH_Car
                    {
                        trainType = trainType,
                        powerType = 1,
                        carPos = (byte) i
                    };
                    ThrContext.Set<CRH_Car>().AddOrUpdate(car);
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName("车型")]
        [ReadOnly(true)]
        public string trainType
        {
            get { return _crhCarTemp.trainType; }
            set { _crhCarTemp.trainType = value; }
        }

        [DisplayName("车厢序号")]
        [ReadOnly(true)]
        public byte carPos
        {
            get
            {
                return (byte)(_crhCarTemp.carPos + 1);
            }
            set
            {
                _crhCarTemp.carPos = value;
            }
        }
       

        [DisplayName("车厢类型")]
        public string powerType
        {
            get
            {
                if (_crhCarTemp.powerType == 0)
                {
                    return "拖车";
                }
                return "动车";
            }
            set
            {
                if (value == "动车")
                {
                    _crhCarTemp.powerType = 1;
                }
                else
                {
                    _crhCarTemp.powerType = 0;
                }
                ThrContext.Set<CRH_Car>().AddOrUpdate(_crhCarTemp);
                ThrContext.SaveChanges();
            }
        }
    }
}