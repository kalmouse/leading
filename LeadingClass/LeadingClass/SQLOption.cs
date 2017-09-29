using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace LeadingClass
{
    public class SQLOption
    {
        public List<UpdateModel> UpdateModelList { get; set; }
        public List<SelectModel> SelectModelList { get; set; }
        public List<DeleteModel> DeleteModelList { get; set; }
        public List<BaseModel> InsertModelList { get; set; }
        public String ParentIdColum { get; set; }
        public SQLOption()
        {
            InsertModelList = new List<BaseModel>();
            UpdateModelList = new List<UpdateModel>();
            SelectModelList = new List<SelectModel>();
            DeleteModelList = new List<DeleteModel>();
            ParentIdColum = "";

        }

    }

    public class UpdateModel
    {
        public string TableName { get; set; }
        public Dictionary<string, string> UpdateColumnDic { get; set; }
        public List<QueryCondition> UpdateColumnList { get; set; }
        public Dictionary<string, string> WhereColumnDic { get; set; }
        public List<QueryCondition> WhereColumnList { get; set; }
        public UpdateModel()
        {
            TableName = "";
            UpdateColumnDic = new Dictionary<string, string>();
            UpdateColumnList = new List<QueryCondition>();
            WhereColumnDic = new Dictionary<string, string>();
            WhereColumnList = new List<QueryCondition>();
        }
    }
    public class SelectModel
    {
        public string TableName { get; set; }
        public bool IsAllColumn { get; set; }
        public bool IsDesc { get; set; }
        public List<String> SelectColumnList { get; set; }
        public Dictionary<string, string> WhereColumnDic { get; set; }
        public List<QueryCondition> WhereColumnList { get; set; }
        public List<string> GroupByColumnList { get; set; }
        public List<string> OrderByColumnList { get; set; }
        public SelectModel()
        {
            TableName = "";
            IsAllColumn = false;
            IsDesc = true;
            SelectColumnList = new List<string>();
            GroupByColumnList = new List<string>();
            OrderByColumnList = new List<string>();
            WhereColumnDic = new Dictionary<string, string>();
            WhereColumnList = new List<QueryCondition>();
        }
    }
    public class DeleteModel
    {
        public string TableName { get; set; }
        public Dictionary<string, string> WhereColumnDic { get; set; }
        public DeleteModel()
        {
            TableName = "";
            WhereColumnDic = new Dictionary<string, string>();
        }
    }

    public class QueryCondition
    {
        public string Field { get; set; }
        public string Symbol { get; set; }
        public object Val { get; set; }
    }

}
