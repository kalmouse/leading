using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LeadingClass
{
    public static class SqlHelper
    {
        public static string SCOPE_IDENTITY = " SELECT SCOPE_IDENTITY(); ";//返回为当前会话和当前作用域中的任何表最后生成的标识值。
        public static string @_IDENTITY = " SELECT @@IDENTITY; ";//返回为当前会话的所有作用域中的任何表最后生成的标识值。

        /// <summary>
        /// 返回为任何会话和任何作用域中的特定表最后生成的标识值。
        /// 获取某个表的最大ID值
        /// </summary>
        /// <param name="TableName">TableName</param>
        /// <returns>SQL语句</returns>
        public static string GetIDENT_CURRENT(String TableName)
        {
            string sql = "";
            if (TableName != "")
            {
                sql = " SELECT IDENT_CURRENT('" + TableName + "'); ";
            }
            return sql;
        }
        /// <summary>
        /// 单表或多表 进行 单条或多条Insert时调用
        /// </summary>
        /// <param name="option">SQLOption</param>
        /// <returns>SQL语句</returns>
        public static string GetInsertSQL(SQLOption option)
        {
            string tableName = "";
            StringBuilder sqlBuilder = new StringBuilder();
            if (option.InsertModelList.Count > 0)
            {
                for (int i = 0; i < option.InsertModelList.Count; i++)
                {

                    string colunmPart = "";
                    string valuePart = "";
                    foreach (var column in option.InsertModelList[i].GetType().GetProperties())
                    {
                        if (column.Name.ToLower() != "id")
                        {
                            colunmPart += column.Name + ",";
                            if (option.ParentIdColum != "" && column.Name.ToLower() == option.ParentIdColum.ToLower() && i > 0)
                            {
                                valuePart += "@ParentID ,";
                            }
                            else
                            {
                                valuePart += "'" + column.GetValue(option.InsertModelList[i],null) + "' ,";
                            }
                        }
                    }
                    if (tableName != option.InsertModelList[i].GetType().Name)
                    {
                        if (i > 0)
                        {
                            sqlBuilder.Append("; ");
                            if (option.ParentIdColum != "" && i == 1)
                            {
                                sqlBuilder.Append(" declare @ParentID int; ");
                                sqlBuilder.Append(" set @ParentID=SCOPE_IDENTITY(); ");
                            }
                        }
                        sqlBuilder.Append("INSERT INTO ");
                        sqlBuilder.Append(" ["+option.InsertModelList[i].GetType().Name+"] ");
                        sqlBuilder.Append(" (" + colunmPart.TrimEnd(',') + ") VALUES ");
                        sqlBuilder.Append(" ( " + valuePart.TrimEnd(',') + " ) ");

                    }
                    else
                    {
                        sqlBuilder.Append(" ,( " + valuePart.TrimEnd(',') + " ) ");
                    }
                    tableName = option.InsertModelList[i].GetType().Name;

                }
                sqlBuilder.Append(";");
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 单表单条 Insert  
        /// </summary>
        /// <param name="option">实体类</param>
        /// <returns>SQL语句</returns>
        public static string GetInsertSQL(BaseModel model)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (model != null)
            {
                string colunmPart = "";
                string valuePart = "";
                foreach (var column in model.GetType().GetProperties())
                {
                    if (column.Name.ToLower() != "id")
                    {
                        colunmPart += column.Name + ",";
                        valuePart += "'" + column.GetValue(model, null) + "' ,";

                    }
                }

                sqlBuilder.Append("INSERT INTO ");
                sqlBuilder.Append(" [" + model.GetType().Name + "] ");
                sqlBuilder.Append(" (" + colunmPart.TrimEnd(',') + ") VALUES ");
                sqlBuilder.Append(" ( " + valuePart.TrimEnd(',') + " ) ");
                sqlBuilder.Append(";");
            }
            return sqlBuilder.ToString();
        }
        /// <summary>
        /// 指定更新字段或指定where条件或需要 批处理时调用
        /// </summary>
        /// <param name="option">SQLOption</param>
        /// <returns>SQL语句</returns>
        public static string GetUpdateSQL(SQLOption option)
        {
            string sql = "";
            if (option.UpdateModelList.Count > 0)
            {
                foreach (var updateModel in option.UpdateModelList)
                {
                    if (updateModel.TableName != "" && updateModel.UpdateColumnDic.Count > 0 && updateModel.WhereColumnDic.Count > 0 || updateModel.UpdateColumnList.Count > 0)
                    {
                        string valuePart = "";
                        string optionPart = "";
                        string wherePart = "";
                        foreach (var item in updateModel.UpdateColumnDic)
                        {
                            valuePart += string.Format(" {0}='{1}',", item.Key, item.Value);
                        }
                        valuePart = valuePart.TrimEnd(",".ToCharArray());

                        foreach (var item in updateModel.WhereColumnDic)
                        {
                            optionPart += string.Format(" {0}='{1}' and", item.Key, item.Value);

                        }
                        optionPart = optionPart.TrimEnd("and".ToCharArray());

                        if (updateModel.UpdateColumnList.Count > 0)
                        {
                            foreach (var item in updateModel.UpdateColumnList)
                            {
                                if (item.Symbol == "replace")
                                {
                                    wherePart += string.Format(", {0} = {1} ('{2}') and", item.Field, item.Symbol, item.Val);
                                }
                            }
                            wherePart = wherePart.TrimEnd("and".ToCharArray());
                        }
                        sql += string.Format(@"Update [{0}] set {1}{3} where {2} ;", updateModel.TableName, valuePart, optionPart,wherePart);
                    }
                }
            }
            return sql;
        }
        /// <summary>
        /// 指定更新字段或指定where条件或需要 批处理时调用
        /// </summary>
        /// <param name="option">SQLOption</param>
        /// <returns>SQL语句</returns>
        public static string GetUpdateSQL(UpdateModel updateModel)
        {
            string sql = "";
            if (updateModel.TableName != "" && updateModel.UpdateColumnDic.Count > 0 && updateModel.WhereColumnDic.Count > 0 || updateModel.WhereColumnList.Count > 0)
            {
                string valuePart = "";
                string optionPart = "";
                string wherePart = "";
                foreach (var item in updateModel.UpdateColumnDic)
                {
                    valuePart += string.Format(" {0}='{1}',", item.Key, item.Value);
                }
                valuePart = valuePart.TrimEnd(",".ToCharArray());

                foreach (var item in updateModel.WhereColumnDic)
                {
                    optionPart += string.Format(" {0}='{1}' and", item.Key, item.Value);
                }
                optionPart = optionPart.TrimEnd("and".ToCharArray());

                if (updateModel.WhereColumnList.Count > 0)
                {
                    foreach (var item in updateModel.WhereColumnList)
                    {
                        if (item.Symbol == "=" || item.Symbol == ">" || item.Symbol == ">=" || item.Symbol == "<" || item.Symbol == "<=")
                        {
                            wherePart += string.Format(" {0}{1} '{2}' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "like")
                        {
                            wherePart += string.Format(" {0}{1}'%{2}%' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "llike")
                        {
                            wherePart += string.Format(" {0}{1}'%{2}' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "rlike")
                        {
                            wherePart += string.Format(" {0}{1}'{2}%' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "in")
                        {
                            wherePart += string.Format(" {0}  {1} ('{2}') and", item.Field, item.Symbol, item.Val);
                        }
                    }
                    wherePart = wherePart.TrimEnd("and".ToCharArray());
                }
                sql += string.Format(@"Update [{0}] set {1} where {2}{3} ;", updateModel.TableName, valuePart, optionPart, wherePart);
            }
            
            return sql;
        }
        /// <summary>
        /// 单表多字段以Id为where条件进行Update时 调用
        /// </summary>
        /// <param name="model">继承BaseModel的实体类</param>
        /// <returns>SQL语句</returns>
        public static string GetUpdateSQL(BaseModel model)
        {
            string sql = "";
            if (model != null)
            {
                string wherePart = "";
                string valuePart = "";
                foreach (var column in model.GetType().GetProperties())
                {
                    if (column.Name.ToLower() != "id")
                    {
                        valuePart += string.Format(" {0} = '{1}',", column.Name, column.GetValue(model,null));
                    }
                    else
                    {
                        wherePart = string.Format(" {0} = '{1}'", column.Name, column.GetValue(model,null));
                    }
                }
                valuePart = valuePart.TrimEnd(",".ToCharArray());
                sql = string.Format(@"Update [{0}] set {1} where {2} ;", model.GetType().Name, valuePart, wherePart);
            }
            return sql;
        }

        /// <summary>
        /// 指定更新字段或指定where条件或需要 批处理时调用
        /// </summary>
        /// <param name="option">SQLOption</param>
        /// <returns>SQL语句</returns>
        public static string GetSelectSQL(SQLOption option)
        {

            string sql = "";
            if (option.SelectModelList.Count > 0)
            {
                foreach (var selectModel in option.SelectModelList)
                {

                    if (selectModel.TableName != "")
                    {
                        string selectPart = "";
                        string wherePart = "";
                        string groupPart = "";
                        string orderPart = "";
                        string descPart = "";
                        if (selectModel.IsAllColumn || selectModel.SelectColumnList.Count == 0)
                        {
                            selectModel.SelectColumnList.Add("*");
                        }

                        selectPart = String.Join(",", selectModel.SelectColumnList.ToArray());

                        if (selectModel.WhereColumnDic.Count > 0)
                        {
                            wherePart = " WHERE ";
                            foreach (var item in selectModel.WhereColumnDic)
                            {
                                wherePart += string.Format(" {0}='{1}' and", item.Key, item.Value);

                            }
                            wherePart = wherePart.TrimEnd("and".ToCharArray());
                        }
                        if (selectModel.WhereColumnList.Count > 0)
                        {
                            wherePart = " WHERE ";
                            foreach (var item in selectModel.WhereColumnList)
                            {
                                if (item.Symbol == "=" || item.Symbol == ">" || item.Symbol == ">=" || item.Symbol == "<" || item.Symbol == "<=")
                                {
                                    wherePart += string.Format(" {0}{1} '{2}' and", item.Field, item.Symbol, item.Val);
                                }
                                else if (item.Symbol == "like")
                                {
                                    wherePart += string.Format(" {0}{1}'%{2}%' and", item.Field, item.Symbol, item.Val);
                                }
                                else if (item.Symbol == "llike")
                                {
                                    wherePart += string.Format(" {0}{1}'%{2}' and", item.Field, item.Symbol, item.Val);
                                }
                                else if (item.Symbol == "rlike")
                                {
                                    wherePart += string.Format(" {0}{1}'{2}%' and", item.Field, item.Symbol, item.Val);
                                }
                                else if (item.Symbol == "in")
                                {
                                    wherePart += string.Format(" {0}{1} ('{2}') and", item.Field, item.Symbol, item.Val);
                                }
                            }
                            wherePart = wherePart.TrimEnd("and".ToCharArray());
                        }
                        if (selectModel.GroupByColumnList.Count > 0)
                        {
                            groupPart = " GROUP BY ";
                            groupPart += String.Join(",", selectModel.GroupByColumnList.ToArray());
                        }
                        if (selectModel.OrderByColumnList.Count > 0)
                        {
                            orderPart = " ORDER BY ";
                            orderPart += String.Join(",", selectModel.OrderByColumnList.ToArray());
                            if (selectModel.IsDesc == false)
                            {
                                descPart = " ASC ";
                            }
                            else
                            {
                                descPart = " DESC ";
                            }
                        }

                        sql += string.Format(@"SELECT {0} FROM [{1}] {2} {3} {4} {5} ;", selectPart, selectModel.TableName, wherePart, groupPart, orderPart, descPart);

                    }

                }

            }
            return sql;
        }
        /// <summary>
        /// 单表，多字段，多条件 查询
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string GetSelectSQL(SelectModel selectModel)
        {
            string sql = "";

            if (selectModel.TableName != "")
            {
                string selectPart = "";
                string wherePart = "";
                string groupPart = "";
                string orderPart = "";
                string descPart = "";
                if (selectModel.IsAllColumn || selectModel.SelectColumnList.Count == 0)
                {
                    selectModel.SelectColumnList.Add("*");
                }

                selectPart = String.Join(",", selectModel.SelectColumnList.ToArray());

                if (selectModel.WhereColumnDic.Count > 0)
                {
                    wherePart = " WHERE ";
                    foreach (var item in selectModel.WhereColumnDic)
                    {
                        wherePart += string.Format(" {0}='{1}' and", item.Key, item.Value);

                    }
                    wherePart = wherePart.TrimEnd("and".ToCharArray());
                }
                if (selectModel.WhereColumnList.Count > 0)
                {
                    wherePart = " WHERE ";
                    foreach (var item in selectModel.WhereColumnList)
                    {
                        if (item.Symbol == "=" || item.Symbol == ">" || item.Symbol == ">=" || item.Symbol == "<" || item.Symbol == "<=")
                        {
                            wherePart += string.Format(" {0}{1} '{2}' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "like")
                        {
                            wherePart += string.Format(" {0}{1}'%{2}%' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "llike")
                        {
                            wherePart += string.Format(" {0}{1}'%{2}' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "rlike")
                        {
                            wherePart += string.Format(" {0}{1}'{2}%' and", item.Field, item.Symbol, item.Val);
                        }
                        else if (item.Symbol == "in")
                        {
                            wherePart += string.Format(" {0}{1} ('{2}') and", item.Field, item.Symbol, item.Val);
                        }
                    }
                    wherePart = wherePart.TrimEnd("and".ToCharArray());
                }
                if (selectModel.GroupByColumnList.Count > 0)
                {
                    groupPart = " GROUP BY ";
                    groupPart += String.Join(",", selectModel.GroupByColumnList.ToArray());
                }
                if (selectModel.OrderByColumnList.Count > 0)
                {
                    orderPart = " ORDER BY ";
                    orderPart += String.Join(",", selectModel.OrderByColumnList.ToArray());
                    if (selectModel.IsDesc == false)
                    {
                        descPart = " ASC ";
                    }
                    else
                    {
                        descPart = " DESC ";
                    }
                }

                sql += string.Format(@"SELECT {0} FROM [{1}] {2} {3} {4} {5} ;", selectPart, selectModel.TableName, wherePart, groupPart, orderPart, descPart);

            }
            return sql;
        }
        /// <summary>
        /// 单表Load时调用 ，按Id查询
        /// </summary>
        /// <param name="model">继承BaseModel的实体类</param>
        /// <returns>SQL语句</returns>
        public static string GetSelectSQL(BaseModel model)
        {
            string sql = "";
            if (model != null)
            {
                string idValue = "";
                foreach (var column in model.GetType().GetProperties())
                {
                    if (column.Name.ToLower() == "id")
                    {
                        idValue = column.GetValue(model, null).ToString();
                        break;
                    }
                }
                sql = string.Format(@"SELECT * FROM [{0}] WHERE Id = '{1}' ;", model.GetType().Name, idValue);
            }
            return sql;
        }


    

        /// <summary>
        /// 多条件 批量删除
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string GetDeleteSQL(SQLOption option)
        {
            string sql = "";
            if (option.DeleteModelList.Count > 0)
            {

                foreach (var item in option.DeleteModelList)
                {
                    if (item.TableName != "" && item.WhereColumnDic.Count > 0)
                    {
                        string wherePart = "";
                        foreach (var whereDic in item.WhereColumnDic)
                        {
                            wherePart += string.Format(" {0}='{1}' and", whereDic.Key, whereDic.Value);

                        }
                        wherePart = wherePart.TrimEnd("and".ToCharArray());
                        sql += string.Format(@"DELETE FROM [{0}] WHERE {1} ;", item.TableName, wherePart);
                    }
                }
            }
            return sql;
        }

      
        /// <summary>
        /// 单条 删除
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string GetDeleteSQL(BaseModel model)
        {
            string sql = "";
            if (model != null)
            {
                string idValue = "";
                foreach (var column in model.GetType().GetProperties())
                {
                    if (column.Name.ToLower() == "id")
                    {
                        idValue = column.GetValue(model, null).ToString();
                        break;
                    }
                }
                sql = string.Format(@"DELETE FROM [{0}] WHERE Id = '{1}' ;", model.GetType().Name, idValue);
            }
            return sql;
        }


    }
}
