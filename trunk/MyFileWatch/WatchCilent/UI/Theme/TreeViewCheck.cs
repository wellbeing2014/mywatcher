/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2012/2/16
 * 时间: 15:16
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WatchCilent.UI.Theme
{
	public static class TreeViewCheck
	{
	    /// <summary>
	    /// 系列节点 Checked 属性控制
	    /// </summary>
	    /// <param name="e"></param>
	    public static void CheckControl(TreeViewEventArgs e)
	    {
	        if (e.Action != TreeViewAction.Unknown)
	        {
	            if (e.Node != null && !Convert.IsDBNull(e.Node))
	            {
	                CheckParentNode(e.Node);
	                if (e.Node.Nodes.Count > 0)
	                {
	                    CheckAllChildNodes(e.Node, e.Node.Checked);
	                }
	            }
	        }
	    }
	
	    #region 私有方法
	
	    //改变所有子节点的状态
	    private static void CheckAllChildNodes(TreeNode pn, bool IsChecked)
	    {
	    	pn.ExpandAll();
	        foreach (TreeNode tn in pn.Nodes)
	        {
	            tn.Checked = IsChecked;
	
	            if (tn.Nodes.Count > 0)
	            {
	                CheckAllChildNodes(tn, IsChecked);
	            }
	        }
	    }
	
	    //改变父节点的选中状态，此处为所有子节点不选中时才取消父节点选中，可以根据需要修改
	    private static void CheckParentNode(TreeNode curNode)
	    {
	        bool bChecked = false;
	
	        if (curNode.Parent != null)
	        {
	            foreach (TreeNode node in curNode.Parent.Nodes)
	            {
	                if (node.Checked)
	                {
	                    bChecked = true;
	                    break;
	                }
	            }
	
	            if (bChecked)
	            {
	                curNode.Parent.Checked = true;
	                CheckParentNode(curNode.Parent);
	            }
	            else
	            {
	                curNode.Parent.Checked = false;
	                CheckParentNode(curNode.Parent);
	            }
	        }
	    }
	
	    public static void  GetSelectedTreeNode(TreeNodeCollection nodes,List<TreeNode> selectedNodes)
		{
		      foreach(TreeNode node in nodes)
		      {
		            if(node.Checked)
		            {
		                  selectedNodes.Add(node);
		            }
		           GetSelectedTreeNode(node.Nodes,selectedNodes);
		      }
		}
	    
	    public static void ExpandParent(TreeNode tn)
	    {
	    	if(tn.Parent!=null)
	    	{
	    		tn.Parent.Expand();
	    		ExpandParent(tn.Parent);
	    	}
	    }
	    
	    #endregion
	}

}
