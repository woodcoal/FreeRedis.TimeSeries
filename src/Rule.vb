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
' 	采样规则
'
' 	name: TimeSeries.Rule
' 	create: 2024-06-17
' 	memo: 采样规则
'
' ------------------------------------------------------------

Imports System.Runtime.CompilerServices

Partial Public Module TimeSeries

	''' <summary>创建采样规则。（https://redis.io/commands/ts.createrule/）</summary>
	''' <param name="sourceKey">源时间序列的键名</param>
	''' <param name="destKey">目标（采样）时间序列的键名称。它必须在调用 TS.CREATERULE 之前创建。</param>
	''' <param name="aggregation">聚合器，聚合类型</param>
	''' <param name="duration">持续时间，以毫秒为单位</param>
	''' <returns>True: 创建成功；UseDefault 已经存在相同的键；False 创建失败</returns>
	''' <remark>TS.CREATERULE使用非空destKey调用可能会导致未定义的行为；不应将样本显式添加到destKey；只有在创建规则后添加到源系列中的新样本才会被聚合</remark>
	<Extension()>
	Public Function TSCreateRule(client As RedisClient, sourceKey As String, destKey As String, aggregation As AggregationEnum, duration As ULong, Optional ByRef errorMessage As String = "") As Boolean
		If String.IsNullOrWhiteSpace(sourceKey) OrElse String.IsNullOrWhiteSpace(destKey) Then Return False

		Dim command = New CommandPacket("TS.CREATERULE").
			InputKey(sourceKey).
			InputKey(destKey).
			Input("AGGREGATION", GetAggregation(aggregation), duration)

		Return client.ExecteOK(command, errorMessage)
	End Function

	''' <summary>删除采样规则，不会删除采样序列。（https://redis.io/commands/ts.deleterule/）</summary>
	''' <param name="sourceKey">源时间序列的键名</param>
	''' <param name="destKey">目标（采样）时间序列的键名称。它必须在调用 TS.CREATERULE 之前创建。</param>
	<Extension()>
	Public Function TSDeleteRule(client As RedisClient, sourceKey As String, destKey As String, Optional ByRef errorMessage As String = "") As Boolean
		If String.IsNullOrWhiteSpace(sourceKey) OrElse String.IsNullOrWhiteSpace(destKey) Then Return False

		Dim command = New CommandPacket("TS.DELETERULE").
			InputKey(sourceKey).
			InputKey(destKey)

		Return client.ExecteOK(command, errorMessage)
	End Function

End Module
