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
' 	时间戳类型
'
' 	name: TimeSeries.TimeStampEnum
' 	create: 2024-06-15
' 	memo: 时间戳类型
'
' ------------------------------------------------------------

''' <summary>时间戳类型</summary>
Public Enum TimeStampEnum
	''' <summary>未知/自定义</summary>
	UNKNOWN

	''' <summary>* 当前服务器时间</summary>
	NOW

	''' <summary>- 最小时间戳</summary>
	MIN

	''' <summary>+ 最大时间戳</summary>
	MAX
End Enum
