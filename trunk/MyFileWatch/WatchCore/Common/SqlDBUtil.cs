/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-10-31
 * 时间: 15:04
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace WatchCore.Common
{

    public class SqlDBUtil
    {
        /// 
        /// 通用数据库类
        /// 
        private static string ConnStr = null;

        static SqlDBUtil()
        {            
            //ConnStr = ConfigurationManager.ConnectionStrings["conSql"].ConnectionString;
            ConnStr = System.Configuration.ConfigurationManager.AppSettings["SqlDB"];
        }
        
        /// 
        /// 返回connection对象
        /// 
        /// 
        public SqlConnection ReturnConn()
        {
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            return Conn;
        }
        public void Dispose(SqlConnection Conn)
        {
            if (Conn != null)
            {
                Conn.Close();
                Conn.Dispose();
            }
        }
        /// 
        /// 运行SQL语句
        /// 
        /// 
        public void RunProc(string SQL)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlCommand Cmd;
            Cmd = CreateCmd(SQL, Conn);
            try
            {
                Cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception(SQL);
            }
            Dispose(Conn);
            return;
        }

        /// 
        /// 运行SQL语句返回DataReader
        /// 
        /// 
        /// SqlDataReader对象.
        public SqlDataReader RunProcGetReader(string SQL)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlCommand Cmd;
            Cmd = CreateCmd(SQL, Conn);
            SqlDataReader Dr;
            try
            {
                Dr = Cmd.ExecuteReader(CommandBehavior.Default);
            }
            catch
            {
                throw new Exception(SQL);
            }
            //Dispose(Conn);
            return Dr;
        }        /// 
        /// 生成Command对象
        /// 
        /// 
        /// 
        /// 
        public SqlCommand CreateCmd(string SQL, SqlConnection Conn)
        {
            SqlCommand Cmd;
            Cmd = new SqlCommand(SQL, Conn);
            return Cmd;
        }        /// 
        /// 生成Command对象
        /// 
        /// 
        /// 
        public SqlCommand CreateCmd(string SQL)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlCommand Cmd;
            Cmd = new SqlCommand(SQL, Conn);
            return Cmd;
        }
        /// 
        /// 返回adapter对象
        /// 
        /// 
        /// 
        /// 
        public SqlDataAdapter CreateDa(string SQL)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlDataAdapter Da;
            Da = new SqlDataAdapter(SQL, Conn);
            return Da;
        }        /// 
        /// 运行SQL语句,返回DataSet对象
        /// 
        /// SQL语句
        /// DataSet对象
        public DataSet RunProc(string SQL, DataSet Ds)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlDataAdapter Da;
            //Da = CreateDa(SQL, Conn);
            Da = new SqlDataAdapter(SQL, Conn);
            try
            {
                Da.Fill(Ds);
            }
            catch (Exception Err)
            {
                throw Err;
            }
            Dispose(Conn);
            return Ds;
        }
        /// 
        /// 运行SQL语句,返回DataSet对象
        /// 
        /// SQL语句
        /// DataSet对象
        /// 表名
        public DataSet RunProc(string SQL, DataSet Ds, string tablename)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlDataAdapter Da;
            Da = CreateDa(SQL);
            try
            {
                Da.Fill(Ds, tablename);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            Dispose(Conn);
            return Ds;
        }        /// 
        /// 运行SQL语句,返回DataSet对象
        /// 
        /// SQL语句
        /// DataSet对象
        /// 表名
        public DataSet RunProc(string SQL, DataSet Ds, int StartIndex, int PageSize, string    tablename)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlDataAdapter Da;
            Da = CreateDa(SQL);
            try
            {
                Da.Fill(Ds, StartIndex, PageSize, tablename);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            Dispose(Conn);
            return Ds;
        }        /// 
        /// 检验是否存在数据
        /// 
        /// 
        public bool ExistDate(string SQL)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlDataReader Dr;
            Dr = CreateCmd(SQL, Conn).ExecuteReader();
            if (Dr.Read())
            {
                Dispose(Conn);
                return true;
            }
            else
            {
                Dispose(Conn);
                return false;
            }
        }        /// 
        /// 返回SQL语句执行结果的第一行第一列
        /// 
        /// 字符串
        public string ReturnValue(string SQL)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            string result;
            SqlDataReader Dr;
            try
            {
                Dr = CreateCmd(SQL, Conn).ExecuteReader();
                if (Dr.Read())
                {
                    result = Dr[0].ToString();
                    Dr.Close();
                }
                else
                {
                    result = "";
                    Dr.Close();
                }
            }
            catch
            {
                throw new Exception(SQL);
            }
            Dispose(Conn);
            return result;
        }        /// 
        /// 返回SQL语句第一列,第ColumnI列,
        /// 
        /// 字符串
        public string ReturnValue(string SQL, int ColumnI)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            string result;
            SqlDataReader Dr;
            try
            {
                Dr = CreateCmd(SQL, Conn).ExecuteReader();
            }
            catch
            {
                throw new Exception(SQL);
            }
            if (Dr.Read())
            {
                result = Dr[ColumnI].ToString();
            }
            else
            {
                result = "";
            }
            Dr.Close();
            Dispose(Conn);
            return result;
        }        /// 
        /// 生成一个存储过程使用的sqlcommand.
        /// 
        /// 存储过程名.
        /// 存储过程入参数组.
        /// sqlcommand对象.
        public SqlCommand CreateCmd(string procName, SqlParameter[] prams)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlCommand Cmd = new SqlCommand(procName, Conn);
            Cmd.CommandType = CommandType.StoredProcedure;
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                {
                    if (parameter != null)
                    {
                        Cmd.Parameters.Add(parameter);
                    }
                }
            }
            return Cmd;
        }
        /// 
        /// 为存储过程生成一个SqlCommand对象
        /// 
        /// 存储过程名
        /// 存储过程参数
        /// SqlCommand对象
        private SqlCommand CreateCmd(string procName, SqlParameter[] prams, SqlDataReader Dr)
        {
            SqlConnection Conn;
            Conn = new SqlConnection(ConnStr);
            Conn.Open();
            SqlCommand Cmd = new SqlCommand(procName, Conn);
            Cmd.CommandType = CommandType.StoredProcedure;
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    Cmd.Parameters.Add(parameter);
            }
            Cmd.Parameters.Add(
             new SqlParameter("ReturnValue", SqlDbType.Int, 4,
             ParameterDirection.ReturnValue, false, 0, 0,
             string.Empty, DataRowVersion.Default, null));

            return Cmd;
        }        ///         /// 运行存储过程,返回.
        /// 
        /// 存储过程名
        /// 存储过程参数
        /// SqlDataReader对象
        public void RunProc(string procName, SqlParameter[] prams, SqlDataReader Dr)
        {

            SqlCommand Cmd = CreateCmd(procName, prams, Dr);
            Dr = Cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return;
        }        /// 
        /// 运行存储过程,返回.
        /// 
        /// 存储过程名
        /// 存储过程参数
        public string RunProc(string procName, SqlParameter[] prams)
        {
            SqlDataReader Dr;
            SqlCommand Cmd = CreateCmd(procName, prams);
            Dr = Cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (Dr.Read())
            {
                return Dr.GetValue(0).ToString();
            }
            else
            {
                return "";
            }
        }        /// 
        /// 运行存储过程,返回dataset.
        /// 
        /// 存储过程名.
        /// 存储过程入参数组.
        /// dataset对象.
        public DataSet RunProc(string procName, SqlParameter[] prams, DataSet Ds)
        {
            SqlCommand Cmd = CreateCmd(procName, prams);
            SqlDataAdapter Da = new SqlDataAdapter(Cmd);
            try
            {
                Da.Fill(Ds);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Ds;
        }
        
        public static bool insert(object obj)
		{
			List<SqlParameter> para = new List<SqlParameter>();
			try {
				//反射出类型
				Type objclass =obj.GetType();
				//拼SQL开始
				string sqltou = "insert into "+objclass.Name+"(";
				string sqlval="values('";
				//验证是否为pojo类。
				PropertyInfo ispojo = objclass.GetProperty("Px");
				if(ispojo==null)
				{
					return false;
				}
				//获取所有属性
				PropertyInfo[] fields = objclass.GetProperties();
				foreach(PropertyInfo field in fields)
				{
					//去掉验证属性及主键
					if(field.Name!="Px"&&field.Name!=ispojo.GetValue(obj,null).ToString())
					{
						object fvalue=field.GetValue(obj,null);
						SqlParameter tt = new SqlParameter();
						tt.ParameterName = "@"+field.Name;
						tt.Value=fvalue;
						//验证整形
						if(field.PropertyType.Name=="Int32")
						{
							tt.DbType=DbType.Int32;
						}
						if(field.PropertyType.Name=="String")
						{
							tt.DbType=DbType.String;
						}
						if(field.PropertyType.Name=="Boolean")
						{
							tt.DbType=DbType.Boolean;
						}
						if(field.PropertyType.Name=="DateTime")
						{
							tt.DbType=DbType.Date;
						}
						//大字段
						if(field.PropertyType.Name=="Byte[]")
						{
							tt.DbType=DbType.Binary;
						}
						if(fvalue!=null)
						{
							para.Add(tt);
							sqltou+=field.Name+",";
							sqlval=sqlval.Substring(0,sqlval.Length-1)+tt.ParameterName+",'";
						}
						
					}
				}
				String sql=sqltou.Substring(0,sqltou.Length-1)+")"+sqlval.Substring(0,sqlval.Length-2)+");";
				//拼SQL结束。
				ExecuteQuery(sql,para.ToArray());
				return true;
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
				return false;
			}
		}
    }    
}
 

