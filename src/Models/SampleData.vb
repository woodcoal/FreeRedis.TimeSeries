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
' 	样本数据
'
' 	name: TimeSeries.Model.SampleData
' 	create: 2022-05-10
' 	memo: 样本数据
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>样本列表</summary>
	Public Class SampleData

		''' <summary>目标序列名称</summary>
		Public Property Key As String

		''' <summary>对应标签</summary>
		Public Property Labels As Dictionary(Of String, Object)

		''' <summary>值列表</summary>
		Public Property Values As SampleBase()

	End Class

End Namespace