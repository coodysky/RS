using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Extension
{
    public static class DateExtension
    {
        #region 将日期以字符串输出

        private static DateTime utcStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 将DateTime类型转成“yyyy-MM-dd HH:mm:ss”的字符串格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToStringDateFromDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 将DateTime类型转成“yyyy-MM-dd HH:mm:ss”的字符串格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToStringDateFromDate(this DateTime? dt)
        {
            if (!dt.HasValue)
                return string.Empty;

            return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 将long类型转成“yyyy-MM-dd HH:mm:ss”的字符串格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToStringDateFromLong(this long dt_utc)
        {
            DateTime utcDt = utcStart.AddMilliseconds(dt_utc).ToLocalTime();

            return utcDt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 将long类型转成“yyyy-MM-dd HH:mm:ss”的字符串格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToStringDateFromLong(this long? dt_utc)
        {
            if (!dt_utc.HasValue)
                return string.Empty;

            DateTime utcDt = utcStart.AddMilliseconds(dt_utc.Value).ToLocalTime();

            return utcDt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion


    }
}
