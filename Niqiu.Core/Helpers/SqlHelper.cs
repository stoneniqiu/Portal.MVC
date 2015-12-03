using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Niqiu.Core.Helpers
{
    public class SqlHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["defCon"].ConnectionString;

        private static SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static DataTable ExecuteDataTable(string cmdText, params IDataParameter[] cmdParms)
        {
            return ExecuteDataset(cmdText, cmdParms).Tables[0];
        }

        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params IDataParameter[] cmdParms)
        {
            return ExecuteDataset(cmdType, cmdText, cmdParms).Tables[0];
        }

        public static DataSet ExecuteDataset(string cmdText, params IDataParameter[] cmdParms)
        {
            return ExecuteDataset(CommandType.Text, cmdText, cmdParms);
        }

        public static DataSet ExecuteDataset(CommandType cmdType, string cmdText, params IDataParameter[] cmdParms)
        {
            using (SqlConnection connection = CreateConnection())
            {
                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dataSet);
                return dataSet;
            }
        }

        public static int ExecuteNonQuery(string cmdText, params IDataParameter[] cmdParms)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, cmdParms);
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params IDataParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = CreateConnection())
            {
                PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return num;
            }
        }

        public static IDataReader ExecuteReader(string cmdText, params IDataParameter[] cmdParms)
        {
            return ExecuteReader(CommandType.Text, cmdText, cmdParms);
        }

        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText, params IDataParameter[] cmdParms)
        {
            IDataReader reader2;
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = CreateConnection();
            try
            {
                PrepareCommand(cmd, conn, cmdType, cmdText, cmdParms);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                reader2 = reader;
            }
            catch
            {
                conn.Close();
                throw;
            }
            return reader2;
        }

        public static object ExecuteScalar(string cmdText, params IDataParameter[] cmdParms)
        {
            return ExecuteScalar(CommandType.Text, cmdText, cmdParms);
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params IDataParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = CreateConnection())
            {
                PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                if (obj2 == DBNull.Value)
                {
                    obj2 = null;
                }
                return obj2;
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, CommandType cmdType, string cmdText, IDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (parameter != null)
                    {
                        if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parameter);
                    }
                }
            }
        }

        /// <summary>
        /// 获取Sql Server 2000表的字段注释
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllDescriptions(string tableName)
        {
            string sql = string.Format(@"SELECT b.name AS tblName, c.name AS colName, a.[value] AS Description
                FROM sysproperties a INNER JOIN
                sysobjects b ON a.id = b.id INNER JOIN
                syscolumns c ON a.id = c.id AND a.smallid = c.colid
                WHERE (a.name = 'MS_Description') and b.name='{0}'
                ORDER BY c.colorder", tableName);
            using (IDataReader reader = ExecuteReader(sql))
            {
                Dictionary<string, string> descDict = new Dictionary<string, string>();
                while (reader.Read())
                {
                    descDict.Add(reader.GetString(1), reader.GetString(2));
                }
                return descDict;
            }
        }

        /// <summary>
        /// 给DataTable附加注释
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tableName">数据库表名称</param>
        public static void FillDescription(DataTable table, string tableName)
        {
            Dictionary<string, string> descDict = GetAllDescriptions(tableName);
            if (descDict.Count > 0)
            {
                string temp;
                foreach (DataColumn column in table.Columns)
                {
                    if (descDict.TryGetValue(column.ColumnName, out temp))
                        column.Caption = temp;
                }
            }
        }
    }
}