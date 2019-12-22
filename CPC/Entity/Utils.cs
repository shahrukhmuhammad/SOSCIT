using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Web.Configuration;
using System.Reflection;
using System.Globalization;
using System.Security;

namespace CPC
{
    public static class Utils
    {
        public static string getConfig(string keyName)
        {
            return WebConfigurationManager.AppSettings.Get(keyName);
        }
        //public static string getSiteUrl()
        //{
        //    try
        //    {
        //        if (RootPath != null && !RootPath.Equals(string.Empty))
        //        {
        //            return RootPath;
        //        }
        //        else
        //        {
        //            return WebConfigurationManager.AppSettings.Get(Constants.SITE_URL);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return WebConfigurationManager.AppSettings.Get(Constants.SITE_URL);
        //    }
        //}
        //public static string getSiteName()
        //{
        //    try
        //    {
        //        return WebConfigurationManager.AppSettings.Get(Constants.SITE_NAME);
        //    }
        //    catch (Exception)
        //    {
        //        return WebConfigurationManager.AppSettings.Get(Constants.SITE_NAME);
        //    }
        //}
        public static string RootPath
        {
            get
            {
                return "http://" + HttpContext.Current.Request.Url.Host +
                (HttpContext.Current.Request.Url.Port != 80 && HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : string.Empty);
            }
        }


