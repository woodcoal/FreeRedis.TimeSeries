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
' 	时间戳
'
' 	name: TimeSeries.Model.TimeStamp
' 	create: 2022-05-10
' 	memo: 时间戳
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>时间戳</summary>
	Public Class TimeStamp

		''' <summary>1970-1-1 的时间戳数值</summary>
		Protected Const TICKS_19700101 = 621355968000000000

		''' <summary>时间戳类型</summary>
		Protected ReadOnly Property Type As TimeStampEnum

		''' <summary>Js 时间戳（毫秒）</summary>
		Public ReadOnly Property Ticks As Long

		''' <summary>当前时间</summary>
		Public ReadOnly Property Time As Date
			Get
				Return New Date((Ticks * 10000) + TICKS_19700101).ToLocalTime
			End Get
		End Property

		''' <summary>构造，使用当前时间戳</summary>
		''' <param name="type">时间类型</param>
		Public Sub New()
			Type = TimeStampEnum.NOW
			Ticks = Nothing
		End Sub

		''' <summary>构造</summary>
		''' <param name="type">时间类型</param>
		Public Sub New(type As TimeStampEnum)
			If type = TimeStampEnum.UNKNOWN Then Throw New ArgumentException("无效时间类型")

			Me.Type = type
			Ticks = Nothing
		End Sub

		''' <summary>构造</summary>
		''' <param name="ticks">Js 时间戳（毫秒）</param>
		Public Sub New(ticks As Long, Optional throwException As Boolean = True)
			If ticks < 0 AndAlso throwException Then Throw New ArgumentException("无效 Js 时间戳，时间戳必须大于等于 0")

			Type = TimeStampEnum.UNKNOWN
			Me.Ticks = If(ticks < 1, 0, ticks)
		End Sub

		''' <summary>构造</summary>
		''' <param name="time">时间对象</param>
		Public Sub New(time As Date)
			Type = TimeStampEnum.UNKNOWN
			Ticks = (time.ToUniversalTime.Ticks - TICKS_19700101) / 10000

			If Ticks < 0 Then Throw New ArgumentException("无效时间，必须晚于或等于 1970 年")
		End Sub

		''' <summary>获取命令行时间戳值</summary>
		Public ReadOnly Property Value As Object
			Get
				Select Case Type
					Case TimeStampEnum.NOW
						Return "*"

					Case TimeStampEnum.MIN
						Return "-"

					Case TimeStampEnum.MAX
						Return "+"

					Case Else
						Return Ticks
				End Select
			End Get
		End Property

	End Class

End Namespace