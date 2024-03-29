﻿/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-7-19
 * 时间: 13:24
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Word;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Graph;


namespace WatchCore.Common
{
	/// <summary>
	/// Description of WordDocumentMerger.
	/// </summary>
	public class WordDocumentMerger
	{
		//缺陷列表的DOC路径
		private string unitDOCpath;
		
		public string UnitDOCpath {
			get { return unitDOCpath; }
			set { unitDOCpath = value; }
		}
		
        public ApplicationClass objApp = null;
        public Document objDocLast = null;
        private Document objDocBeforeLast = null;
        object objFalse = false;
        object objMissing = System.Reflection.Missing.Value;
        object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;//末尾
        object oPageBreak = Word.WdBreakType.wdPageBreak;//换页
        object WdPara = Word.WdUnits.wdParagraph;//段落;
        object WdLine = Word.WdUnits.wdCharacter;//行;
        public WordDocumentMerger()
        {
            objApp = new ApplicationClass();
        }
        #region 打开文件
        public void Open(string tempDoc)
        {
        	try {
        		 object objTempDoc = tempDoc;
           		 objDocLast = objApp.Documents.Open(
                 ref objTempDoc,    //FileName
                 ref objMissing,   //ConfirmVersions
                 ref objMissing,   //ReadOnly
                 ref objMissing,   //AddToRecentFiles
                 ref objMissing,   //PasswordDocument
                 ref objMissing,   //PasswordTemplate
                 ref objMissing,   //Revert
                 ref objMissing,   //WritePasswordDocument
                 ref objMissing,   //WritePasswordTemplate
                 ref objMissing,   //Format
                 ref objMissing,   //Enconding
                 ref objMissing,   //Visible
                 ref objMissing,   //OpenAndRepair
                 ref objMissing,   //DocumentDirection
                 ref objMissing,   //NoEncodingDialog
                 ref objMissing    //XMLTransform
                 );
            	objDocLast.Activate();
        	} catch (Exception) {
        		
        		throw(new Exception("打开模版："+tempDoc+"出错！"));
        	}
           
        }
        #endregion

        #region 保存文件到输出模板
        public void SaveAs(string reportPath)
        {
        	try {
	            object objMissing = System.Reflection.Missing.Value;
	            object objOutDoc =reportPath;
	            objDocLast.SaveAs(
	              ref objOutDoc,      //FileName
	              ref objMissing,     //FileFormat
	              ref objMissing,     //LockComments
	              ref objMissing,     //PassWord    
	              ref objMissing,     //AddToRecentFiles
	              ref objMissing,     //WritePassword
	              ref objMissing,     //ReadOnlyRecommended
	              ref objMissing,     //EmbedTrueTypeFonts
	              ref objMissing,     //SaveNativePictureFormat
	              ref objMissing,     //SaveFormsData
	              ref objMissing,     //SaveAsAOCELetter,
	              ref objMissing,     //Encoding
	              ref objMissing,     //InsertLineBreaks
	              ref objMissing,     //AllowSubstitutions
	              ref objMissing,     //LineEnding
	              ref objMissing      //AddBiDiMarks
	              );
        		
        	} catch (Exception e) {
        		
        		throw(new Exception("保存失败！"+e.ToString()));
        	}
			
        }
        
