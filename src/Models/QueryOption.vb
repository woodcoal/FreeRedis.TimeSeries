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
' 	搜索条件
'
' 	name: TimeSeries.QueryOption
' 	create: 2024-06-16
' 	memo: 搜索条件
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>搜索条件</summary>
	Public Class QueryOption

		''' <summary>起始时间戳（包含），不设置则使用最早时间</summary>
		Public Property TimeStart As TimeStamp

		''' <summary>结束时间戳（包含），不设置则使用最晚时间</summary>
		Public Property TimeEnd As TimeStamp

		''' <summary>对于压缩数据使用最新值</summary>
		Public Property Latest As Boolean

		''' <summary>对于压缩数据使用最新值</summary>
		Public Property FilterTs As IEnumerable(Of TimeStamp)

		''' <summary>对于压缩数据使用最新值</summary>
		Public Property FilterValue As (Min As Double, Max As Double)?

		''' <summary>返回结果的最大数量。</summary>
		Public Property Count As Integer?

		''' <summary>与 AGGREGATION 一起使用，控制聚合时间桶的对齐方式，确保在特定时间边界上对齐。</summary>
		''' <remarks>
		''' start 或 - ：参考时间戳将是查询开始间隔时间（ fromTimestamp ），不能是 -
		''' end 或 + ：参考时间戳将是查询结束间隔时间（ toTimestamp ），不能是 +
		''' 特定时间戳：将参考时间戳与特定时间对齐
		''' 注意：如果未提供，则对齐方式设置为 0
		''' </remarks>
		Public Property Align As String

		''' <summary>将指定聚合类型的结果按桶大小进行聚合。</summary>
		Public Property Aggregation As (aggregator As AggregationEnum, bucketDuration As Long)?

		''' <summary>控制如何报告存储桶时间戳。</summary>
		''' <remarks>
		''' start 或 - ：存储桶的开始时间（默认）
		''' end 或 + ：桶的结束时间
		''' mid 或 ~ ：桶的中间时间（如果不是整数则向下舍入）
		''' </remarks>
		Public Property Buckettimestamp As String

		''' <summary>是否报告空存储桶的聚合</summary>
		Public Property Empty As Boolean = False

		' 构造函数
		Public Sub New()
		End Sub

		' 构造函数
		Public Sub New(fromTimestamp As TimeStamp, toTimestamp As TimeStamp)
			TimeStart = fromTimestamp
			TimeEnd = toTimestamp
		End Sub

		' 构造函数
		Public Sub New(fromTimestamp As Long, toTimestamp As Long)
			TimeStart = New TimeStamp(fromTimestamp)
			TimeEnd = New TimeStamp(toTimestamp)
		End Sub

		' 构造函数
		Public Sub New(fromTime As Date, toTime As Date)
			TimeStart = New TimeStamp(fromTime)
			TimeEnd = New TimeStamp(toTime)
		End Sub

		''' <summary>生成表达式</summary>
		Public ReadOnly Property Expression As Object()
			Get
				Dim start = If(TimeStart, New TimeStamp(TimeStampEnum.MIN))
				Dim finish = If(TimeEnd, New TimeStamp(TimeStampEnum.MAX))
				Dim exps As New List(Of Object) From {start.Value, finish.Value}

				' LATEST
				If Latest Then exps.Add("LATEST")


				' FILTER_BY_TS 
				If FilterTs?.Any Then
					exps.Add("FILTER_BY_TS")
					For Each timestamp As TimeStamp In FilterTs
						exps.Add(timestamp.Ticks)
					Next
				End If

				' FILTER_BY_VALUE
				If FilterValue.HasValue Then
					exps.Add("FILTER_BY_VALUE")
					exps.Add(FilterValue.Value.Min)
					exps.Add(FilterValue.Value.Max)
				End If

				' COUNT
				If Count.HasValue Then
					exps.Add("COUNT")
					exps.Add(Count.Value)
				End If

				' ALIGN 
				If Not String.IsNullOrEmpty(Align) Then
					exps.Add("ALIGN")

					Dim a = Align.Trim
					If Not String.IsNullOrEmpty(a) AndAlso a <> "0" Then exps.Add(Align)
				End If

				' AGGREGATION 
				If Aggregation.HasValue Then
					exps.Add("AGGREGATION")
					exps.Add(GetAggregation(Aggregation.Value.aggregator))
					exps.Add(Aggregation.Value.bucketDuration)
				End If

				' BUCKETTIMESTAMP
				If Not String.IsNullOrEmpty(Buckettimestamp) Then
					exps.Add("BUCKETTIMESTAMP")
					exps.Add(Buckettimestamp)
				End If

				' EMPTY
				If Empty Then exps.Add("EMPTY")

				Return exps.ToArray
			End Get
		End Property

	End Class
End Namespace