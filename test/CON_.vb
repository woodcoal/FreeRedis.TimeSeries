' ------------------------------------------------------------
'
' 	Copyright © 2021 湖南大沥网络科技有限公司.
' 	Dali.Utils Is licensed under Mulan PSL v2.
'
' 	  author:	木炭(WOODCOAL)
' 	   email:	i@woodcoal.cn
' 	homepage:	http://www.hunandali.com/
'
' 	请依据 Mulan PSL v2 的条款使用本项目。获取 Mulan PSL v2 请浏览 http://license.coscl.org.cn/MulanPSL2
'
' ------------------------------------------------------------
'
' 	控制台简化操作
'
' 	name: CONS
' 	create: 2019-03-31
' 	memo: 控制台简化操作
' 	
' ------------------------------------------------------------

''' <summary>控制台简化操作</summary>
Public Class CON

#Region "其他输出"

	''' <summary>清空控制台</summary>
	Public Shared Sub Clear()
		Console.Clear()
		ColorDefault()
		Console.OutputEncoding = Text.Encoding.UTF8
	End Sub

	''' <summary>应用启动</summary>
	Public Shared Sub AppStart(Optional information As String = "")
		Title(If(String.IsNullOrWhiteSpace(information), "应用启动", information))
	End Sub

	''' <summary>应用结束</summary>
	Public Shared Sub AppFinish(Optional showPressKey As Boolean = True)
		Echo("应用执行完成")
		Echo()
		If showPressKey Then Wait("按任意键退出系统...", True)
	End Sub

	''' <summary>等待</summary>
	Public Shared Sub Wait(Optional information As String = "", Optional anyKey As Boolean = False)
		Console.ForegroundColor = ConsoleColor.Gray
		Echo()
		Echo(information)
		Console.ResetColor()

		If anyKey Then
			Console.ReadKey()
		Else
			Console.Read()
		End If
	End Sub

	''' <summary>输出标题</summary>
	Public Shared Sub Title(information As String, Optional titleColor As ConsoleColor = ConsoleColor.Yellow)
		If String.IsNullOrWhiteSpace(information) Then Return

		Dim Left = ((Console.WindowWidth - information.Length) / 2) - 1
		If Left < 1 Then Left = 0

		Dim Start = Console.WindowWidth - 1
		If Start < 1 Then Start = 0

		Console.ForegroundColor = titleColor
		Console.WriteLine()
		Console.WriteLine(New String("#"c, Start))
		Console.WriteLine()
		Console.WriteLine(New String(" "c, Left) & information)
		Console.WriteLine()
		Console.WriteLine(New String("#"c, Start))
		Console.WriteLine()

		ColorDefault()
	End Sub

	''' <summary>输出带时间的内容</summary>
	Public Shared Sub Time(information As String, Optional foregroundColor As ConsoleColor = ConsoleColor.White)
		Console.BackgroundColor = ConsoleColor.DarkGray
		Console.ForegroundColor = ConsoleColor.Blue
		Console.Write(Date.Now.ToString("HH:mm:ss"))

		Console.Write(vbTab)

		Console.BackgroundColor = ConsoleColor.Black
		Console.ForegroundColor = foregroundColor
		Console.Write(information)
		Console.Write(vbCrLf)

		ColorDefault()
	End Sub

	''' <summary>获取用户输入</summary>
	''' <param name="information">提示文本</param>
	''' <param name="isLine">是否含行输出，否则内容跟随其后。</param>
	Public Shared Function Input(Optional information As String = "", Optional isLine As Boolean = True) As String
		If Not String.IsNullOrWhiteSpace(information) Then Echo(information, isLine)
		Return Console.ReadLine
	End Function

#End Region

