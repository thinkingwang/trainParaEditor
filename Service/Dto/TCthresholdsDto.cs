using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using CommonModel;
using DCModel;
using TCModel;

namespace Service.Dto
{
    public class TCthresholdsDto : Dto
    {
        private readonly TCthreshold _thresholds;
        static TCthresholdsDto()
        {
        }
        private TCthresholdsDto(threshold th)
        {
            _thresholds = th as TCthreshold;
        }

        /// <summary>
        /// 导入外部数据，重写数据库数据
        /// </summary>
        /// <param name="data"></param>
        public static void CreateDataBase(ArrayList data)
        {
            using (var context = new DCContext(Dto.connectionstring))
            {
                foreach (var thresholdse in (List<TCthreshold>)data[0])
                {
                    context.Set<TCthreshold>().AddOrUpdate(thresholdse);
                }
                CRH_wheelDto.CreateDataBase((List<CRH_wheel>) data[1],context);
                TrainTypeDto.CreateDataBase((List<TrainType>) data[2],context);
                if (!context.Set<ProfileAdjust>().Any())
                {
                    context.Set<ProfileAdjust>().Add(new ProfileAdjust() { position = 0 });
                    context.Set<ProfileAdjust>().Add(new ProfileAdjust() { position = 1 });
                }
                context.SaveChanges();
            }
            
            Nlogger.Trace("导入外部数据，重写数据库数据");
        }

        /// <summary>
        /// 获得门限表所有数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<threshold> GetAll()
        {
            var data = from d in ThrContext.Set<TCthreshold>() select d;
            return data.ToList();
        }

        /// <summary>
        /// 获得门限表车型列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetThresholdsTypes()
        {
            //获取所有车型
            var trainTypes =
                (from v in ThrContext.Set<TCthreshold>() select v.trainType).Distinct();
            return trainTypes;
        }

        /// <summary>
        /// 获得门限表中指定车型的所有数据
        /// </summary>
        /// <param name="trainType"></param>
        /// <returns></returns>
        public static IEnumerable<TCthresholdsDto> GetThresholds(string trainType)
        {
            var result = new List<TCthresholdsDto>();
            IQueryable<threshold> data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                result.Add(new TCthresholdsDto(thresholdse));
            }
            return result;
        }

