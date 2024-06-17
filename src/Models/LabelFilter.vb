' ------------------------------------------------------------
'
' 	Copyright © 2022 湖南大沥网络科技有限公司.
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
' 	标签筛选
'
' 	name: TimeSeries.Model.LabelFilter
' 	create: 2022-05-11
' 	memo: 标签筛选
'
' ------------------------------------------------------------

Imports System.Text

Namespace Model

	''' <summary>标签筛选</summary>
	Public Class LabelFilter

		'''' <summary>匹配方式，等于（包含）或者不等于（不包含）</summary>
		Public Property Equal As Boolean = True

		'''' <summary>值，多个则使用括号包含</summary>
		Public Property Value As String

		'''' <summary>索引</summary>
		Public Property IndexName As String

		''' <summary>构造标签是否等于此值</summary>
		''' <param name="indexName">索引标签名称</param>
		''' <param name="equal">是否等于</param>
		''' <param name="value">值，为空表示是否包含此标签</param>
		Public Sub New(indexName As String, equal As Boolean, Optional value As String = "")
			Me.IndexName = indexName
			Me.Equal = equal
			Me.Value = value
		End Sub

		''' <summary>构造是否包含其中一个值</summary>
		''' <param name="indexName">索引标签名称</param>
		''' <param name="equal">是否包含</param>
		''' <param name="value">值</param>
		Public Sub New(indexName As String, equal As Boolean, ParamArray value() As String)
			Me.IndexName = indexName
			Me.Equal = equal
			Me.Value = $"({String.Join(","c, value)})"
		End Sub

		''' <summary>生成表达式</summary>
		Public Overrides Function ToString() As String
			Return Expression
		End Function

		''' <summary>生成表达式</summary>
		''' <param name="singleRule">是否生成单挑规则，单条将忽略 Or 逻辑操作</param>
		Public ReadOnly Property Expression As String
			Get
				With New StringBuilder
					.Append(IndexName)
					.Append(If(Equal, "=", "!="))

					If Not String.IsNullOrWhiteSpace(Value) Then
						If Value.Contains(" ") Then
							.Append("""")
							.Append(Value.Replace("""", "'"))
							.Append("""")
						Else
							.Append(Value)
						End If
					End If

					Return .ToString
				End With
			End Get
		End Property

		''' <summary>多条筛选规则生成表达式</summary>
		Public Shared Function Filters2Expression(filters As IEnumerable(Of LabelFilter)) As String()
			If filters.Any Then
				Return filters.Select(Function(x) x.Expression).ToArray
			Else
				Return Nothing
			End If
		End Function
	End Class

End Namespace