#Region "颜色（默认/信息/成功/警告/错误）"

	Private Shared Sub ColorDefault()
		Console.ResetColor()
		Console.BackgroundColor = ConsoleColor.Black
		Console.ForegroundColor = ConsoleColor.White
	End Sub


	''' <summary>输出警告</summary>
	Private Shared Sub ColorWarn()
		Console.BackgroundColor = ConsoleColor.Black
		Console.ForegroundColor = ConsoleColor.Yellow
	End Sub

	''' <summary>输出错误</summary>
	Private Shared Sub ColorErr()
		Console.BackgroundColor = ConsoleColor.Black
		Console.ForegroundColor = ConsoleColor.Red
	End Sub

	''' <summary>输出错误</summary>
	Private Shared Sub ColorInfo()
		Console.BackgroundColor = ConsoleColor.Black
		Console.ForegroundColor = ConsoleColor.Blue
	End Sub

	''' <summary>输出错误</summary>
	Private Shared Sub ColorSucc()
		Console.BackgroundColor = ConsoleColor.Black
		Console.ForegroundColor = ConsoleColor.Green
	End Sub

	''' <summary>输出警告标题</summary>
	Private Shared Sub ColorWarnTitle()
		Console.BackgroundColor = ConsoleColor.Yellow
		Console.ForegroundColor = ConsoleColor.Red
	End Sub

	''' <summary>输出错误标题</summary>
	Private Shared Sub ColorErrTitle()
		Console.BackgroundColor = ConsoleColor.White
		Console.ForegroundColor = ConsoleColor.Red
	End Sub

	''' <summary>输出信息标题</summary>
	Private Shared Sub ColorInfoTitle()
		Console.BackgroundColor = ConsoleColor.Gray
		Console.ForegroundColor = ConsoleColor.Blue
	End Sub

	''' <summary>输出错误</summary>
	Private Shared Sub ColorSuccTitle()
		Console.BackgroundColor = ConsoleColor.White
		Console.ForegroundColor = ConsoleColor.Green
	End Sub

#End Region

#Region "输出"

	''' <summary>输出字符到控制台</summary>
	''' <param name="information">输出文本</param>
	''' <param name="isLine">是否含行输出，否则内容跟随其后。</param>
	Public Shared Sub Echo(Optional information As String = "", Optional isLine As Boolean = True)
		If String.IsNullOrWhiteSpace(information) Then
			Console.WriteLine()
		ElseIf isLine Then
			Console.WriteLine(information)
		Else
			Console.Write(information)
		End If

		ColorDefault()
	End Sub

	''' <summary>输出格式化字符到控制台</summary>
	Public Shared Sub Echo(informationListFormat As String, ParamArray informationValues() As Object)
		Echo(String.Format(informationListFormat, informationValues))
	End Sub

	''' <summary>输出一组字符到控制台，并用指定字符区分</summary>
	Public Shared Sub Echo(informationList As Object(), Optional splitString As String = vbTab)
		If informationList?.Length > 0 Then
			Echo(String.Join(splitString, informationList))
		Else
			Echo()
		End If
	End Sub

#End Region

#Region "信息输出"

	''' <summary>信息标题</summary>
	Public Shared Sub InfoTitle(information As String)
		ColorInfoTitle()
		Echo(information)
	End Sub

	''' <summary>信息标题</summary>
	Public Shared Sub InfoTitle(ParamArray informations() As String)
		ColorInfoTitle()
		Echo(informations)
	End Sub

	''' <summary>输出字符到控制台</summary>
	Public Shared Sub Info(Optional information As String = "", Optional isLine As Boolean = True)
		ColorInfo()
		Echo(information, isLine)
	End Sub

	''' <summary>输出格式化字符到控制台</summary>
	Public Shared Sub Info(informationFormat As String, ParamArray informationValues() As Object)
		ColorInfo()
		Echo(informationFormat, informationValues)
	End Sub

	''' <summary>输出一组字符到控制台，并用指定字符区分</summary>
	Public Shared Sub Info(informationList As Object(), Optional splitString As String = vbTab)
		ColorInfo()
		Echo(informationList, splitString)
	End Sub

#End Region

