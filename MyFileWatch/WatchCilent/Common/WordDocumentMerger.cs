/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2011-7-19
 * 时间: 13:24
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Text;
using Word;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace WatchCilent.Common
{
	/// <summary>
	/// Description of WordDocumentMerger.
	/// </summary>
	public class WordDocumentMerger
	{
        private ApplicationClass objApp = null;
        public Document objDocLast = null;
        private Document objDocBeforeLast = null;
        object objFalse = false;
        object objMissing = System.Reflection.Missing.Value;
        object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;//末尾
        object oPageBreak = Word.WdBreakType.wdPageBreak;//换页
        object count = 99999999;
        object WdLine = Word.WdUnits.wdParagraph;//;
        object WdLine1 = Word.WdUnits.wdLine;//;
        public WordDocumentMerger()
        {
            objApp = new ApplicationClass();
        }
        #region 打开文件
        public void Open(string tempDoc)
        {
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
        }
        #endregion

        #region 保存文件到输出模板
        public void SaveAs()
        {
			SaveFileDialog sfd = new SaveFileDialog();  
			sfd.Filter = "Word Document(*.doc)|*.doc";  
			sfd.DefaultExt = "Word Document(*.doc)|*.doc";  
			if (sfd.ShowDialog() == DialogResult.OK)  
			{
	            object objMissing = System.Reflection.Missing.Value;
	            object objOutDoc = sfd.FileName;
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
				throw;
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
        public void InsertMerge(string[] arrCopies)
        {
            //object objMissing = Missing.Value;
            object objFalse = false;
            object confirmConversion = false;
            object link = false;
            object attachment = false;
            
            try
            {
            	objApp.Selection.MoveDown(ref WdLine, ref count, ref objMissing);//移动焦点
				Range myRange = objDocLast.Range(ref objMissing, ref objMissing);
				myRange.InsertAfter("\r是我回车的");

				objApp.Selection.MoveDown(ref WdLine, ref count, ref objMissing);//移动焦点				
            	 //打开模板文件
               // Open(tempDoc);
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
//                //保存到输出文件
//                SaveAs();
//                foreach (Document objDocument in objApp.Documents)
//                {
//                    objDocument.Close(
//                      ref objFalse,     //SaveChanges
//                      ref objMissing,   //OriginalFormat
//                      ref objMissing    //RouteDocument
//                      );
//                }
            }
            catch(Exception)
			{
				throw;
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
            InsertMerge(arrFiles);
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
            objApp.Selection.MoveDown(ref WdLine, ref count, ref objMissing);//移动焦点
			objDocLast.Paragraphs.Last.Range.Text = text;
			object oStyleName=heading;
			objDocLast.Paragraphs.Last.Range.set_Style(ref oStyleName);
			
        }
    }
}