        #region 保存到指定路径
        public void Save(string path)
        {
        	try {
	            object objOutDoc = path;
	            objDocLast.SaveAs(
	              ref objOutDoc,      //FileName
	              ref objMissing,     //FileFormat
	              ref objMissing,     //LockComments
	              ref objMissing,     //PassWord    
	              ref objMissing,     //AddToRecentFiles
	              ref objMissing,     //WritePassword
	              ref objMissing,     //ReadOnlyRecommended
	              ref objMissing,     //EmbedTrueTypeFonts
	              ref objMissing,     //SaveNativePictureFormat
	              ref objMissing,     //SaveFormsData
	              ref objMissing,     //SaveAsAOCELetter,
	              ref objMissing,     //Encoding
	              ref objMissing,     //InsertLineBreaks
	              ref objMissing,     //AllowSubstitutions
	              ref objMissing,     //LineEnding
	              ref objMissing      //AddBiDiMarks
	              );
        		
        	} catch (Exception) {
        		
        		throw(new Exception("保存失败！"));
        	}
			
        }
         #endregion
        /// <summary>
        /// 退出
        /// </summary>
        public void Quit()
        {
        	 foreach (Document objDocument in objApp.Documents)
            {
                objDocument.Close(
                  ref objFalse,     //SaveChanges
                  ref objMissing,   //OriginalFormat
                  ref objMissing    //RouteDocument
                  );
            }
            objApp.Quit(
              ref objMissing,     //SaveChanges
              ref objMissing,     //OriginalFormat
              ref objMissing      //RoutDocument
              );
            objApp = null;
        }
        #endregion

        #region 循环合并多个文件（复制合并重复的文件）
        ///
        /// 循环合并多个文件（复制合并重复的文件）
        ///
        /// 模板文件
        /// 需要合并的文件
        /// 合并后的输出文件
        public void CopyMerge( string[] arrCopies)
        {
            //object objMissing = Missing.Value;
            
            object objTarget = WdMergeTarget.wdMergeTargetSelected;
            object objUseFormatFrom = WdUseFormattingFrom.wdFormattingFromSelected;
            try
            {
                //打开模板文件
               // Open(tempDoc);
                foreach (string strCopy in arrCopies)
                {
                    objDocLast.Merge(
                      strCopy,                //FileName   
                      ref objTarget,          //MergeTarget
                      ref objMissing,         //DetectFormatChanges
                      ref objUseFormatFrom,   //UseFormattingFrom
                      ref objMissing          //AddToRecentFiles
                      );
                    objDocBeforeLast = objDocLast;
                    objDocLast = objApp.ActiveDocument;
                    if (objDocBeforeLast != null)
                    {
                        objDocBeforeLast.Close(
                          ref objFalse,     //SaveChanges
                          ref objMissing,   //OriginalFormat
                          ref objMissing    //RouteDocument
                          );
                    }
                }
                //保存到输出文件
              //  SaveAs();
                
            }
            catch(Exception)
			{
            	throw(new Exception("复制合并出错！"));
			}
        }
        ///
        /// 循环合并多个文件（复制合并重复的文件）
        ///
        /// 模板文件
        /// 需要合并的文件
        /// 合并后的输出文件
        public void CopyMerge(string tempDoc, string strCopyFolder, string outDoc)
        {
            string[] arrFiles = Directory.GetFiles(strCopyFolder);
            CopyMerge( arrFiles);
        }
        #endregion

        #region 循环合并多个文件（插入合并文件）
        ///
        /// 循环合并多个文件（插入合并文件）
        ///
        /// 模板文件
        /// 需要合并的文件
        /// 合并后的输出文件
        public void InsertMerge(string[] arrCopies,string parLableName)
        {
        	object lableName = parLableName; 
        	int start = objDocLast.Content.End;
        	if(lableName!=null)
        	{
				Bookmark bm = objDocLast.Bookmarks.get_Item(ref lableName);//返回标签
        		start = bm.Start;
        	}
			//object objMissing = Missing.Value;
            object objFalse = false;
            object confirmConversion = false;
            object link = false;
            object attachment = false;
            
            try
            {
            	
            	objApp.Selection.Start = start;
                foreach (string strCopy in arrCopies)
                {
                    objApp.Selection.InsertFile(
                        strCopy,
                        ref objMissing,
                        ref confirmConversion,
                        ref link,
                        ref attachment
                        );
                }
                object restart = start;
                Word.Range rg = objDocLast.Range(ref restart,ref objMissing);
                object oStyleName = "正文";
                rg.set_Style(ref oStyleName);

            }
            catch(Exception e)
			{
            	throw(new Exception("生成报告出错！"+e.ToString()));
			}
        }
        ///
        /// 循环合并多个文件（插入合并文件）
        ///
        /// 模板文件
        /// 需要合并的文件
        /// 合并后的输出文件
        public void InsertMerge(string strCopyFolder)
        {
            string[] arrFiles = Directory.GetFiles(strCopyFolder);
            InsertMerge(arrFiles,null);
        }
        #endregion
        
