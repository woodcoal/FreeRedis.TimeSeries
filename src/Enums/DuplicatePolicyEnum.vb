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
' 	相同时间戳数据插入策略枚举
'
' 	name: TimeSeries.DuplicatePolicyEnum
' 	create: 2024-06-15
' 	memo: 相同时间戳数据插入策略枚举
'
' ------------------------------------------------------------

''' <summary>相同时间戳数据插入策略枚举</summary>
Public Enum DuplicatePolicyEnum
	''' <summary>阻止，忽略任何新报告的值并回复错误</summary>
	BLOCK

	''' <summary>仅使用最早的值</summary>
	FIRST

	''' <summary>仅使用最后的值</summary>
	LAST

	''' <summary>仅当值低于现有值时才覆盖</summary>
	MIN

	''' <summary>仅当值高于现有值时才覆盖</summary>
	MAX

	''' <summary>如果存在先前的样本，则将新样本添加到其中，以使更新的值等于（先前的 + 新的）。如果不存在先前的样本，则将更新值设置为等于新值。</summary>
	SUM
End Enum
