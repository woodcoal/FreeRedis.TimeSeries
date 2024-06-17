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
' 	title
'
' 	name: TimeSeries.ReducerEnum 
' 	create: 2024-06-15
' 	memo: introduce
'
' ------------------------------------------------------------

''' <summary>聚合具有相同标签值的系列的减速器类型</summary>
Public Enum ReducerEnum
	''' <summary>所有值的总和</summary>
	SUM

	''' <summary>最小值</summary>
	MIN

	''' <summary>最大值</summary>
	MAX

End Enum
