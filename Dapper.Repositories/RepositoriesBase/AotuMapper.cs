using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Kogel.Dapper.Extension;
using Kogel.Dapper.Extension.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Dapper.Repositories
{
    /// <summary>
    /// 实体映射
    /// </summary>
    public class AotuMapper<T> : ClassMap<T>
    {
        public AotuMapper()
        {
            var type = typeof(T);
            var entityObject = EntityCache.Register(type);
            foreach (var item in entityObject.Properties)
            {
                var map = Map(type, item);
                var tcOption = map.TypeConverterOption;
                tcOption.NumberStyles(NumberStyles.Any);
                tcOption.DateTimeStyles(DateTimeStyles.None);
                map.Name(entityObject.FieldPairs[item.Name]);
                if (item.PropertyType.IsEnum)
                {
                    map.TypeConverter<EnumToShortConverter>();
                    tcOption.Format();
                }
                if (item.PropertyType == typeof(DateTime))
                {
                    //  map.TypeConverter<DateTimeConverter>();
                    tcOption.Format("yyyy-MM-dd HH:mm:ss");
                }
                if (item.PropertyType == typeof(bool))
                {
                    map.TypeConverter<BoolenToBitConverter>();
                    tcOption.Format();
                }
            }
        }

    }
    /// <summary>
    /// 将枚举类型转换成short
    /// </summary>
    public class EnumToShortConverter : DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var type = memberMapData.Member.MemberType();
            try
            {
                return  Convert.ToInt16( Enum.Parse(type,  value.ToString())).ToString();
            }
            catch
            {
                return base.ConvertToString(value.ToString(), row, memberMapData);
            }
        }
        ///// <summary>
        ///// Converts the string to an object.
        ///// </summary>
        ///// <param name="text">The string to convert to an object.</param>
        ///// <param name="row">The <see cref="IReaderRow"/> for the current record.</param>
        ///// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being created.</param>
        ///// <returns>The object created from the string.</returns>
        //public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        //{
        //    var type = memberMapData.Member.MemberType();
        //    try
        //    {
        //        return (short)Enum.Parse(type, text, false);
        //    }
        //    catch
        //    {
        //        return base.ConvertFromString(text, row, memberMapData);
        //    }
        //}
    }

    /// <summary>
    /// 将枚举类型转换成short
    /// </summary>
    public class BoolenToBitConverter : DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (bool.TryParse(value.ToString(), out var b))
            {
                if (!b)
                {
                    return 0.ToString();
                }
                else { return 1.ToString(); }
            }
            else
            {
                return 0.ToString();
            }
        }
        ///// <summary>
        ///// Converts the string to an object.
        ///// </summary>
        ///// <param name="text">The string to convert to an object.</param>
        ///// <param name="row">The <see cref="IReaderRow"/> for the current record.</param>
        ///// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being created.</param>
        ///// <returns>The object created from the string.</returns>
        //public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        //{
        //    if (bool.TryParse(text, out var b))
        //    {
        //        if (b)
        //        {
        //            return 0;
        //        }
        //        else { return 1; }
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
    }
}
