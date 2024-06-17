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
' 	序列信息
'
' 	name: TimeSeries.Model.CollectionInfo
' 	create: 2022-05-10
' 	memo: 序列信息
'
' ------------------------------------------------------------

Namespace Model

	''' <summary>数据库信息</summary>
	Public Class CollectionInfo

		''' <summary>压缩规则</summary>
		Public Class Rule

			''' <summary>目标序列名称</summary>
			Public Property DestKey As String

			''' <summary>有效时长，以毫秒为单位</summary>
			Public Property BucketDuration As Integer

			''' <summary>聚合类型</summary>
			Public Property Aggregation As AggregationEnum

			''' <summary>对齐方式</summary>
			Public Property Alignment As Integer

		End Class

		''' <summary>块</summary>
		Public Class Chunk

			''' <summary>开始时间</summary>
			Public Property StartTimestamp As TimeStamp

			''' <summary>结束时间</summary>
			Public Property EndTimestamp As TimeStamp

			''' <summary>样本总数</summary>
			Public Property Samples As Integer

			''' <summary>块的内部数据大小（没有开销）以字节为单位</summary>
			Public Property Size As Integer

			''' <summary> size 和 samples 的比率</summary>
			Public Property BytesPerSample As Double

		End Class

		''' <summary>时间序列中的样本总数</summary>
		Public Property TotalSamples As Integer = -1

		''' <summary>时间序列分配的总字节数</summary>
		Public Property MemoryUsage As Integer = -1

		''' <summary>此时间序列中出现的第一个时间戳</summary>
		Public Property FirstTimeStamp As TimeStamp

		''' <summary>时间序列中存在的最后一个时间戳</summary>
		Public Property LastTimeStamp As TimeStamp

		''' <summary>时间序列的保留期，以毫秒为单位</summary>
		Public Property RetentionTime As Integer = -1

		''' <summary>此时间序列的内存块数</summary>
		Public Property ChunkCount As Integer = -1

		''' <summary>为数据分配的内存大小，以字节为单位</summary>
		Public Property ChunkSize As Integer = -1

		''' <summary>块类型：compressed 或 uncompressed</summary>
		Public Property ChunkType As EncodingEnum

		''' <summary>时间序列的重复策略</summary>
		Public Property DuplicatePolicy As DuplicatePolicyEnum

		''' <summary>此时间序列的元数据标签的标签值对的嵌套数组</summary>
		Public Property Labels As IDictionary(Of String, Object)

		''' <summary>源时间序列的键名，以防当前序列是压缩规则的目标</summary>
		Public Property SourceKey As String

		''' <summary>此时间序列的元数据标签的标签值对的嵌套数组</summary>
		Public Property Rules As IEnumerable(Of Rule)

		''' <summary>数据集名称</summary>
		Public Property KeySelfName As String

		''' <summary>源时间序列的键名，以防当前序列是压缩规则的目标</summary>
		Public Property Chunks As IEnumerable(Of Chunk)

	End Class

End Namespace