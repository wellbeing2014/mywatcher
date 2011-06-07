using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace WatchCilent
{
	/// <summary>
	/// Description of DBUtil.
	/// </summary>
	public class AccessDBUtil
	{
		private static String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
		static AccessDBUtil()
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;  
			Microsoft.Win32.RegistryKey dbc = key.OpenSubKey("software\\WisoftWatchClient");  
			connectionString +=@dbc.GetValue("dbpath").ToString();
		}
		
		public static bool insert(object obj)
		{
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
						//验证整形，空的整形为0
						if(field.PropertyType.Name=="Int32")
						{
							
							sqltou+=field.Name+",";
							sqlval=sqlval.Substring(0,sqlval.Length-1)+fvalue.ToString()+",'";
						}
						else
						if(fvalue!=null&&fvalue.ToString()!=null)
						{
							sqltou+=field.Name+",";
							sqlval+=fvalue.ToString()+"','";
						}
					}
				}
				String sql=sqltou.Substring(0,sqltou.Length-1)+")"+sqlval.Substring(0,sqlval.Length-2)+");";
				//拼SQL结束。
				ExecuteNonQuery(sql);
				return true;
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
				return false;
			}
		}
		
		public static  int insertreturn(object obj)
		{
			int i =0;
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
					return i;
				}
				//获取所有属性
				PropertyInfo[] fields = objclass.GetProperties();
				foreach(PropertyInfo field in fields)
				{
					//去掉验证属性及主键
					if(field.Name!="Px"&&field.Name!=ispojo.GetValue(obj,null).ToString())
					{
						object fvalue=field.GetValue(obj,null);
						//验证整形，空的整形为0
						if(field.PropertyType.Name=="Int32")
						{
							
							sqltou+=field.Name+",";
							sqlval=sqlval.Substring(0,sqlval.Length-1)+fvalue.ToString()+",'";
						}
						else
						if(fvalue!=null&&fvalue.ToString()!=null)
						{
							sqltou+=field.Name+",";
							sqlval+=fvalue.ToString()+"','";
						}
					}
				}
				String sql=sqltou.Substring(0,sqltou.Length-1)+")"+sqlval.Substring(0,sqlval.Length-2)+");";
				//拼SQL结束。
				i=ExecuteNonQuery(sql,null);
				return i;
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
				return i;
			}
		}
		
		public static bool update(object obj)
		{
			try {
				//反射出类型
				Type objclass =obj.GetType();
				//拼SQL开始
				string sqltou = "update "+objclass.Name+" set ";
				
				//验证是否为pojo类。
				PropertyInfo ispojo = objclass.GetProperty("Px");
				if(ispojo==null)
				{
					return false;
				}
				PropertyInfo px = objclass.GetProperty(ispojo.GetValue(obj,null).ToString());
				if(px==null||px.GetValue(obj,null)==null)
				{
					return false;
				}
				//获取所有属性
				PropertyInfo[] fields = objclass.GetProperties();
				string tempsql ="";
				foreach(PropertyInfo field in fields)
				{
					//去掉验证属性及主键
					if(field.Name!="Px"&&field.Name!=ispojo.GetValue(obj,null).ToString())
					{
						object fvalue=field.GetValue(obj,null);
						//验证整形，空的整形为0
						if(field.PropertyType.Name=="Int32")
						{
							tempsql+=field.Name+"=" +fvalue.ToString()+",";
							
						}
						else
						if(fvalue!=null&&fvalue.ToString()!=null)
						{
							tempsql+=field.Name+"='" +fvalue.ToString()+"',";
						}
					}
				}
				String sql=sqltou+tempsql.Substring(0,tempsql.Length-1)+" where "+px.Name+"="+px.GetValue(obj,null).ToString()+";";
				//拼SQL结束。
				ExecuteNonQuery(sql);
				return true;
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
				return false;
			}
		}
		
		public static bool delete(object obj)
		{
			try {
				//反射出类型
				Type objclass =obj.GetType();
				//拼SQL开始
				string sqltou = "delete from "+objclass.Name+" where ";
				
				//验证是否为pojo类。
				PropertyInfo ispojo = objclass.GetProperty("Px");
				if(ispojo==null)
				{
					return false;
				}
				PropertyInfo px = objclass.GetProperty(ispojo.GetValue(obj,null).ToString());
				if(px==null||px.GetValue(obj,null)==null)
				{
					return false;
				}
				
				String sql=sqltou+px.Name+"="+px.GetValue(obj,null).ToString()+";";
				//拼SQL结束。
				ExecuteNonQuery(sql);
				return true;
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
				return false;
			}
		}
		
		//执行单条插入语句，并返回id，不需要返回id的用ExceuteNonQuery执行。
		public static int ExecuteInsert(string sql,OleDbParameter[] parameters)
        {
        	//Debug.WriteLine(sql);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                try
                {
                    connection.Open();
                    if(parameters!=null) cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"select @@identity";
                    int value = Int32.Parse(cmd.ExecuteScalar().ToString());
                    return value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
		public static int ExecuteInsert(string sql)
		{
			return ExecuteInsert(sql,null);
		}
		
		//执行带参数的sql语句,返回影响的记录数（insert,update,delete)
		public static int ExecuteNonQuery(string sql,OleDbParameter[] parameters)
        {
        	//Debug.WriteLine(sql);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                try
                {
                    connection.Open();
                    if(parameters!=null) cmd.Parameters.AddRange(parameters);
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
		//执行不带参数的sql语句，返回影响的记录数
		//不建议使用拼出来SQL
		public static int ExecuteNonQuery(string sql)
        {
			return ExecuteNonQuery(sql,null);
        }
		
		//执行单条语句返回第一行第一列,可以用来返回count(*)
		public static int ExecuteScalar(string sql,OleDbParameter[] parameters)
        {
        	//Debug.WriteLine(sql);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                try
                {
                    connection.Open();
                    if(parameters!=null) cmd.Parameters.AddRange(parameters);
                    int value = Int32.Parse(cmd.ExecuteScalar().ToString());
                    return value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
		public static int ExecuteScalar(string sql)
        {
			return ExecuteScalar(sql,null);
        }
		
		//执行事务
		public static void ExecuteTrans(List<string> sqlList,List<OleDbParameter[]> paraList)
		{
			//Debug.WriteLine(sql);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd = new OleDbCommand();
            	OleDbTransaction transaction = null;
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    cmd.Transaction = transaction;
                    
                    for(int i=0;i<sqlList.Count;i++)
                    {
                    	cmd.CommandText=sqlList[i];
                    	if(paraList!=null&&paraList[i]!=null) 
                    	{
                    		cmd.Parameters.Clear();
                    		cmd.Parameters.AddRange(paraList[i]);
                    	}
                    	cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();

                }
                catch (Exception e)
                {
                	try
		            {
		                transaction.Rollback();
		            }
		            catch
		            {
		               
		            }
                    throw e;
                }
                
            }
		}
		public static void ExecuteTrans(List<string> sqlList)
		{
			ExecuteTrans(sqlList,null);
		}

		//执行查询语句，返回dataset
        public static DataSet ExecuteQuery(string sql,OleDbParameter[] parameters)
        {
        	//Debug.WriteLine(sql);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    
                    OleDbDataAdapter da = new OleDbDataAdapter(sql, connection);
                    if(parameters!=null) da.SelectCommand.Parameters.AddRange(parameters);
                    da.Fill(ds,"ds");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return ds;
            }
        }
        public static DataSet ExecuteQuery(string sql)
        {
        	using (OleDbConnection connection = new OleDbConnection(connectionString))//先建立Connection对象
			{
				OleDbCommand cmd = new OleDbCommand(sql,connection);//建立Command对象,用Select命令
				using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))//利用Command对象,建立DataAdapter对象
				{
					DataSet ds = new DataSet();//建立DataSet对象
					
					da.Fill(ds, "ds");//填充
					return ds;
				}
			
			}

        	//return ExecuteQuery(sql,null);
        }
        //执行查询语句返回datareader，使用后要注意close
        //这个函数在AccessPageUtils中使用，执行其它查询时最好不要用
        public static OleDbDataReader ExecuteReader(string sql)
        {
        	//Debug.WriteLine(sql);
        	OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(sql, connection);
            try
            {
                connection.Open();
                return  cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }
        
	}
}
