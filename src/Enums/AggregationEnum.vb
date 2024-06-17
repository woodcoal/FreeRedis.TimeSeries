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
' 	聚合规则类型枚举
'
' 	name: TimeSeries.AggregationEnum
' 	create: 2024-06-15
' 	memo: 聚合规则类型枚举
'
' ------------------------------------------------------------

''' <summary>聚合规则类型枚举</summary>
Public Enum AggregationEnum
	''' <summary>所有值的算术平均值</summary>
	AVG

	''' <summary>所有值的总和</summary>
	SUM

	''' <summary>最小值</summary>
	MIN

	''' <summary>最大值</summary>
	MAX

	''' <summary>最高值与最低值之差</summary>
	RANGE

	''' <summary>值的数量</summary>
	COUNT

	''' <summary>桶中时间戳最低的值</summary>
	FIRST

	''' <summary>桶中时间戳最高的值</summary>
	LAST

	''' <summary>值的总体标准差</summary>
	STD_P

	''' <summary>样本值的标准偏差</summary>
	STD_S

	''' <summary>值的总体方差</summary>
	VAR_P

	''' <summary>值的样本方差</summary>
	VAR_S

	''' <summary>所有值的时间加权平均值（自 RedisTimeSeries v1.8 起）</summary>
	TWA
End Enum

