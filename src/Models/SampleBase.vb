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
' 	基础样本
'
' 	name: TimeSeries.Model.SampleBase
' 	create: 2024-06-16
' 	memo: 仅包含时间戳与值
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>样本</summary>
	Public Class SampleBase
		Inherits TimeStamp

		''' <summary>值</summary>
		Public Overloads ReadOnly Property Value As Double

		Public Sub New()
			MyBase.New()
		End Sub

		Public Sub New(value As Double)
			MyBase.New()
			Me.Value = value
		End Sub

		Public Sub New(value As Double, ticks As Long)
			MyBase.New(ticks)
			Me.Value = value
		End Sub

		Public Sub New(value As Double, time As Date)
			MyBase.New(time)
			Me.Value = value
		End Sub

	End Class

End Namespace