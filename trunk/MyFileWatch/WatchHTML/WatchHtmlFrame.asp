<%@LANGUAGE="JAVASCRIPT" CODEPAGE="65001"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
<link href="a.css" rel="stylesheet" type="text/css" />
</head>

<body>

<style>
<!--
.drag{position:relative;cursor:hand
}
#scontentmain{
position:absolute;
width:150px;
}
#scontentbar{
cursor:hand;
position:absolute;
background-color:green;
height:15;
width:100%;
top:0;
font: 9pt;
}
#scontentsub{
position:absolute;
width:100%;
top:15;
background-color:lightyellow;
border:2px solid green;
padding:1.5px;
}
-->
</style>

<script language="JavaScript1.2">
<!--

var dragapproved=false
var zcor,xcor,ycor
function movescontentmain(){
if (event.button==1&&dragapproved){
zcor.style.pixelLeft=tempvar1+event.clientX-xcor
zcor.style.pixelTop=tempvar2+event.clientY-ycor
leftpos=document.all.scontentmain.style.pixelLeft-document.body.scrollLeft
toppos=document.all.scontentmain.style.pixelTop-document.body.scrollTop
return false
}
}
function dragscontentmain(){
if (!document.all)
return
if (event.srcElement.id=="scontentbar"){
dragapproved=true
zcor=scontentmain
tempvar1=zcor.style.pixelLeft
tempvar2=zcor.style.pixelTop
xcor=event.clientX
ycor=event.clientY
document.onmousemove=movescontentmain
}
}
document.onmousedown=dragscontentmain
document.onmouseup=new Function("dragapproved=false")
//-->
</script>
<div id="scontentmain">
<div id="scontentbar" onClick="onoffdisplay()"  align="right">
<span size=1>显示/隐藏</span>
</div>
<div id="scontentsub">
<font face="Arial"><small><small>While we didn't invent JavaScript, we sure as hell
created the best site on IT. <a href="http://www.1stscript.com">First Script</a> is
considered by many online as the definitive JavaScript technology site on the
internet. Online since December, 1997, <a href="http://www.1stscript.com">First Script</a> features over 300+ original scripts, 100+
tutorials on JavaScript programming and web design, and a highly active programming forum
where developers from all over meet and share ideas on their latest projects. Click <b><a href="http://www.1stscript.com">HERE</a></b>
for JavaScript!</small></small></font></p>
</div>
</div>
</div>
<script language="JavaScript1.2">

var w=document.body.clientWidth-195
var h=50


////Do not edit pass this line///////////
w+=document.body.scrollLeft
h+=document.body.scrollTop

var leftpos=w
var toppos=h
scontentmain.style.left=w
scontentmain.style.top=h

function onoffdisplay(){
if (scontentsub.style.display=='') 
scontentsub.style.display='none'
else
scontentsub.style.display=''
}

function staticize(){
w2=document.body.scrollLeft+leftpos
h2=document.body.scrollTop+toppos
scontentmain.style.left=w2
scontentmain.style.top=h2
}
window.onscroll=staticize

</script>

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="clas1">
  	<tr>  
    <td width="100"><em><strong>当前BUG单号:</strong></em></td>
    <td id="bugno" >BUG2011080801</td>  
    <td align="right"><input type="text" name="textfield" value="请输入BUG单号" /><input name="Submit" type="button" class="bt" value="查询" />
<input name="Submit" type="button" class="bt" value="上一个" />
<input name="Submit" type="button" class="bt" value="下一个" /></td>
	</tr>
</table>
<iframe id="frameBord" name="frameBord" frameborder="0" width="100%" height="100%" src="siteInfo.html"></iframe>
<span class="clas1"></span>
</body>
</html>
