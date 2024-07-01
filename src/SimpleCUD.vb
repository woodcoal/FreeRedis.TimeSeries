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
' 	数据增改删
'
' 	name: TimeSeries.SimpleCUD
' 	create: 2024-06-17
' 	memo: 数据增改删
'
' ------------------------------------------------------------

Imports System.Runtime.CompilerServices
Imports FreeRedis.TimeSeries.Model

Partial Public Module TimeSeries

#Region "增加数据"

	''' <summary>将样本附加到时间序列。当指定键不存在时，将创建一个新的时间序列。（https://redis.io/commands/ts.add/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStamp">时间戳</param>
	''' <param name="value">样本的（双精度）数值数据值。</param>
	''' <param name="options">创建相关参数选项</param>
	''' <returns>返回创建的 ticks 值，-1 表示添加失败</returns>
	<Extension>
	Public Function TSAdd(client As RedisClient, key As String, timeStamp As TimeStamp, value As Double, Optional options As CreateOption = Nothing, Optional ByRef errorMessage As String = "") As TimeStamp
		If String.IsNullOrWhiteSpace(key) Then Return Nothing

		Dim command = New CommandPacket("TS.ADD").InputKey(key).Input(timeStamp.Value, value)
		options?.UpdateCommand(command)

		Return client.ExecteTimeStamp(command, errorMessage)
	End Function

	''' <summary>将样本附加到时间序列。当指定键不存在时，将创建一个新的时间序列。（https://redis.io/commands/ts.add/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStamp">时间戳</param>
	''' <param name="value">样本的（双精度）数值数据值。</param>
	''' <param name="options">创建相关参数选项</param>
	''' <returns>返回创建的 ticks 值，-1 表示添加失败</returns>
	<Extension>
	Public Function TSAdd(client As RedisClient, key As String, timeStamp As Long, value As Double, Optional options As CreateOption = Nothing, Optional ByRef errorMessage As String = "") As TimeStamp
		If String.IsNullOrWhiteSpace(key) Then Return Nothing

		Dim command = New CommandPacket("TS.ADD").InputKey(key).Input(New TimeStamp(timeStamp), value)
		options?.UpdateCommand(command)

		Return client.ExecteTimeStamp(command, errorMessage)
	End Function

	''' <summary>将新样本附加到一个或多个时间序列。（https://redis.io/commands/ts.madd/）</summary>
	<Extension>
	Public Function TSMAdd(client As RedisClient, data As IEnumerable(Of (Key As String, TimeStamp As TimeStamp, Value As Double)), Optional ByRef errorMessage As String = "") As TimeStamp()
		If data Is Nothing OrElse Not data.Any Then Return Nothing

		Dim command = New CommandPacket("TS.MADD")
		For Each item In data
			command.InputKey(item.Key).Input(item.TimeStamp.Value, item.Value)
		Next

		Return client.ExecteTimeStampArray(command, errorMessage)
	End Function

	''' <summary>将新样本附加到一个或多个时间序列。（https://redis.io/commands/ts.madd/）</summary>
	<Extension>
	Public Function TSMAdd(client As RedisClient, ParamArray data() As (Key As String, TimeStamp As TimeStamp, Value As Double)) As TimeStamp()
		Return client.TSMAdd(data, "")
	End Function

	''' <summary>将新样本附加到一个或多个时间序列。（https://redis.io/commands/ts.madd/）</summary>
	<Extension>
	Public Function TSMAdd(client As RedisClient, key As String, data As IDictionary(Of Long, Double), Optional ByRef errorMessage As String = "") As TimeStamp()
		If String.IsNullOrWhiteSpace(key) OrElse data Is Nothing OrElse Not data.Any Then Return Nothing

		Return client.TSMAdd(data.Select(Function(x) (key, New TimeStamp(x.Key), x.Value)), errorMessage)
	End Function

#End Region

#Region "调整数据"

	''' <summary>将最后一个时间数据减去指定值，如果键不存在则创建新键。（https://redis.io/commands/ts.decrby/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="value">样本的（双精度）数值数据值。</param>
	''' <param name="timeStamp">时间戳，必须大于等于最大时间戳，等于时则减去值</param>
	''' <param name="options">创建相关参数选项</param>
	<Extension>
	Public Function TSDecrby(client As RedisClient, key As String, value As Double, Optional timeStamp As TimeStamp = Nothing, Optional options As CreateOption = Nothing, Optional ByRef errorMessage As String = "") As TimeStamp
		If String.IsNullOrWhiteSpace(key) Then Return Nothing

		Dim command = New CommandPacket("TS.DECRBY").InputKey(key).Input(value)
		If timeStamp IsNot Nothing Then command.Input("TIMESTAMP", timeStamp.Value)
		options?.UpdateCommand(command)

		Return client.ExecteTimeStamp(command, errorMessage)
	End Function

	''' <summary>将最后一个时间数据增加指定值，如果键不存在则创建新键。（https://redis.io/commands/ts.incrby/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="value">样本的（双精度）数值数据值。</param>
	''' <param name="timeStamp">时间戳，必须大于等于最大时间戳，等于时则减去值</param>
	''' <param name="options">创建相关参数选项</param>
	<Extension>
	Public Function TSIncrby(client As RedisClient, key As String, value As Double, Optional timeStamp As TimeStamp = Nothing, Optional options As CreateOption = Nothing, Optional ByRef errorMessage As String = "") As TimeStamp
		If String.IsNullOrWhiteSpace(key) Then Return Nothing

		Dim command = New CommandPacket("TS.INCRBY").InputKey(key).Input(value)
		If timeStamp IsNot Nothing Then command.Input("TIMESTAMP", timeStamp.Value)
		options?.UpdateCommand(command)

		Return client.ExecteTimeStamp(command, errorMessage)
	End Function

#End Region

#Region "移除数据"

	''' <summary>删除给定时间序列的两个时间戳之间的所有样本。（https://redis.io/commands/ts.del/）</summary>
	''' <param name="key">时间序列的键名</param>
	''' <param name="timeStart">开始时间戳（包含）</param>
	''' <param name="timeEnd">结束时间戳（包含）</param>
	''' <returns>删除数量</returns>
	<Extension>
	Public Function TSDel(client As RedisClient, key As String, timeStart As TimeStamp, timeEnd As TimeStamp, Optional ByRef errorMessage As String = "") As Long
		If String.IsNullOrWhiteSpace(key) Then Return 0

		Dim command = New CommandPacket("TS.DEL").InputKey(key).Input(timeStart.Value, timeEnd.Value)
		Return client.ExecteLong(command, errorMessage)
	End Function

#End Region

End Module
