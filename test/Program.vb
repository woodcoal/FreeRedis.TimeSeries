' ------------------------------------------------------------
'
' 	Copyright © 2024 WOODCOAL.
' 	FreeRedis.TimeSeries Is licensed under Mulan PSL v2.
'
' 	  author:	木炭(WOODCOAL)
' 	   email:	i@woodcoal.cn
' 	homepage:	http://www.woodcoal.cn/
'
' 	请依据 Mulan PSL v2 的条款使用本项目。获取 Mulan PSL v2 请浏览 http://license.coscl.org.cn/MulanPSL2
'
' ------------------------------------------------------------
'
' 	测试
'
' 	name: Program
' 	create: 2024-06-15
' 	memo: 测试
'
' ------------------------------------------------------------

Public Module Program

	''' <summary>Redis 连接字符串</summary>
	Private ReadOnly CONN As String = "10.10.10.1,password=redis"

	Public Sub Main()
		Dim client = CreateClient()

		If client IsNot Nothing Then
			TimeSeries.Execute(client)
		End If

		CON.Wait("按任意键退出系统...", True)
	End Sub

	''' <summary>创建连接</summary>
	Private Function CreateClient() As RedisClient
		Try
			Dim client As New RedisClient(CONN, Nothing)
			If client Is Nothing Then
				CON.Err("创建 RedisClient 失败!")
			Else
				' Ping 检测
				Dim result = client.Ping
				If result = "PONG" Then Return client

				CON.Succ("RedisClient 连接失败!")
			End If
		Catch ex As Exception
			CON.Err(ex.Message)
		End Try

		Return Nothing
	End Function
End Module
