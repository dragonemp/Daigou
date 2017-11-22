using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PurchaseHelper.BusinessObjects
{
    public class BOBase<T> where T : new()
    {
        public string TableName = "";
        public string _connString = "";
        public string PrimaryKey = "";
        protected T _myValues;
        protected List<T> _myList;
        public List<string> ValidationErrors = new List<string>();
        public virtual int Save(T Contract)
        {
            int pkVal = -1;
            if (Validate(Contract))
            {
                ContractWrapper<T> cw = new ContractWrapper<T>(Contract);
                bool isInsert = true;
                
                if (cw.GetFieldValue(PrimaryKey) != null && (int)cw.GetFieldValue(PrimaryKey) > 0)
                {
                    isInsert = false;
                    pkVal = (int)cw.GetFieldValue(PrimaryKey);
                }
                string cols = "", vals = "";
                string update = "";
                object val;
                string formattedVal;
                string sql = "";
                foreach (var fldInfo in cw.FieldMap)
                {
                    string fldName = fldInfo.Key;
                    PropertyInfo prop = fldInfo.Value;
                    val = cw.GetFieldValue(fldName);
                    if (val != null && fldName != PrimaryKey)
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            formattedVal = "'" + val.ToString().Replace("'", "''") + "'";
                        }
                        else if (prop.PropertyType == typeof(DateTime))
                        {
                            formattedVal = "'" + val.ToString() + "'";
                        }
                        else
                        {
                            formattedVal = val.ToString();
                        }

                        if (isInsert)
                        {
                            cols += fldName + ",";
                            vals += formattedVal + ",";
                        }
                        else
                        {
                            update += fldName + "=" + formattedVal + ",";
                        }
                    }

                }
                if (isInsert)
                    sql = string.Format("INSERT INTO {0}({1}) OUTPUT Inserted.{3} VALUES({2})", TableName, cols.Substring(0, cols.Length - 1), vals.Substring(0, vals.Length - 1), PrimaryKey);
                else
                    sql = string.Format("UPDATE {0} SET {1} WHERE {2}={3}", TableName, update.Substring(0, update.Length - 1), PrimaryKey, pkVal.ToString());

                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = sql;
                        try
                        {
                            conn.Open();
                            pkVal = (int)comm.ExecuteScalar();
                        }
                        catch (Exception e)
                        {
                            ValidationErrors.Add(e.Message);
                        }
                    }
                }
            }

            return pkVal;
        }

        public virtual bool Delete(int id)
        {
            bool deleted = false;

            string sql = string.Format("DELETE FROM {0} WHERE {1}={2}", TableName, PrimaryKey, id.ToString());
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = sql;
                    try
                    {
                        conn.Open();
                        comm.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        ValidationErrors.Add(e.Message);
                    }
                }
            }

            return deleted;
        }

        public virtual T GetByID(int id)
        {
            DataReaderMapper _mapper = new DataReaderMapper();
            ContractWrapper<T> _contractWrapper = new ContractWrapper<T>();
            using (SqlConnection myConnection = new SqlConnection(_connString))
            {
                string sql = string.Format("Select * from {0} where {1}=@id",TableName, PrimaryKey);
                SqlCommand oCmd = new SqlCommand(sql, myConnection);
                oCmd.Parameters.AddWithValue("@id", id);           
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {    
                        _myValues = _mapper.MapDataToFields<T>( oReader, _contractWrapper );
                    }
                }
            }

            return _myValues;
        }

        public virtual List<T> GetList(string filter)
        {
            DataReaderMapper _mapper = new DataReaderMapper();
            ContractWrapper<T> _contractWrapper = new ContractWrapper<T>();
            _myList = new List<T>();
            using (SqlConnection myConnection = new SqlConnection(_connString))
            {
                string sql;
                if (string.IsNullOrEmpty(filter))
                    sql = string.Format("Select * from {0}", TableName);
                else
                    sql = string.Format("Select * from {0} Where {1}", TableName, filter);
                SqlCommand oCmd = new SqlCommand(sql, myConnection);          
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {    
                        _myValues = _mapper.MapDataToFields<T>( oReader, _contractWrapper );
                        _myList.Add(_myValues);
                    }
                }
            }

            return _myList;
        }

        protected virtual bool Validate(T Contract)
        {
            return true;
        }
    }
}
