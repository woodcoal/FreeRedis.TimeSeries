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
' 	批量搜索条件
'
' 	name: TimeSeries.QueryOptionEx
' 	create: 2024-06-16
' 	memo: 批量搜索条件
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>批量搜索条件</summary>
	Public Class QueryOptionEx
		Inherits QueryOption

		''' <summary>滤器表达式</summary>
		Public Property Filter As IEnumerable(Of LabelFilter)

		''' <summary>是否包含标签</summary>
		Public Property Withlabels As Boolean

		''' <summary>筛选的标签</summary>
		Public Property SelectedLabels As IEnumerable(Of String)

		''' <summary>分组</summary>
		Public Property Groupby As (Label As String, Reduce As AggregationEnum)?

		' 构造函数
		Public Sub New(ParamArray filter As LabelFilter())
			Me.Filter = filter?.ToList
		End Sub

		' 构造函数
		Public Sub New(fromTimestamp As TimeStamp, toTimestamp As TimeStamp, ParamArray filter As LabelFilter())
			MyBase.New(fromTimestamp, toTimestamp)
			Me.Filter = filter?.ToList
		End Sub

		' 构造函数
		Public Sub New(fromTimestamp As Long, toTimestamp As Long, ParamArray filter As LabelFilter())
			MyBase.New(fromTimestamp, toTimestamp)
			Me.Filter = filter?.ToList
		End Sub

		' 构造函数
		Public Sub New(fromTime As Date, toTime As Date, ParamArray filter As LabelFilter())
			MyBase.New(fromTime, toTime)
			Me.Filter = filter?.ToList
		End Sub

		''' <summary>生成表达式</summary>
		Public Overloads ReadOnly Property Expression As Object()
			Get
				Dim exps = MyBase.Expression.ToList

				' FILTER
				If Filter?.Any Then
					exps.Add("FILTER")
					exps.AddRange(LabelFilter.Filters2Expression(Filter))
				End If

				' WITHLABELS
				If Withlabels Then exps.Add("WITHLABELS")

				' SELECTED_LABELS 
				If SelectedLabels?.Any Then
					exps.Add("SELECTED_LABELS")
					exps.AddRange(SelectedLabels)
				End If

				' GROUPBY
				If Groupby.HasValue Then
					exps.Add("GROUPBY")
					exps.Add(Groupby.Value.Label)
					exps.Add("REDUCE")
					exps.Add(Groupby.Value.Reduce.ToString.ToLower)
				End If

				Return exps.ToArray
			End Get
		End Property

	End Class
End Namespace