        public void WriteIntoMarkBook(string parLableName, string parFillName)  
		{  
			object lableName = parLableName;  
			Bookmark bm = objDocLast.Bookmarks.get_Item(ref lableName);//返回标签  
			bm.Range.Text = parFillName;//设置域标签的内容			
		}  
        public void AppendText(string text,string heading)
        {
           // objApp.Selection.EndOf(ref objMissing,ref objMissing);//移动焦点至文档最后
			Paragraph my  = objDocLast.Content.Paragraphs.Add(ref objMissing);//添加一个段落
			my.Range.Text = text;//设置文本信息
			object oStyleName=heading; //新建一个样式
			my.Range.set_Style(ref oStyleName);//设置段落样式
			
        }
        //转化
        public static void WordToHtmlFile(string WordFilePath,string htmlpath)
        {
            try
            {
                Word.Application newApp = new Word.Application();
                // 指定原文件和目标文件
                object Source = WordFilePath;
                //string SaveHtmlPath = WordFilePath.Substring(0, WordFilePath.Length - 3) + "html";
                object Target = htmlpath;

                // 缺省参数  
                object Unknown = Type.Missing;

                //为了保险,只读方式打开
                object readOnly = true;

                // 打开doc文件
                Word.Document doc = newApp.Documents.Open(ref Source, ref Unknown,
                     ref readOnly, ref Unknown, ref Unknown,
                     ref Unknown, ref Unknown, ref Unknown,
                     ref Unknown, ref Unknown, ref Unknown,
                     ref Unknown, ref Unknown, ref Unknown,
                     ref Unknown, ref Unknown);

                // 指定另存为格式(rtf)
                object format = Word.WdSaveFormat.wdFormatHTML;
                // 转换格式
                doc.SaveAs(ref Target, ref format,
                        ref Unknown, ref Unknown, ref Unknown,
                        ref Unknown, ref Unknown, ref Unknown,
                        ref Unknown, ref Unknown, ref Unknown,
                        ref Unknown, ref Unknown, ref Unknown,
                        ref Unknown, ref Unknown);

                // 关闭文档和Word程序
                doc.Close(ref Unknown, ref Unknown, ref Unknown);
                newApp.Quit(ref Unknown, ref Unknown, ref Unknown);
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message); 
            }
        }
		/// <summary>
		/// 插入更新包列表的方法
		/// </summary>
		/// <param name="tableindex"></param>
		/// <param name="dtable"></param>
        public void insertTableForPack(string parLableName,System.Data.DataTable dtable)
        {
        	object lableName = parLableName;  
			Bookmark bm = objDocLast.Bookmarks.get_Item(ref lableName);//返回标签  
			Word.Table wtable = bm.Range.Tables[1];
        	object wrow = wtable.Rows.Last;
        	wtable.Rows.Last.Select();
        	object rownum =dtable.Rows.Count;
        	objApp.Selection.InsertRowsBelow(ref rownum);
        	
        	for (int i = 0; i < dtable.Rows.Count; i++) {
        		wtable.Cell(i+2, 1).Range.Text = (i+1).ToString();
        		wtable.Cell(i+2, 2).Range.Text = dtable.Rows[i][0].ToString();
        		wtable.Cell(i+2, 3).Range.Text = dtable.Rows[i][1].ToString();
        		wtable.Cell(i+2, 4).Range.Text = dtable.Rows[i][2].ToString();
//        		object range =wtable.Cell(i+2, 2).Range;
//        		object adr = @"\\192.10.110.206\e\测试文档\DOC\BUG2011080901.doc";
//				object adr1 = @"\\192.10.110.206\";        		
//        		objApp.Selection.Hyperlinks.Add(range,ref adr,ref objMissing,ref objMissing,
//        		                               	ref adr1,ref objMissing	);
        	}
    	}
        /// <summary>
        /// 插入测试列表的方法。
        /// </summary>
        /// <param name="tableindex"></param>
        /// <param name="dtable"></param>
    	public void insertTableForTest(string parLableName,System.Data.DataTable dtable)
        {
    		object lableName = parLableName;  
			Bookmark bm = objDocLast.Bookmarks.get_Item(ref lableName);//返回标签  
			Word.Table wtable = bm.Range.Tables[1];
        //	object wrow = wtable.Rows.Last;
        	wtable.Rows.Last.Select();
        	object rownum =dtable.Rows.Count;
        	objApp.Selection.InsertRowsBelow(ref rownum);
        	
        	for (int i = 0; i < dtable.Rows.Count; i++) {
        		//超链接(编号)
        		object range =wtable.Cell(i+2, 1).Range;
        		object unitno = dtable.Rows[i][0].ToString();     
        		object address = unitDOCpath+"\\"+unitno+".doc";
        		objApp.Selection.Hyperlinks.Add(range,ref address,ref objMissing,ref objMissing,
        		                               	ref unitno,ref objMissing	);
        		wtable.Cell(i+2, 2).Range.Text = dtable.Rows[i][1].ToString();//等级
        		wtable.Cell(i+2, 3).Range.Text = dtable.Rows[i][2].ToString();//名称
        		wtable.Cell(i+2, 4).Range.Text = dtable.Rows[i][3].ToString();//title
        	}
        }
    	/// <summary>
    	/// 插入图表数据的方法。
    	/// </summary>
    	/// <param name="parLableName"></param>
    	/// <param name="dt"></param>
    	public void InsertChartFromBK(string parLableName,System.Data.DataTable dt)  
		{  
    		
			object lableName = parLableName;  
			Bookmark bm = objDocLast.Bookmarks.get_Item(ref lableName);//返回标签  
			bm.Select();
			Word.InlineShape oShape =bm.Range.InlineShapes[1];
			oShape.OLEFormat.Application.Visible = false;
			oShape.OLEFormat.Activate();
			Chart _testChart =(Chart)oShape.OLEFormat.Object;
           	Graph.Application _testApp = _testChart.Application; 
			_testApp.DataSheet.Cells.Clear();//清空表格的初始数据           	
           	for (int a = 0; a < dt.Columns.Count; a++)//初始化列名
            {
                _testApp.DataSheet.Cells[1, a + 1] = dt.Columns[a].ColumnName;
            }
            for (int i = 0; i < dt.Rows.Count; i++)//填充数据
           	{
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    _testApp.DataSheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                }
            }
           _testApp.Update();
           _testApp.Quit();
    	}
    	
    	/// <summary>
    	/// 插入一般表格的方法。
    	/// </summary>
    	/// <param name="parLableName"></param>
    	/// <param name="dt"></param>
    	public void insertTable(string parLableName,System.Data.DataTable dt)  
		{  
			object lableName = parLableName;  
			Bookmark bm = objDocLast.Bookmarks.get_Item(ref lableName);//返回标签  
			Table table =bm.Range.Tables[1];
			table.Rows.Last.Select();
        	object rownum =dt.Rows.Count-1;
        	objApp.Selection.InsertRowsBelow(ref rownum);
        	
        	for (int i = 0; i < dt.Rows.Count; i++)//填充数据
           	{
	            for (int j = 0; j < table.Columns.Count; j++)
	            {
	            	table.Cell(i + 2, j + 1).Range.Text = dt.Rows[i][j].ToString();
	            }
            }
        	
    	}
    	
    }
}
