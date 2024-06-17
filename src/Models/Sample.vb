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
' 	标准样本
'
' 	name: TimeSeries.Model.Sample
' 	create: 2024-06-16
' 	memo: 含标签及序列的样本
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>标准样本</summary>
	Public Class Sample
		Inherits SampleBase

		''' <summary>目标序列名称</summary>
		Public Property Key As String

		''' <summary>对应标签</summary>
		Public Property Labels As Dictionary(Of String, Object)

		Public Sub New(value As Double)
			MyBase.New(value)
		End Sub

		Public Sub New(value As Double, ticks As Long)
			MyBase.New(value, ticks)
		End Sub

		Public Sub New(value As Double, time As Date)
			MyBase.New(value, time)
		End Sub

	End Class

End Namespace