        /// <summary>
        /// 在门限表中删除指定车型
        /// </summary>
        /// <param name="trainType"></param>
        public static void Delete(string trainType)
        {
            CRH_wheelDto.Delete(trainType);
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                ThrContext.Set<TCthreshold>().Remove(thresholdse);
            }
            ThrContext.SaveChanges();
            Nlogger.Trace("在门限表中删除指定车型");
        }

        /// <summary>
        /// 新建指定车型
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="name"></param>
        public static void NewThresholds(string trainType, string name)
        {
            var item = (ThrContext.Set<TCthreshold>().FirstOrDefault(m => m.trainType.Equals(trainType))).Copy() as TCthreshold;
            if (item == null)
            {
                return;
            }
            item.name = name;
            ThrContext.Set<TCthreshold>().Add(item);
            ThrContext.SaveChanges();
            Nlogger.Trace("新建制定车型");
        }

        /// <summary>
        /// 删除指定车型某行数据
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="index"></param>
        public static void DeleteThresholds(string trainType, int index)
        {
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            ThrContext.Set<TCthreshold>().Remove(data.ToList().ElementAt(index));
            ThrContext.SaveChanges();
            Nlogger.Trace("删除指定车型某行数据");
        }

        /// <summary>
        /// 复制指定车型数据，并插入到门限表中
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="name"></param>
        public static void Copy(string trainType, string name)
        {
            CRH_wheelDto.Copy(trainType, name);
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                var holds = thresholdse.Copy() as TCthreshold;
                if (holds == null)
                {
                    continue;
                }
                holds.trainType = name;
                ThrContext.Set<TCthreshold>().Add(holds);
            }
            ThrContext.SaveChanges();
        }

        /// <summary>
        /// 复制指定车型数据，并插入到门限表中
        /// </summary>
        /// <param name="trainType"></param>
        public static void CopyPowerType(string trainType)
        {
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                var holds = thresholdse.Copy() as TCthreshold;
                if (holds == null)
                {
                    continue;
                }
                holds.powerType = 0;
                ThrContext.Set<TCthreshold>().AddOrUpdate(holds);
            }
            ThrContext.SaveChanges();
        }
        /// <summary>
        /// 复制指定车型数据，并插入到门限表中
        /// </summary>
        /// <param name="trainType"></param>
        public static void DeletePowerType(string trainType)
        {
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                if (thresholdse.powerType != 0)
                {
                    continue;
                }
                ThrContext.Set<TCthreshold>().Remove(thresholdse);
            }
            ThrContext.SaveChanges();
        }


        [Category("车型与参数名"), Description("车型与参数名"), ReadOnly(true), DisplayName(@"车型")]
        public string trainType
        {
            get { return _thresholds.trainType; }
            set
            {
                Nlogger.Trace("编辑表thresholds的trainType字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.trainType + ",修改后为：" + value);
                _thresholds.trainType = value;
                ThrContext.SaveChanges();
            }
        }



        [Category("车型与参数名"), Description("车型与参数名"), ReadOnly(true), DisplayName(@"参数描述")]
        public string ParamName
        {
            get
            {
                if (Document == null)
                {
                    return _thresholds.name;
                }
                var node = Document.SelectSingleNode(string.Format((string) "/config/add[@key='{0}']/@value", (object) _thresholds.name));
                return node == null ? _thresholds.name : node.Value;
            }
        }

        [DisplayName(@"标准值")]
        public decimal? standard
        {
            get { return _thresholds.standard; }
            set
            {
                Nlogger.Trace("编辑表thresholds的standard字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.standard + ",修改后为：" + value);
                _thresholds.standard = value;
                if (_thresholds.up_level3 < standard)
                {
                    Up3 = "禁用";
                }
                if (_thresholds.up_level2 < standard)
                {
                    Up2 = "禁用";
                }
                if (_thresholds.up_level1 < standard)
                {
                    Up1 = "禁用";
                }
                if (_thresholds.low_level3 > standard)
                {
                    Low3 = "禁用";
                }
                if (_thresholds.low_level2 > standard)
                {
                    Low2 = "禁用";
                }
                if (_thresholds.low_level1 > standard)
                {
                    Low1 = "禁用";
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName("上限三级")]
        public string Up3
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level3 - 2000))) < Convert.ToDecimal(0.01))
                {
                    return "禁用";
                }
                return _thresholds.up_level3.ToString();
            }
            set
            {
                Nlogger.Trace("编辑表thresholds的up_level3字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.up_level3 + ",修改后为：" + value);
                if (value.Equals("禁用"))
                {
                    _thresholds.up_level3 = Convert.ToDecimal(2000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter < standard)
                    {
                        throw new Exception("上限值必须大于标准值，请重新设定");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.up_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level1 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.up_level1))
                    {
                        _thresholds.up_level3 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("上限三级值必须小于等于上限二级和上限一级，请重新设定");
                    }
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName("上限二级")]
        public string Up2
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01))
                {
                    return "禁用";
                }
                return _thresholds.up_level2.ToString();
            }
            set
            {
                Nlogger.Trace("编辑表thresholds的up_level2字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.up_level2 + ",修改后为：" + value);
                if (value.Equals("禁用"))
                {
                    _thresholds.up_level2 = Convert.ToDecimal(2000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter < standard)
                    {
                        throw new Exception("上限值必须大于标准值，请重新设定");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level3 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.up_level3) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level1 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.up_level1))
                    {
                        if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01))
                        {
                            if (desc.Contains("预警"))
                            {
                                desc = desc.Remove(0, desc.IndexOf("预警") + 3);
                            }
                        }
                        else
                        {
                            if (desc.Contains("预警"))
                            {
                                desc = desc.Replace(_thresholds.up_level2.ToString(), value);
                            }
                            else
                            {
                                desc = desc.Insert(0, "大于" + value + "预警,");
                            }
                        }
                        _thresholds.up_level2 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("上限二级值必须大于等于上限三级，小于等于上限一级，请重新设定");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"上限一级")]
        public string Up1
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level1 - 2000))) < Convert.ToDecimal(0.01))
                {
                    return "禁用";
                }
                return _thresholds.up_level1.ToString();
            }
            set
            {
                Nlogger.Trace("编辑表thresholds的up_level1字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.up_level1 + ",修改后为：" + value);
                if (value.Equals("禁用"))
                {
                    _thresholds.up_level1 = Convert.ToDecimal(2000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter < standard)
                    {
                        throw new Exception("上限值必须大于标准值，请重新设定");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.up_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level3 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.up_level3))
                    {
                        _thresholds.up_level1 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("上限一级值必须大于等于上限三级和上限二级，请重新设定");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"下限三级")]
        public string Low3
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level3 + 1000))) < Convert.ToDecimal(0.01))
                {
                    return "禁用";
                }
                return _thresholds.low_level3.ToString();
            }
            set
            {
                Nlogger.Trace("编辑表thresholds的low_level3字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.low_level3 + ",修改后为：" + value);
                if (value.Equals("禁用"))
                {
                    _thresholds.low_level3 = Convert.ToDecimal(-1000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter > standard)
                    {
                        throw new Exception("下限值必须小于标准值，请重新设定");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level2 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.low_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level1 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.low_level1))
                    {
                        _thresholds.low_level3 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("下限三级值必须大于等于下限二级和下限一级，请重新设定");
                    }
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"下限二级")]
        public string Low2
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level2 + 1000))) < Convert.ToDecimal(0.01))
                {
                    return "禁用";
                }
                return _thresholds.low_level2.ToString();
            }
            set
            {
                Nlogger.Trace("编辑表thresholds的low_level2字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.low_level2 + ",修改后为：" + value);
                if (value.Equals("禁用"))
                {
                    _thresholds.low_level2 = Convert.ToDecimal(-1000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter > standard)
                    {
                        throw new Exception("下限值必须小于标准值，请重新设定");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level3 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.low_level3) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level1 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.low_level1))
                    {
                        _thresholds.low_level2 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("下限二级值必须大于等于下限一级小于等于下限三级，请重新设定");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"下限一级")]
        public string Low1
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level1 + 1000))) < Convert.ToDecimal(0.01))
                {
                    return "禁用";
                }
                return _thresholds.low_level1.ToString();
            }
            set
            {
                Nlogger.Trace("编辑表thresholds的low_level1字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.low_level1 + ",修改后为：" + value);
                if (value.Equals("禁用"))
                {
                    _thresholds.low_level1 = Convert.ToDecimal(-1000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter > standard)
                    {
                        throw new Exception("下限值必须小于标准值，请重新设定");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level2 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.low_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level3 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.low_level3))
                    {
                        _thresholds.low_level1 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("下限一级值必须小于等于下限二级和下限三级，请重新设定");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"精度")]
        public decimal? precision
        {
            get { return _thresholds.precision; }
            set
            {
                Nlogger.Trace("编辑表thresholds的precision字段，车型为："+ trainType +",参数描述为：" + ParamName + "，初始为：" + _thresholds.precision + ",修改后为：" + value);
                _thresholds.precision = value;
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"描述")]
        public string desc
        {
            get { return _thresholds.desc; }
            set
            {
                _thresholds.desc = value;
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"车厢类型")]
        public string PowerType 
        {
            get
            {
                if (_thresholds.powerType == 0)
                {
                    return "拖车";
                }
                return "动车";
            }
            set
            {
                ThrContext.Set<TCthreshold>().Remove(_thresholds);
                ThrContext.SaveChanges();
                if (value == "动车")
                {
                    _thresholds.powerType = 1;
                }
                else
                {
                    _thresholds.powerType = 0;
                }
                ThrContext.Set<TCthreshold>().AddOrUpdate(_thresholds);
                ThrContext.SaveChanges();
            }
        }




        private void SetDesc()
        {
            if (!Up1.Equals("禁用") && !Up2.Equals("禁用") && !Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm或小于{1}mm预警，大于{2}mm或小于{3}mm报警",
                    Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("禁用") && Up2.Equals("禁用") && !Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("小于{0}mm预警，大于{1}mm或小于{2}mm报警",
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("禁用") && !Up2.Equals("禁用") && !Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm或小于{1}mm预警，小于{2}mm报警", Convert.ToString((object) _thresholds.up_level2),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("禁用") && Up2.Equals("禁用") && !Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("小于{0}mm预警，小于{1}mm报警", Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }

            if (!Up1.Equals("禁用") && !Up2.Equals("禁用") && Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm预警，大于{1}mm或小于{2}mm报警",
                    Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("禁用") && !Up2.Equals("禁用") && !Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm或小于{1}mm预警，大于{2}mm报警",
                    Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("禁用") && !Up2.Equals("禁用") && Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm预警，大于{1}mm报警", Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", "")) ;
            }


            if (!Up1.Equals("禁用") && Up2.Equals("禁用") && Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm或小于{1}mm报警", Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("禁用") && Up2.Equals("禁用") && !Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("小于{0}mm预警，大于{1}mm报警", Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("禁用") && Up2.Equals("禁用") && Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm报警", Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""));
            }


            if (Up1.Equals("禁用") && !Up2.Equals("禁用") && Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm预警，小于{1}mm报警", Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("禁用") && !Up2.Equals("禁用") && !Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm或小于{1}mm预警", Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", "")) ;
            }
            if (Up1.Equals("禁用") && !Up2.Equals("禁用") && Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("大于{0}mm预警", Convert.ToString((object) _thresholds.up_level2).Replace(".00", "")) ;
            }


            if (Up1.Equals("禁用") && Up2.Equals("禁用") && Low2.Equals("禁用") && !Low1.Equals("禁用"))
            {
                desc = string.Format("小于{0}mm报警", Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("禁用") && Up2.Equals("禁用") && !Low2.Equals("禁用") && Low1.Equals("禁用"))
            {
                desc = string.Format("小于{0}mm预警", Convert.ToString((object) _thresholds.low_level2).Replace(".00", "")) ;
            }
        }
    }
}