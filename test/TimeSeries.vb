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
' 	时序测试
'
' 	name: TimeSeries
' 	create: 2024-06-15
' 	memo: 时序测试
'
' ------------------------------------------------------------

Imports FreeRedis.TimeSeries.Model

Public Class TimeSeries
	Public Shared Sub Execute(client As RedisClient)
		Dim info = client.TSInfo("TimeSeries:TEST:key1")
		COLLECTION(client)
		'RULE(client)
		'CREATE(client)
		'UPDATE(client)
		'QUERY(client)
	End Sub

	Public Shared Sub COLLECTION(client As RedisClient)
		Dim paths = {"hunan", "guangdong", "beijing"}
		Dim devs = {"pc", "mobile"}
		Dim apps = {"web", "app", "unknown"}

		Dim opts As New List(Of CreateOption)
		For Each path In paths
			For Each dev In devs
				For Each app In apps
					Dim opt As New CreateOption
					opt.AddLabel("path", path)
					opt.AddLabel("dev", dev)
					opt.AddLabel("app", app)

					opts.Add(opt)
				Next
			Next
		Next

		Dim idx = 1
		Dim message = ""

		CON.Title("数据集创建，调整")

		' 创建
		opts.ForEach(Sub(opt)
						 Dim ret = client.TSCreate($"data:{idx}", opt, message)
						 If ret Then
							 CON.Succ($"创建数据集：data:{idx} 成功")
						 Else
							 CON.Err($"创建数据集：data:{idx} 失败，{message}")
						 End If
						 idx += 1
					 End Sub)

		' 调整
		idx = 1
		opts.ForEach(Sub(opt)
						 Dim ret = client.TSAlert($"data:{idx}", opt, message)
						 If ret Then
							 CON.Succ($"调整数据集：data:{idx} 成功")
						 Else
							 CON.Err($"调整数据集：data:{idx} 失败，{message}")
						 End If
						 idx += 1
					 End Sub)

		' 查询
		CON.Title("数据集查询")
		CON.Info("数据集查询，至少需要存在一个 = 查询才会有结果")

		Dim filter = New LabelFilter("app", True, "web")
		CON.Echo($"查询：{filter}")
		Dim data = client.TSQueryIndex(filter, message)

		If data?.Length > 0 Then
			CON.Info($"查询结果：{String.Join("；", data)}")
		Else
			CON.Warn($"未查询到任何数据集 {message}")
		End If

		CON.Echo()
		Dim filters = {New LabelFilter("app", True, "web"), New LabelFilter("path", False, "hunan", "guangdong")}
		Dim query = String.Join(" ", LabelFilter.Filters2Expression(filters))
		CON.Echo($"查询：{query}")
		data = client.TSQueryIndex(filters, message)
		If data?.Length > 0 Then
			CON.Info($"查询结果：{String.Join("；", data)}")
		Else
			CON.Warn($"未查询到任何数据集 {message}")
		End If

		' 信息
		For I = 1 To 18
			Dim ret = client.TSInfo($"data:{I}", True, message)
			CON.Echo({"数据集信息", $"data:{I}", ret?.FirstTimeStamp.Time, ret?.LastTimeStamp.Time, ret?.TotalSamples, message})
		Next
	End Sub

	Public Shared Sub RULE(client As RedisClient)
		Dim paths = {"hunan", "guangdong", "beijing"}
		Dim devs = {"pc", "mobile"}
		Dim types = {"avg", "max", "count"}

		Dim opts As New List(Of CreateOption)
		For Each path In paths
			For Each dev In devs
				For Each type In types
					Dim opt As New CreateOption
					opt.AddLabel("path", path)
					opt.AddLabel("dev", dev)
					opt.AddLabel("type", type)

					opts.Add(opt)
				Next
			Next
		Next

		Dim idx = 1
		Dim message = ""

		CON.Title("采样规则数据集")
		CON.Info("创建规则前需要先创建规则对应的表")

		' 创建
		opts.ForEach(Sub(opt)
						 Dim ret = client.TSCreate($"rule:{idx}", opt, message)
						 If ret Then
							 CON.Succ($"创建数据集：rule:{idx} 成功")
						 Else
							 CON.Err($"创建数据集：rule:{idx} 失败，{message}")
						 End If
						 idx += 1
					 End Sub)

		' 创建规则
		idx = 1
		opts.ForEach(Sub(opt)
						 Dim agg = If(idx Mod 3 = 0, AggregationEnum.AVG, If(idx Mod 3 = 1, AggregationEnum.MAX, AggregationEnum.COUNT))

						 Dim ret = client.TSCreateRule($"data:{idx}", $"rule:{idx}", agg, 60000, message)
						 If ret Then
							 CON.Succ($"创建规则：data:{idx} 成功")
						 Else
							 CON.Err($"创建规则：data:{idx} 失败，{message}")
						 End If
						 idx += 1
					 End Sub)

		' 移除一个规则
		idx = New Random().Next(1, 18)
		Dim flag = client.TSDeleteRule($"data:{idx}", $"rule:{idx}", message)
		If flag Then
			CON.Succ($"移除规则：data:{idx} 成功")
		Else
			CON.Err($"移除规则：data:{idx} 失败，{message}")
		End If
	End Sub

	Public Shared Sub CREATE(client As RedisClient)
		CON.Title("添加采样数据")

		Dim message = ""
		Dim rnd As New Random()
		Dim list As New List(Of (String, TimeStamp, Double))

		For I = 1 To 18
			Dim v = rnd.Next(-100000, 100000)
			Dim t = client.TSAdd($"data:{I}", New TimeStamp(TimeStampEnum.NOW), v, Nothing, message)
			CON.Echo({"添加单条记录", $"data:{I}", v, t.Ticks, message})

			list.Add(($"data:{I}", New TimeStamp(Date.Now.AddSeconds(-20)), rnd.Next(-100000, 100000)))
			list.Add(($"data:{I}", New TimeStamp(Date.Now.AddSeconds(-10)), rnd.Next(-100000, 100000)))
			list.Add(($"data:{I}", New TimeStamp(Date.Now.AddSeconds(-6)), rnd.Next(-100000, 100000)))
			list.Add(($"data:{I}", New TimeStamp(Date.Now.AddSeconds(-1)), rnd.Next(-100000, 100000)))
		Next

		' 批量添加
		Dim ret = client.TSMAdd(list, message)
		If ret?.Length > 0 Then
			CON.Succ({"批量添加记录成功", String.Join(vbTab, ret.Select(Function(x) x.Time))})
		Else
			CON.Err($"批量添加记录失败 {message}")
		End If
	End Sub

	Public Shared Sub UPDATE(client As RedisClient)
		CON.Title("调整采样数据与删除")

		Dim message = ""
		Dim rnd As New Random()

		For I = 1 To 18
			Dim v = rnd.Next(-100000, 100000)
			client.TSIncrby($"data:{I}", v, Nothing, Nothing, message)
			CON.Echo({"增加记录", $"data:{I}", v, message})
		Next

		For I = 1 To 18
			Dim v = rnd.Next(-100000, 100000)
			client.TSDecrby($"data:{I}", v, Nothing, Nothing, message)
			CON.Echo({"减去记录", $"data:{I}", v, message})
		Next

		' 删除 15 秒前到 5 秒前的数据
		CON.Title("删除 15 秒前到 5 秒前的数据")
		For I = 1 To 18
			Dim v = rnd.Next(-100000, 100000)
			Dim ret = client.TSDel($"data:{I}", New TimeStamp(Date.Now.AddSeconds(-15)), New TimeStamp(Date.Now.AddSeconds(-5)), message)
			CON.Echo({"删除记录", $"data:{I}", ret, message})
		Next

	End Sub

	Public Shared Sub QUERY(client As RedisClient)
		CON.Title("数据查询")

		Dim message = ""
		Dim rnd As New Random()

		For I = 1 To 18
			Dim ret = client.TSGet($"data:{I}", True, message)
			CON.Echo({"最近记录", $"data:{I}", ret?.Time, ret.Value, message})
		Next


		' 查询
		CON.Title("批量查询最近记录")
		CON.Info("批量查询最近记录，至少需要存在一个 = 查询才会有结果")

		Dim filter = New LabelFilter("app", True, "web")
		CON.Echo($"查询：{filter}")
		Dim data = client.TSMGet(filter, Nothing, message)

		If data?.Length > 0 Then
			CON.Info($"查询结果：{String.Join("；", data.Select(Function(x) $"{x.Time}=>{x.Value}"))}")
		Else
			CON.Warn($"未查询到任何数据集 {message}")
		End If

		CON.Echo()
		Dim filters = {New LabelFilter("app", True, "web"), New LabelFilter("path", False, "hunan", "guangdong")}
		Dim query = String.Join(" ", LabelFilter.Filters2Expression(filters))
		CON.Echo($"查询：{query}")
		data = client.TSMGet(filters, New List(Of String), message)
		If data?.Length > 0 Then
			CON.Info($"查询结果：{String.Join("；", data.Select(Function(x) $"{x.Time}=>{x.Value}"))}")
		Else
			CON.Warn($"未查询到任何数据集 {message}")
		End If

		CON.Title("批量查询")
		Dim opt = New QueryOption(New TimeStamp(TimeStampEnum.MIN), New TimeStamp(TimeStampEnum.MAX)) With {
			.Aggregation = (AggregationEnum.AVG, 60000),
			.Latest = True
		}
		For I = 1 To 18
			Dim ret = client.TSRange($"data:{I}", opt, message)
			If ret?.Length > 0 Then
				CON.Info($"查询 data:{I}：{String.Join("；", ret.Select(Function(x) $"{x.Time}=>{x.Value}"))}")
			Else
				CON.Warn($"查询 data:{I} 无任何结果 {message}")
			End If
		Next

		Dim optEx = New QueryOptionEx(New TimeStamp(TimeStampEnum.MIN), New TimeStamp(TimeStampEnum.MAX), New LabelFilter("app", True, "web")) With {
			.Aggregation = (AggregationEnum.AVG, 60000),
			.Latest = True,
			.Groupby = ("dev", AggregationEnum.MAX)
		}

		Dim rets = client.TSMRange(optEx, message)
		If rets?.Length > 0 Then
			CON.Info($"多数据集查询：{rets.Length}")
		Else
			CON.Warn($"多数据集查询 无任何结果 {message}")
		End If
	End Sub


End Class
