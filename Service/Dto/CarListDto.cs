using System;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonModel;

namespace Service.Dto
{
    public class CarListDto : Dto
    {
        private static CarList _carList;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static readonly string _picturePath = @"D:\tycho\data";
        private static object _tempValue;
        //private readonly Detect _detect;

        static CarListDto()
        {
            if (Document != null)
            {
                var node = Document.SelectSingleNode(string.Format("/config/add[@key='{0}']/@value", "PicturePath"));
                if (node != null)
                {
                    _picturePath = node.Value;
                }
            }
        }

        private CarListDto()
        {
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
            DataGridViewColumn column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表CarList的字段：" + column.HeaderText + "，修改前为：" + _tempValue + "，修改为：" +
                          _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_carList == null)
            {
                return;
            }
            ThrContext.AddCarList(_carList);
            ThrContext.SaveChanges();
            _source.DataSource = ThrContext.GetCarLists(_carList.testDateTime);
        }

        private static void _dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _tempValue = _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            var time = DateTime.Parse(_dgv.Rows[0].Cells["testDateTime"].Value.ToString());
            _carList = ThrContext.GetCarLists(time).ElementAt(e.RowIndex);
            ThrContext.RemoveCarList(_carList);
            ThrContext.SaveChanges();
        }

        private static BindingSource GetSource()
        {
            return _source ?? (_source = new BindingSource());
        }

        public static BindingNavigator GetBn(IContainer components)
        {
            if (_bn == null)
            {
                _bn = new BindingNavigator(components) {BindingSource = GetSource()};
            }
            return _bn;
        }

        /// <summary>
        ///     车厢列表增加新行
        /// </summary>
        /// <param name="testDateTime"></param>
        public static void Insert(DateTime testDateTime)
        {
            try
            {
                ThrContext.InsertCarList(testDateTime);
                ThrContext.SaveChanges();
                Nlogger.Trace("车厢列表增加新行");
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public static void CopyTo(string sourceText, string desText, bool overWriteTanShang = false,
            bool overWriteCaShang = false, bool overWriteProfile = false)
        {
            var source = DateTime.Parse(sourceText);
            var des = DateTime.Parse(desText);
            var detectSource = ThrContext.Database.SqlQuery<Detect>(string.Format("select * from Detect where testDateTime='{0}'",
                source.ToString("yyyy-MM-dd HH:mm:ss"))).ToList().FirstOrDefault();
            if (detectSource == null)
            {
                return;
            }
            CarNoRepair(source,des);
            var detect = ThrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(des));
            if (detect == null)
            {
                detect = detectSource.Copy() as Detect;
                if (detect == null)
                {
                    return;
                }
                detect.testDateTime = des;
                detect.outDateTime = des;
            }
            else
            {
                detect.engNum = detectSource.engNum;
            }
            if (overWriteTanShang)
            {
                TanRepair(sourceText, desText);
            }
            if (overWriteCaShang)
            {
                CaRepair(sourceText, desText);
            }
            if (overWriteProfile)
            {
                ProfileDetectResultDto.Repair(desText, sourceText);
            }
            ThrContext.Set<Detect>().AddOrUpdate(detect);
            ThrContext.SaveChanges();

            var result = MessageBox.Show(@"是否自动分析探伤和外形", @"确认对话框", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            ThrContext.Reanalysis(des);
        }

        private static void CarNoRepair(DateTime source,DateTime des)
        {
            var carSources = ThrContext.GetCarLists(source);
            var cardeses = ThrContext.GetCarLists(des);
            foreach (var carList in cardeses)
            {
                ThrContext.RemoveCarList(carList);
            }
            ThrContext.SaveChanges();
            foreach (var carSource in carSources)
            {
                var car = new CarList
                {
                    testDateTime = des,
                    carNo = carSource.carNo,
                    carNo2 = carSource.carNo2,
                    posNo = carSource.posNo
                };
                ThrContext.AddCarList(car);
            }
            ThrContext.SaveChanges();
            //复制车厢图片
            var sourcePath = _picturePath + @"\" + source.ToString(@"yyyy\\MM");
            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show(@"文件路径" + sourcePath + @"不存在");
                return;
            }
            var desPath = _picturePath + @"\" + des.ToString(@"yyyy\\MM");
            var oldValue = source.ToString("yyyyMMdd_HHmmss");
            var newValue = des.ToString("yyyyMMdd_HHmmss");
            var files = Directory.GetFiles(sourcePath);
            Directory.CreateDirectory(desPath);
            foreach (var file in files)
            {
                var fileName = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                if (fileName.Contains(oldValue)&&fileName.Contains("jpg"))
                {
                    File.Copy(file, desPath + "\\" + fileName.Replace(oldValue, newValue), true);
                }
            }
        }
        /// <summary>
        ///     探伤补缺
        /// </summary>
        private static void TanRepair(string sourceText, string desText)
        {
            try
            {
                var source = DateTime.Parse(sourceText);
                var des = DateTime.Parse(desText);
                if (source.Equals(des))
                {
                    MessageBox.Show(@"起止时间相等");
                    return;
                }
                Nlogger.Trace("对操作对象（时刻作为主键）：" + desText + "，进行了探伤补缺操作，源时刻为：" + sourceText);
                ThrContext.proc_tanShangDataFill(des, source);
                ThrContext.Proc1(des);
                var sourcePath = _picturePath + @"\" + source.ToString(@"yyyy\\MM");
                if (!Directory.Exists(sourcePath))
                {
                    MessageBox.Show(@"文件路径" + sourcePath + @"不存在");
                    return;
                }
                var desPath = _picturePath + @"\" + des.ToString(@"yyyy\\MM");
                var oldValue = source.ToString("yyyyMMdd_HHmmss");
                var newValue = des.ToString("yyyyMMdd_HHmmss");
                var files = Directory.GetFiles(sourcePath);
                Directory.CreateDirectory(desPath);
                foreach (var file in files)
                {
                    var fileName = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                    if (fileName.Contains(oldValue) && !fileName.Contains("jpg"))
                    {
                        File.Copy(file, desPath + "\\" + fileName.Replace(oldValue, newValue), true);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     擦伤补缺
        /// </summary>
        private static void CaRepair(string sourceText, string desText)
        {
            try
            {
                var source = DateTime.Parse(sourceText);
                var des = DateTime.Parse(desText);
                if (source.Equals(des))
                {
                    MessageBox.Show(@"起止时间相等");
                    return;
                }
                Nlogger.Trace("对操作对象（时刻作为主键）：" + desText + "，进行了擦伤补缺操作，源时刻为：" + sourceText);
                ThrContext.proc_caShangDataFill(des, source);
                ThrContext.Proc2(des);
                GetValue(source, des, "擦伤数据");
                GetValue(source, des, "擦伤结果");
            }
            catch (Exception e)
            {
                Nlogger.Trace(e.Message);
                throw;
            }
        }

        private static bool GetValue(DateTime source, DateTime des, string fileName)
        {
            var sourcePath = _picturePath + @"\" + fileName + "\\datas\\" + source.ToString(@"yyyy\\MM");
            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show(@"文件路径" + sourcePath + @"不存在");
                return true;
            }
            var desPath = _picturePath + @"\" + fileName + "\\datas\\" + des.ToString(@"yyyy\\MM");
            var oldValue = source.ToString("yyyyMMdd_HHmmss");
            var newValue = des.ToString("yyyyMMdd_HHmmss");
            var files = Directory.GetFiles(sourcePath);
            Directory.CreateDirectory(desPath);
            foreach (var file in files)
            {
                var name = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                if (file.Contains(oldValue))
                {
                    File.Copy(file, desPath + "\\" + name.Replace(oldValue, newValue), true);
                }
            }
            return false;
        }

        public static void GetCars(string time)
        {
            var testDateTime = DateTime.Parse(time);
            _source.DataSource = ThrContext.GetCarLists(testDateTime);
            switch (ServerConfig.Type)
            {
                case "DC":
                    _dgv.Columns["testDateTime"].DisplayIndex = 0;
                    _dgv.Columns["posNo"].DisplayIndex = 1;
                    _dgv.Columns["carNo"].DisplayIndex = 2;
                    break;
                case "JC":
                    _dgv.Columns["testDateTime"].DisplayIndex = 0;
                    _dgv.Columns["posNo"].DisplayIndex = 1;
                    _dgv.Columns["carNo"].DisplayIndex = 2;
                    _dgv.Columns["direction"].DisplayIndex = 3;
                    break;
                default:
                    _dgv.Columns["testDateTime"].DisplayIndex = 0;
                    _dgv.Columns["posNo"].DisplayIndex = 1;
                    _dgv.Columns["carNo"].DisplayIndex = 2;
                    break;
            }
        }

        public static void Delete(string time, int index)
        {
            var testDateTime = DateTime.Parse(time);
            ThrContext.DeleteCarList(testDateTime, index);
            ThrContext.SaveChanges();
            Nlogger.Trace("车厢列表删除一行,时刻为：" + time);
        }
    }
}