#Region "成功输出"

	''' <summary>成功标题</summary>
	Public Shared Sub SuccTitle(information As String)
		ColorSuccTitle()
		Echo(information)
	End Sub

	''' <summary>警告标题</summary>
	Public Shared Sub SuccTitle(ParamArray informations() As String)
		ColorSuccTitle()
		Echo(informations)
	End Sub

	''' <summary>输出字符到控制台</summary>
	Public Shared Sub Succ(Optional information As String = "", Optional isLine As Boolean = True)
		ColorSucc()
		Echo(information, isLine)
	End Sub

	''' <summary>输出格式化字符到控制台</summary>
	Public Shared Sub Succ(informationFormat As String, ParamArray informationValues() As Object)
		ColorSucc()
		Echo(informationFormat, informationValues)
	End Sub

	''' <summary>输出一组字符到控制台，并用指定字符区分</summary>
	Public Shared Sub Succ(informationList As Object(), Optional splitString As String = vbTab)
		ColorSucc()
		Echo(informationList, splitString)
	End Sub

#End Region

#Region "错误输出"

	''' <summary>错误标题</summary>
	Public Shared Sub ErrTitle(information As String)
		ColorErrTitle()
		Echo(information)
	End Sub

	''' <summary>错误标题</summary>
	Public Shared Sub ErrTitle(ParamArray informations() As String)
		ColorErrTitle()
		Echo(informations)
	End Sub

	''' <summary>输出字符到控制台</summary>
	Public Shared Sub Err(ex As Exception, Optional information As String = "")
		ErrTitle(If(String.IsNullOrWhiteSpace(information), ex.ToString, information))

		If ex IsNot Nothing Then
			Dim st As New StackTrace(ex, True)
			Dim frs = st?.GetFrames
			Dim sf = frs?(frs.Length - 1)

			With New Text.StringBuilder
				.AppendFormat("时间：{0}{1}", Date.Now, vbCrLf)

				If sf IsNot Nothing Then
					.AppendFormat("类名：{0}{1}", sf.GetMethod().DeclaringType.FullName, vbCrLf)
					.AppendFormat("方法：{0}{1}", sf.GetMethod().Name, vbCrLf)
					.AppendFormat("行号：{0}{1}", sf.GetFileLineNumber, vbCrLf)
				End If

				.AppendFormat("异常：{0}{1}", ex.ToString, vbCrLf)


				Err(.ToString)
			End With
		Else
			Info("暂未发生异常")
		End If
	End Sub

	''' <summary>输出字符到控制台</summary>
	Public Shared Sub Err(Optional information As String = "", Optional isLine As Boolean = True)
		ColorErr()
		Echo(information, isLine)
	End Sub

	''' <summary>输出格式化字符到控制台</summary>
	Public Shared Sub Err(informationFormat As String, ParamArray informationValues() As Object)
		ColorErr()
		Echo(informationFormat, informationValues)
	End Sub

	''' <summary>输出一组字符到控制台，并用指定字符区分</summary>
	Public Shared Sub Err(informationList As Object(), Optional splitString As String = vbTab)
		ColorErr()
		Echo(informationList, splitString)
	End Sub

#End Region

#Region "警告输出"

	''' <summary>警告标题</summary>
	Public Shared Sub WarnTitle(information As String)
		ColorWarnTitle()
		Echo(information)
	End Sub

	''' <summary>警告标题</summary>
	Public Shared Sub WarnTitle(ParamArray informations() As String)
		ColorWarnTitle()
		Echo(informations)
	End Sub

	''' <summary>输出字符到控制台</summary>
	Public Shared Sub Warn(Optional information As String = "", Optional isLine As Boolean = True)
		ColorWarn()
		Echo(information, isLine)
	End Sub

	''' <summary>输出格式化字符到控制台</summary>
	Public Shared Sub Warn(informationFormat As String, ParamArray informationValues() As Object)
		ColorWarn()
		Echo(informationFormat, informationValues)
	End Sub

	''' <summary>输出一组字符到控制台，并用指定字符区分</summary>
	Public Shared Sub Warn(informationList As Object(), Optional splitString As String = vbTab)
		ColorWarn()
		Echo(informationList, splitString)
	End Sub

#End Region

End Class
