using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonModel;
using JCModel;

namespace Service.Dto
{
    public class CarListDto:Dto
    {
        private static CarList _carList;
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static  string _picturePath = @"D:\tycho\data";
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

        static void _dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dgv.Columns[e.ColumnIndex];
            Nlogger.Trace("编辑表CarList的字段：" + column.HeaderText + "，修改前为：" + _tempValue + "，修改为：" + _dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        static void _dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
        /// 车厢列表增加新行
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

        public static void CopyTo(string sourceText, string desText, bool overWriteTanShang = false, bool overWriteCaShang = false, bool overWriteProfile = false)
        {

            var source = DateTime.Parse(sourceText);
            var des = DateTime.Parse(desText);
            var detectSource = ThrContext.Set<Detect>().
                FirstOrDefault(m => m.testDateTime.Equals(source));
            var detectOrg = ThrContext.Set<Detect>().
                FirstOrDefault(m => m.testDateTime.Equals(des));
            if (detectSource == null||detectOrg == null)
            {
                return;
            }
            if (!detectOrg.engNum.Equals(detectSource.engNum))
            {
                var result = MessageBox.Show(@"复制到目标检测时间车型与源检测时间车型不一致，是否继续复制,源时刻：" + source + ",目标时刻：" + des, @"确认对话框", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            var carSources = ThrContext.GetCarLists(source); ;
            var cardeses = ThrContext.GetCarLists(des); ;
            foreach (var carList in cardeses)
            {
                ThrContext.RemoveCarList(carList);
            }
            var detect = ThrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(des));
            if (overWriteProfile || overWriteTanShang || overWriteCaShang || detect == null)
            {
                if (detect != null && overWriteProfile && overWriteTanShang && overWriteCaShang)
                {
                    ThrContext.Set<Detect>().Remove(detect);
                    ThrContext.SaveChanges();
                }
                var detectDes = detectSource.Copy() as Detect;
                if (detectDes == null)
                {
                    return;
                }
                detectDes.testDateTime = des;
                detectDes.outDateTime = des;
                ThrContext.Set<Detect>().AddOrUpdate(detectDes);
                ThrContext.SaveChanges();
                if (overWriteTanShang || detect == null)
                {
                    TanRepair(sourceText, desText);
                }
                if (overWriteCaShang || detect == null)
                {
                    CaRepair(sourceText, desText);
                }
                if (overWriteProfile || detect == null)
                {
                    ProfileDetectResultDto.Repair(desText, sourceText);
                }
            }

            foreach (var carSource in carSources)
            {
                var car = new CarList()
                {
                    testDateTime = des,
                    carNo = carSource.carNo,
                    carNo2 = carSource.carNo2,
                    posNo = carSource.posNo
                };
                ThrContext.AddCarList(car);
            }
            ThrContext.SaveChanges();
        }

        /// <summary>
        ///     探伤补缺
        /// </summary>
        private static void TanRepair(string sourceText, string desText)
        {
            try
            {
                DateTime source = DateTime.Parse(sourceText);
                DateTime des = DateTime.Parse(desText);
                if (source.Equals(des))
                {
                    MessageBox.Show(@"起止时间相等");
                    return;
                }
                Nlogger.Trace("对操作对象（时刻作为主键）：" + desText + "，进行了探伤补缺操作，源时刻为：" + sourceText);
                ThrContext.proc_tanShangDataFill(des, source);
                    string sourcePath = _picturePath + @"\" + source.ToString(@"yyyy\\MM");
                    if (!Directory.Exists(sourcePath))
                    {
                        MessageBox.Show(@"文件路径" + sourcePath + @"不存在");
                        return;
                    }
                    string desPath = _picturePath + @"\" + des.ToString(@"yyyy\\MM");
                    string oldValue = source.ToString("yyyyMMdd_HHmmss");
                    string newValue = des.ToString("yyyyMMdd_HHmmss");
                    string[] files = Directory.GetFiles(sourcePath);
                    Directory.CreateDirectory(desPath);
                    foreach (string file in files)
                    {
                        var fileName = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)+1);
                        if (fileName.Contains(oldValue))
                        {
                            File.Copy(file, desPath + "\\" + fileName.Replace(oldValue,newValue),true);
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
                DateTime source = DateTime.Parse(sourceText);
                DateTime des = DateTime.Parse(desText);
                if (source.Equals(des))
                {
                    MessageBox.Show(@"起止时间相等");
                    return;
                }
                Nlogger.Trace("对操作对象（时刻作为主键）：" + desText + "，进行了擦伤补缺操作，源时刻为：" + sourceText);
                ThrContext.proc_caShangDataFill(des, source);
                GetValue(source, des, "擦伤数据");
                GetValue(source, des, "擦伤结果");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool GetValue(DateTime source, DateTime des,string fileName)
        {
            string sourcePath = _picturePath + @"\" + fileName+ "\\datas\\" + source.ToString(@"yyyy\\MM");
            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show(@"文件路径" + sourcePath + @"不存在");
                return true;
            }
            string desPath = _picturePath + @"\" + fileName + "\\datas\\" + des.ToString(@"yyyy\\MM");
            string oldValue = source.ToString("yyyyMMdd_HHmmss");
            string newValue = des.ToString("yyyyMMdd_HHmmss");
            string[] files = Directory.GetFiles(sourcePath);
            Directory.CreateDirectory(desPath);
            foreach (string file in files)
            {
                var name = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                if (file.Contains(oldValue))
                {
                    File.Copy(file, desPath + "\\" + name.Replace(oldValue, newValue),true);
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