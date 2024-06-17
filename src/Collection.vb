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
' 	数据集处理
'
' 	name: TimeSeries.Collection
' 	create: 2024-06-17
' 	memo: 数据集处理
'
' ------------------------------------------------------------

Imports System.Runtime.CompilerServices
Imports FreeRedis.TimeSeries.Model


''' <summary>数据集处理</summary>
Partial Public Module TimeSeries

	''' <summary>创建一个新的时间序列。（https://redis.io/commands/ts.create/）</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">创建相关参数选项</param>
	''' <returns>True: 创建成功；False 创建失败</returns>
	''' <remarks>如果一个键已经存在，你会得到一个正常的 Redis 错误回复TSDB: key already exists。您可以使用 Redis EXISTS 命令检查密钥是否存在。</remarks>
	<Extension>
	Public Function TSCreate(client As RedisClient, key As String, Optional options As CreateOption = Nothing, Optional ByRef errorMessage As String = "") As Boolean
		If String.IsNullOrWhiteSpace(key) Then Return False

		Dim command = New CommandPacket("TS.CREATE").InputKey(key)
		options?.UpdateCommand(command)

		Return client.ExecteOK(command, errorMessage)
	End Function

	''' <summary>更新现有时间序列的保留、块大小、重复策略和标签。（https://redis.io/commands/ts.alter/）</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="key">时间序列的键名</param>
	''' <param name="options">创建相关参数选项</param>
	''' <returns>True: 更新成功；False 更新失败</returns>
	<Extension>
	Public Function TSAlert(client As RedisClient, key As String, Optional options As CreateOption = Nothing, Optional ByRef errorMessage As String = "") As Boolean
		If String.IsNullOrWhiteSpace(key) Then Return False

		Dim command = New CommandPacket("TS.ALTER").InputKey(key)
		options?.UpdateCommand(command)

		Return client.ExecteOK(command, errorMessage)
	End Function

	''' <summary>查询所有匹配的序列。（https://redis.io/commands/ts.queryindex/）</summary>
	''' <param name="filters">查询条件</param>
	<Extension>
	Public Function TSQueryIndex(client As RedisClient, filters As IEnumerable(Of LabelFilter), Optional ByRef errorMessage As String = "") As String()
		Dim query = LabelFilter.Filters2Expression(filters)
		If query Is Nothing Then Return Nothing

		Dim command = New CommandPacket("TS.QUERYINDEX").Input(query)
		Return client.ExecteStringArray(command, errorMessage)
	End Function

	''' <summary>查询所有匹配的序列。（https://redis.io/commands/ts.queryindex/）</summary>
	''' <param name="filters">查询条件</param>
	<Extension>
	Public Function TSQueryIndex(client As RedisClient, filter As LabelFilter, Optional ByRef errorMessage As String = "") As String()
		If filter Is Nothing Then Return Nothing

		Dim command = New CommandPacket("TS.QUERYINDEX").Input(filter?.Expression)
		Return client.ExecteStringArray(command, errorMessage)
	End Function

	''' <summary>数据集合信息。（https://redis.io/commands/ts.info/）</summary>
	''' <param name="key">时间序列的键名</param>
	<Extension>
	Public Function TSInfo(client As RedisClient, key As String, Optional debug As Boolean = False, Optional ByRef errorMessage As String = "") As CollectionInfo
		If String.IsNullOrWhiteSpace(key) Then Return Nothing

		Dim command = New CommandPacket("TS.INFO").InputKey(key).InputIf(debug, "DEBUG")
		Return client.ExecteCollectionInfo(command, errorMessage)
	End Function

End Module
