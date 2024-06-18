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
' 	数据查询
'
' 	name: TimeSeries.Query
' 	create: 2024-06-17
' 	memo: 数据查询
'
' ------------------------------------------------------------


Imports System.Runtime.CompilerServices
Imports FreeRedis.TimeSeries.Model

''' <summary>数据查询</summary>
Partial Public Module TimeSeries

#Region "查询数值"

	''' <summary>获取最后一个样本。（https://redis.io/commands/ts.get/）</summary>
	''' <param name="key">时间序列的键名</param>
	<Extension>
	Public Function TSGet(client As RedisClient, key As String, Optional latest As Boolean = False, Optional ByRef errorMessage As String = "") As SampleBase
		If String.IsNullOrWhiteSpace(key) Then Return Nothing

		Dim command = New CommandPacket("TS.GET").InputKey(key).InputIf(latest, "LATEST")
		Return client.ExecteSampleBase(command, errorMessage)
	End Function

	''' <summary>获取与特定过滤器匹配的最后一个样本。（https://redis.io/commands/ts.mget/）</summary>
	''' <param name="withLabels">结果是否包含标签</param>
	''' <param name="filters">查询条件</param>
	<Extension>
	Public Function TSMGet(client As RedisClient, filters As IEnumerable(Of LabelFilter), Optional withLabels As IEnumerable(Of String) = Nothing, Optional ByRef errorMessage As String = "") As Sample()
		Dim query = LabelFilter.Filters2Expression(filters)
		If query Is Nothing Then Return Nothing

		Dim command = New CommandPacket("TS.MGET")

		If withLabels IsNot Nothing Then
			If withLabels.Any Then
				command.Input("SELECTED_LABELS")
				command.Input(withLabels.ToArray)
			Else
				command.Input("WITHLABELS")
			End If
		End If

		command.Input("FILTER").Input(query)

		Return client.ExecteSampleArray(command, errorMessage)
	End Function

	''' <summary>获取与特定过滤器匹配的最后一个样本。（https://redis.io/commands/ts.mget/）</summary>
	''' <param name="withLabels">结果是否包含标签</param>
	''' <param name="filters">查询条件</param>
	<Extension>
	Public Function TSMGet(client As RedisClient, filter As LabelFilter, Optional withLabels As IEnumerable(Of String) = Nothing, Optional ByRef errorMessage As String = "") As Sample()
		If filter Is Nothing Then Return Nothing

		Dim command = New CommandPacket("TS.MGET")

		If withLabels IsNot Nothing Then
			If withLabels.Any Then
				command.Input("SELECTED_LABELS")
				command.Input(withLabels.ToArray)
			Else
				command.Input("WITHLABELS")
			End If
		End If

		command.Input("FILTER").Input(filter.Expression)

		Return client.ExecteSampleArray(command, errorMessage)
	End Function

#End Region

#Region "查询指定序列的范围值"

	''' <summary>获取指定序列的范围值。https://redis.io/commands/ts.range/）</summary>
	''' <param name="isRev">是否降序排序</param>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">搜索条件</param>
	<Extension>
	Private Function TSQuery(client As RedisClient, key As String, options As QueryOption, isRev As Boolean, Optional ByRef errorMessage As String = "") As SampleBase()
		If String.IsNullOrWhiteSpace(key) OrElse options Is Nothing Then Return Nothing

		Dim command = New CommandPacket(If(isRev, "TS.REVRANGE", "TS.RANGE")).InputKey(key).Input(options.Expression)
		Return client.ExecteSampleBaseArray(command, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳升序排列（https://redis.io/commands/ts.range/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">搜索条件</param>
	<Extension>
	Public Function TSRange(client As RedisClient, key As String, options As QueryOption, Optional ByRef errorMessage As String = "") As SampleBase()
		Return client.TSQuery(key, options, False, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳升序排列（https://redis.io/commands/ts.range/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStart">开始时间戳（包含）</param>
	''' <param name="timeEnd">结束时间戳（包含）</param>
	<Extension>
	Public Function TSRange(client As RedisClient, key As String, timeStart As TimeStamp, timeEnd As TimeStamp, Optional ByRef errorMessage As String = "") As SampleBase()
		Return client.TSQuery(key, New QueryOption(timeStart, timeEnd), False, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳降序排列（https://redis.io/commands/ts.revrange/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">搜索条件</param>
	<Extension>
	Public Function TSRevRange(client As RedisClient, key As String, options As QueryOption, Optional ByRef errorMessage As String = "") As SampleBase()
		Return client.TSQuery(key, options, True, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳降序排列（https://redis.io/commands/ts.revrange/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStart">开始时间戳（包含）</param>
	''' <param name="timeEnd">结束时间戳（包含）</param>
	<Extension>
	Public Function TSRevRange(client As RedisClient, key As String, timeStart As TimeStamp, timeEnd As TimeStamp, Optional ByRef errorMessage As String = "") As SampleBase()
		Return client.TSQuery(key, New QueryOption(timeStart, timeEnd), True, errorMessage)
	End Function

#End Region

#Region "查询多个序列的范围值"

	''' <summary>获取多个时间序列的范围值。（https://redis.io/commands/ts.mrange/）</summary>
	''' <param name="isRev">是否降序排序</param>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">搜索条件</param>
	<Extension>
	Private Function TSMQuery(client As RedisClient, options As QueryOptionEx, isRev As Boolean, Optional ByRef errorMessage As String = "") As SampleData()
		If options Is Nothing Then Return Nothing

		Dim command = New CommandPacket(If(isRev, "TS.MREVRANGE", "TS.MRANGE")).Input(options.Expression)
		Return client.ExecteSampleDataArray(command, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳升序排列（https://redis.io/commands/ts.range/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">搜索条件</param>
	<Extension>
	Public Function TSMRange(client As RedisClient, options As QueryOptionEx, Optional ByRef errorMessage As String = "") As SampleData()
		Return client.TSMQuery(options, False, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳升序排列（https://redis.io/commands/ts.range/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStart">开始时间戳（包含）</param>
	''' <param name="timeEnd">结束时间戳（包含）</param>
	<Extension>
	Public Function TSMRange(client As RedisClient, timeStart As TimeStamp, timeEnd As TimeStamp, filter As LabelFilter, Optional ByRef errorMessage As String = "") As SampleData()
		Return client.TSMQuery(New QueryOptionEx(timeStart, timeEnd, filter), False, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳降序排列（https://redis.io/commands/ts.revrange/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">搜索条件</param>
	<Extension>
	Public Function TSMRevRange(client As RedisClient, options As QueryOptionEx, Optional ByRef errorMessage As String = "") As SampleData()
		Return client.TSMQuery(options, True, errorMessage)
	End Function

	''' <summary>获取指定序列的范围值。时间戳降序排列（https://redis.io/commands/ts.revrange/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStart">开始时间戳（包含）</param>
	''' <param name="timeEnd">结束时间戳（包含）</param>
	<Extension>
	Public Function TSMRevRange(client As RedisClient, timeStart As TimeStamp, timeEnd As TimeStamp, filter As LabelFilter, Optional ByRef errorMessage As String = "") As SampleData()
		Return client.TSMQuery(New QueryOptionEx(timeStart, timeEnd, filter), True, errorMessage)
	End Function

#End Region

End Module
