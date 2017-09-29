using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;

/// <summary>
///JsonHelper 的摘要说明
/// </summary>
public class JsonHelper
{
    /// 把dataset数据转换成json的格式
    /// </summary>
    /// <param name="ds">dataset数据集</param>
    /// <returns>json格式的字符串</returns>
    public static string GetJsonByDataset(DataSet ds)
    {
        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
        {
            return null;
        }
        StringBuilder sb = new StringBuilder();
        foreach (System.Data.DataTable dt in ds.Tables)
        {
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("{");
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
            }
            sb.Append("},");
        }
        sb.Remove(sb.ToString().LastIndexOf(','), 1);
        return sb.ToString();
    }
    /// <summary>
    /// 将object转换成为string
    /// </summary>
    /// <param name="ob">obj对象</param>
    /// <returns></returns>
    private static string ObjToStr(object ob)
    {
        if (ob == null)
        {
            return string.Empty;
        }
        else
            return ob.ToString();
    }
    /// <summary>  
    /// JSON序列化  
    /// </summary>  
    public static string JsonSerializer<T>(T t)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }

    /// <summary>  
    /// JSON反序列化  
    /// </summary>  
    public static T JsonDeserialize<T>(string jsonString)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }



    /// <summary>  
    /// JSON带时间序列化  
    /// </summary>  
    public static string JsonDateSerializer<T>(T t)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        string p = @"\\/Date\((\d+)\+\d+\)\\/";
        MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
        Regex reg = new Regex(p);
        jsonString = reg.Replace(jsonString, matchEvaluator);
        return jsonString;
    }

    /// <summary>  
    /// JSON带时间反序列化  
    /// </summary>  
    public static T JsonDateDeserialize<T>(string jsonString)
    {
        //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式  
        string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
        MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
        Regex reg = new Regex(p);
        jsonString = reg.Replace(jsonString, matchEvaluator);
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }

    /// <summary>  
    /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串  
    /// </summary>  
    private static string ConvertJsonDateToDateString(Match m)
    {
        string result = string.Empty;
        DateTime dt = new DateTime(1970, 1, 1);
        dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
        dt = dt.ToLocalTime();
        result = dt.ToString("yyyy-MM-dd HH:mm:ss");
        return result;
    }

    /// <summary>  
    /// 将时间字符串转为Json时间  
    /// </summary>  
    private static string ConvertDateStringToJsonDate(Match m)
    {
        string result = string.Empty;
        DateTime dt = DateTime.Parse(m.Groups[0].Value);
        dt = dt.ToUniversalTime();
        TimeSpan ts = dt - DateTime.Parse("1970-01-01");
        result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
        return result;
    }

    /// <summary>  
    /// dataTable转换成Json格式  
    /// </summary>  
    /// <param name="dt"></param>  
    /// <returns></returns>  
    public static string DataTableJson(DataTable dt)
    {
        StringBuilder jsonBuilder = new StringBuilder();
        //jsonBuilder.Append("{\"");
        jsonBuilder.Append("\"");
        jsonBuilder.Append(dt.TableName);
        jsonBuilder.Append("\":[");
        //jsonBuilder.Append("[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dt.Rows[i][j].ToString());
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            if (i > dt.Rows.Count - 1)
            {
                jsonBuilder.Append("}");
            }
            else
            {
                jsonBuilder.Append("},");
            }
        }
        jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
        jsonBuilder.Append("]");
        // jsonBuilder.Append("}");
        return jsonBuilder.ToString();
    }

    public static string DatatableJson(DataTable dt)
    {
        StringBuilder jsonBuilder = new StringBuilder();


        jsonBuilder.Append("\"");
        jsonBuilder.Append(dt.TableName);
        jsonBuilder.Append("\":[");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Rows[i][j].ToString());
                jsonBuilder.Append("\"");
            }
            if (i <= dt.Rows.Count - 2)
            {
                jsonBuilder.Append(",");
            }
            else
            {
                jsonBuilder.Append("]");
            }
        }
        return jsonBuilder.ToString();
    }

    public static string DataTabletoListJson(DataTable dt)
    {
        StringBuilder jsonBuilder = new StringBuilder();


        jsonBuilder.Append("\"");
        jsonBuilder.Append(dt.TableName);
        jsonBuilder.Append("\":{");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (j < dt.Columns.Count - 1)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(":");
                }
                else
                {
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                }
            }
            if (i <= dt.Rows.Count - 2)
            {
                jsonBuilder.Append(",");
            }
            else
            {
                jsonBuilder.Append("}");
            }
        }
        return jsonBuilder.ToString();
    }
    /// 把dataset数据转换成json的格式
    /// </summary>
    /// <param name="ds">dataset数据集</param>
    /// <returns>json格式的字符串</returns>
    public static string DatasetToJson(DataSet ds)
    {
        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
        {
            //如果查询到的数据为空则返回标记ok:false
            return "{\"ok\":false}";
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"ok\":true,");
        foreach (DataTable dt in ds.Tables)
        {
            sb.Append(string.Format("\"{0}\":[", dt.TableName));

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("{");
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("},");
            }

            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("],");
        }
        sb.Remove(sb.ToString().LastIndexOf(','), 1);
        sb.Append("}");
        return sb.ToString();
    }
}