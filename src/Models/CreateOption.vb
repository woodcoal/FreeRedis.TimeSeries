' ------------------------------------------------------------
'
' 	Copyright © 2021 湖南大沥网络科技有限公司.
' 	[Dali] Is licensed under Mulan PSL v2.
'
' 	  author:	木炭(WOODCOAL)
' 	   email:	i@woodcoal.cn
' 	homepage:	http://www.hunandali.com/
'
' 	请依据 Mulan PSL v2 的条款使用本项目。获取 Mulan PSL v2 请浏览 http://license.coscl.org.cn/MulanPSL2
'
' ------------------------------------------------------------
'
' 	时序集选项
'
' 	name: TimeSeries.CreateOption
' 	create: 2024-06-15
' 	memo: 时序集选项
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>时序集选项</summary>
	Public Class CreateOption

		''' <summary>最后操作时间与最早时间的时间差，单位：毫秒；0：不处理，否则早于此时间差的值都将自动移除</summary>
		Public Property RETENTION As ULong? = 0

		''' <summary>数据压缩方式</summary>
		Public Property ENCODING As EncodingEnum? = EncodingEnum.COMPRESSED

		''' <summary>为每个数据块分配的内存大小</summary>
		''' <remarks>以字节为单位，必须是 [48, 1048576] 范围内的 8 的倍数</remarks>
		Public Property CHUNK_SIZE As UInteger? = 4096

		''' <summary>添加数据时如果有相同时间戳的多个样本的策略</summary>
		Public Property DUPLICATE_POLICY As DuplicatePolicyEnum? = DuplicatePolicyEnum.FIRST

		''' <summary>处理重复样本的政策，新数据忽略条件</summary>
		''' <remarks>用于数据噪声或者精度产生异常值的处理，具体可以参考：https://redis.io/docs/latest/develop/data-types/timeseries/configuration/#ignore_max_time_diff-and-ignore_max_val_diff</remarks>
		Public Property IGNORE As (ignoreMaxTimediff As Double, ignoreMaxValDiff As Double)?

		''' <summary>标签值对</summary>
		Public ReadOnly Property LABELS As New Dictionary(Of String, Object)

		''' <summary>添加标签</summary>
		Public Sub AddLabel(key As String, value As Object)
			If String.IsNullOrWhiteSpace(key) Then Return

			If LABELS.ContainsKey(key) Then
				LABELS(key) = value
			Else
				LABELS.Add(key, value)
			End If
		End Sub

		''' <summary>移除标签</summary>
		Public Sub RemoveLabel(key As String)
			If String.IsNullOrWhiteSpace(key) Then Return
			If LABELS.ContainsKey(key) Then LABELS.Remove(key)
		End Sub

		''' <summary>更新命令行</summary>
		''' <param name="command">命令行</param>
		Public Function UpdateCommand(command As CommandPacket) As CommandPacket
			If command Is Nothing Then Return Nothing

			' RETENTION
			command.InputIf(RETENTION.HasValue, "RETENTION", RETENTION.Value)

			' ENCODING
			command.InputIf(ENCODING.HasValue, "ENCODING", ENCODING.Value.ToString.ToUpper)

			' CHUNK_SIZE 范围需要校验
			If CHUNK_SIZE.HasValue Then
				Dim chunk = CHUNK_SIZE.Value
				If chunk < 48 OrElse chunk > 1048576 Then chunk = 4096
				chunk = Math.Ceiling(chunk / 8) * 8
				If chunk < 48 Then chunk = 48
				If chunk > 1048576 Then chunk = 1048576
				command.Input("CHUNK_SIZE", chunk)
			End If

			'DUPLICATE_POLICY
			command.InputIf(DUPLICATE_POLICY.HasValue, "DUPLICATE_POLICY", DUPLICATE_POLICY.Value.ToString.ToUpper)

			' IGNORE
			If IGNORE.HasValue Then command.Input("IGNORE", IGNORE.Value.ignoreMaxTimediff, IGNORE.Value.ignoreMaxValDiff)

			' LABELS
			If LABELS?.Any Then command.InputKey("LABELS").InputKv(Of Object)(LABELS, True, Nothing)

			Return command
		End Function

	End Class

End Namespace