        public static bool IsImageFile(string contentType)
        {
            switch (contentType)
            {
                case "image/x-png":
                case "image/pjpeg":
                case "image/gif":
                case "image/tiff":
                case "image/png":
                case "image/jpeg":
                    return true;
                default:
                    return false;
            }
        }
        public static string getMonthStr(int month)
        {
            try
            {

                if ((month == 1) || (month == 01))
                {
                    return "January";
                }
                else if ((month == 2) || (month == 02))
                {
                    return "Fabruary";
                }
                else if ((month == 3) || (month == 03))
                {
                    return "March";
                }
                else if ((month == 4) || (month == 04))
                {
                    return "April";
                }
                else if ((month == 5) || (month == 05))
                {
                    return "May";
                }
                else if ((month == 6) || (month == 06))
                {
                    return "June";
                }
                else if ((month == 7) || (month == 07))
                {
                    return "July";
                }
                else if ((month == 8) || (month == 08))
                {
                    return "August";
                }
                else if ((month == 9) || (month == 09))
                {
                    return "September";
                }
                else if (month == 10)
                {
                    return "October";
                }
                else if (month == 11)
                {
                    return "November";
                }
                else if (month == 12)
                {
                    return "December";
                }

                return string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static Boolean isValidEmail(string Email)
        {
            try
            {
                //string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

                string pattern = @"^[a-z0-9_\+-]+(\.[a-z0-9_\+-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,4})$";
                
                Match match = Regex.Match(Email, pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string getRandomCode(int StrLength)
        {
            string AccessCode = DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString();// +DateTime.UtcNow.Year.ToString();

            string randomStr = Guid.NewGuid().ToString();
            randomStr = randomStr.Replace("-", "");
            if (randomStr.Length > StrLength)
            {
                randomStr = randomStr.Substring(1, StrLength - 1);
            }
            AccessCode = AccessCode + randomStr;


            return AccessCode.ToUpper();
        }

        public static string GetEmailTemplate(string strSalutation, string messageBody)
        {
            try
            {
                string retHtml = "<div style='border:Solid 5px #efefef;font:14px/1.5em Arial,Helvetica,sans-serif'>";
                retHtml = retHtml + "<div style='height:30px;background-color:#efefef;padding-left:5px;font-size:20px;padding-top:5px'>";
                retHtml = retHtml + "<b>Sharaka</b>";
                retHtml = retHtml + "</div>";
                retHtml = retHtml + "<div style='padding:20px'>";
                if (!string.IsNullOrEmpty(strSalutation))
                {
                    retHtml = retHtml + "<div style='font-size:16px;'>";
                    retHtml = retHtml + "<b>Dear " + strSalutation + "</b>,";
                    retHtml = retHtml + "</div>";
                }
                else
                {
                    retHtml = retHtml + "<div style='font-size:16px;'>";
                    retHtml = retHtml + "<b>Dear Customer,";
                    retHtml = retHtml + "</div>";
                }
                retHtml = retHtml + "<div style='padding-top:20px;'>";
                retHtml = retHtml + messageBody;
                retHtml = retHtml + "</div>";
         
                retHtml = retHtml + "<div style='padding-top:20px'>";
                retHtml = retHtml + "Kind Regards,";
                retHtml = retHtml + "</div>";
                retHtml = retHtml + "<div>";
                retHtml = retHtml + "ebizLine Team";
                retHtml = retHtml + "</div>";

                retHtml = retHtml + "</div>";
                retHtml = retHtml + "<div style='height:15px;background-color:#efefef;padding:7px;text-align:center;font-size:12px'>";
                retHtml = retHtml + "This is an automated message from <a href='http://www.Sharaka.com'>www.Sharaka.com</a>";
                retHtml = retHtml + "</div>";
                retHtml = retHtml + "</div>";

                return retHtml;

            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
        public static string formatAddressStr(string address, string city, string state, string zipcode)
        {
            try
            {

                string strAddress = "";
                Boolean findAddress = false;
                if (!string.IsNullOrEmpty(address))
                {
                    strAddress = address;
                    findAddress = true;
                }
                else
                {
                    findAddress = false;
                }

                if (!string.IsNullOrEmpty(city))
                {
                    if (findAddress == true)
                    {
                        strAddress = strAddress + ", " + city;
                    }
                    else
                    {
                        strAddress = strAddress + city;
                    }
                    findAddress = true;
                }
                else
                {
                    findAddress = false;
                }
                if (!string.IsNullOrEmpty(state))
                {
                    if (findAddress == true)
                    {
                        strAddress = strAddress + ", " + state;
                    }
                    else
                    {
                        strAddress = strAddress + state;
                    }
                    findAddress = true;
                }
                else
                {
                    findAddress = false;
                }
                if (!string.IsNullOrEmpty(zipcode))
                {
                    if (findAddress == true)
                    {
                        strAddress = strAddress + ", " + zipcode;
                    }
                    else
                    {
                        strAddress = strAddress + zipcode;
                    }
                    findAddress = true;
                }


                return strAddress;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string formatContactNosStr(string PhoneNo, string CellNo)
        {
            try
            {

                string strNos = "";

                if (!string.IsNullOrWhiteSpace(PhoneNo))
                {
                    strNos = PhoneNo;
                }

                if (!string.IsNullOrWhiteSpace(CellNo))
                {
                    if (strNos == "")
                    {
                        strNos = CellNo;
                    }
                    else
                    {
                        strNos = strNos + ", "+ CellNo;
                    }
                }
               return strNos;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public static string getDecription(string reqParam)
        {
            try
            {
                if (reqParam != null)
                {
                    reqParam = reqParam.Replace("bizSlashzib0001", "/");
                    reqParam = reqParam.Replace("bizPluszib0001", "+");
                    reqParam = reqParam.Replace("bizEqualzib0001", "=");
                    reqParam = reqParam.Replace("bizSpacezib0002", " ");
                    reqParam = reqParam.Replace("bizTiledzib0003", "~");
                    reqParam = reqParam.Replace("bizSmallTiledzib0004", "`");
                    reqParam = reqParam.Replace("bizNotzib0005", "!");
                    reqParam = reqParam.Replace("bizAtRateOfzib0006", "@");
                    reqParam = reqParam.Replace("bizHashzib0007", "#");
                    reqParam = reqParam.Replace("bizPercentzib0008", "%");
                    reqParam = reqParam.Replace("bizPowerzib0009", "^");
                    reqParam = reqParam.Replace("bizAndzib00010", "&");
                }
                return reqParam;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string setEncyption(string reqParam)
        {
            try
            {
                if (reqParam != null)
                {
                    reqParam = reqParam.Replace("/", "bizSlashzib0001");
                    reqParam = reqParam.Replace("+", "bizPluszib0001");
                    reqParam = reqParam.Replace("=", "bizEqualzib0001");
                    reqParam = reqParam.Replace(" ", "bizSpacezib0002");
                    reqParam = reqParam.Replace("~", "bizTiledzib0003");
                    reqParam = reqParam.Replace("`", "bizSmallTiledzib0004");
                    reqParam = reqParam.Replace("!", "bizNotzib0005");
                    reqParam = reqParam.Replace("@", "bizAtRateOfzib0006");
                    reqParam = reqParam.Replace("#", "bizHashzib0007");
                    reqParam = reqParam.Replace("%", "bizPercentzib0008");
                    reqParam = reqParam.Replace("^", "bizPowerzib0009");
                    reqParam = reqParam.Replace("&", "bizAndzib00010");
                }
                return reqParam;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string SiteDomainName
        {
            get
            {
                return (HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 80 && HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : string.Empty)).Replace("http://","").Replace("www.","").Replace(".ebizline.pk","");
            }
        }

        public static string getShortString(string str, int strLength)
        {
            try
            {
                if (str == null)
                {
                    return string.Empty;
                }
                if (str.Length > strLength)
                {
                    str = str.Substring(0, strLength - 1) + " ...";
                }

                return str;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string getTimeDifference(DateTime currentDateTime, DateTime fromDateTime)
        {
            string retTimeDifference = "";
            try
            {
                Int64 totalMinutsDiff = Convert.ToInt64((DateTime.Now - fromDateTime).TotalMinutes);

                if (totalMinutsDiff < 1)
                {
                    retTimeDifference = "Now";
                }
                else if (totalMinutsDiff <= 59)
                {
                    retTimeDifference = totalMinutsDiff + " minutes ago";
                }
                else
                {
                    var hours = Convert.ToInt64(totalMinutsDiff / 60);
                    if (hours == 1)
                    {
                        retTimeDifference = hours + " hour ago";
                    }
                    else if (hours <= 23)
                    {
                        retTimeDifference = hours + " hours ago";
                    }
                    else if (hours > 23)
                    {
                        retTimeDifference = fromDateTime.ToString("ddd, MMM d, yyyy");
                    }
                    

                }
            }
            catch(Exception)
            { }
            return retTimeDifference;

        }
        

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            if (items != null)
            {
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
            }
            return dataTable;
        }

        public static string GetReportName(string ReportName)
        {
            return "rpt" + ReportName + ".rdlc";
        }

        //public static string GetReportForm(string ReportFormName)
        //{
        //    return Constants.REPORT_PATH + ReportFormName + ".aspx?";
        //}

        

        //public static string GetReportHeader(string ReportTitle)
        //{
        //    return
        //        string.Format(
        //            @"<div class='col-lg-5 pull-left' style='text-align:left;font-size:smaller'><small>Print Date: <b>{0}</b></small></div><div class='col-lg-5 pull-right' style='text-align: right; font-size:smaller'><small>Print Time: <b>{1}</b></small></div><div class='col-lg-12' style='text-align: center'><h1>{2}</h1></div>",
        //            DateTime.Now.ToString(Constants.DATE_FORMAT_SERVER_SIDE), DateTime.Now.ToString("HH:mm:ss"),
        //            ReportTitle);
        //}

        public static DateTime SetDateFormate(string DateString)
        {
            return DateTime.ParseExact(DateString, Constants.DATE_FORMAT_CLIENT_SIDE, CultureInfo.InvariantCulture);
        }

        public static Int32 nullSafe(string value)
        {
            if (string.IsNullOrEmpty(value) || !IsNumeric(value))
                return 0;
            else
                return Convert.ToInt32(value);
        }

        public static bool IsNumeric(string val)
        {
            return Regex.IsMatch(val, @"^\d+$");
        }
    }
}
