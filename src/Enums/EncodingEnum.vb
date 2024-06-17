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
' 	数据压缩方式
'
' 	name: TimeSeries.EncodingEnum
' 	create: 2024-06-15
' 	memo: 数据压缩方式，一般都使用 COMPRESSED
'
' ------------------------------------------------------------


''' <summary>数据压缩方式</summary>
''' <remarks>
''' COMPRESSED 几乎总是正确的选择。
''' 压缩不仅可以节省内存，而且由于内存访问次数减少，通常还可以提高性能。它可以减少大约 90% 的内存。
''' 例外情况是高度不规则的时间戳或值，这种情况很少发生。
''' </remarks>
Public Enum EncodingEnum
	''' <summary>对系列样本施加压缩</summary>
	COMPRESSED

	''' <summary>将原始样本保存在内存中。添加此标志可将数据保留为未压缩形式</summary>
	UNCOMPRESSED
End Enum

