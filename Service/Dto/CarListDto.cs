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
        private static BindingSource _source;
        private static DataGridView _dgv;
        private static BindingNavigator _bn;
        private static  string _picturePath = @"D:\tycho\data";
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

        public static DataGridView GetDgv()
        {
            return _dgv ?? (_dgv = new DataGridView {DataSource = GetSource()});
        }

        public static BindingSource GetSource()
        {
            if (_source == null)
            {

                _source = new BindingSource();
            }
            return _source;
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
                thrContext.InsertCarList(testDateTime);
                thrContext.SaveChanges();
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
            var carSources = from v in thrContext.Set<CarList>() where v.testDateTime.Equals(source) select v;
            var cardeses = (from v in thrContext.Set<CarList>() where v.testDateTime.Equals(des) select v).ToList();
            foreach (var carList in cardeses)
            {
                thrContext.Set<CarList>().Remove(carList);
            }
            var detect = thrContext.Set<Detect>().FirstOrDefault(m => m.testDateTime.Equals(des));
            if (overWriteProfile || overWriteTanShang || overWriteCaShang ||detect == null)
            {
                if (detect != null && overWriteProfile && overWriteTanShang && overWriteCaShang)
                {
                    thrContext.Set<Detect>().Remove(detect);
                    thrContext.SaveChanges();
                }
                var detectSource = thrContext.Set<Detect>().
                    FirstOrDefault(m => m.testDateTime.Equals(source));
                if (detectSource != null)
                {
                    var detectDes = DeepCopy(detectSource);
                    detectDes.testDateTime = des;
                    detectDes.outDateTime = des;
                    thrContext.Set<Detect>().AddOrUpdate(detectDes);
                    thrContext.SaveChanges();
                    if (overWriteTanShang||detect == null)
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
                thrContext.Set<CarList>().Add(car);
            }
            thrContext.SaveChanges();
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
                thrContext.proc_tanShangDataFill(des, source);
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
                thrContext.proc_caShangDataFill(des, source);
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
            _source.DataSource = thrContext.GetCarLists(testDateTime);
        }

        public static void Delete(string time, int index)
        {
            var testDateTime = DateTime.Parse(time);
            thrContext.DeleteCarList(testDateTime, index);
            thrContext.SaveChanges();
            Nlogger.Trace("车厢列表删除一行,时刻为：" + time);
        }

    }
}