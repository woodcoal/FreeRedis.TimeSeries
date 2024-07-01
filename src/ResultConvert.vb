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
' 	结果转换相关操作
'
' 	name: TimeSeries.ResultConvert
' 	create: 2024-06-17
' 	memo: 结果转换相关操作
'
' ------------------------------------------------------------

Imports System.Runtime.CompilerServices
Imports FreeRedis.TimeSeries.Model

Partial Public Module TimeSeries

	''' <summary>执行并转换成指定类型</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function Execte(Of T)(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As T
		Try
			errorMessage = ""
			Return client.Call(command, Function(x) x.ThrowOrValue(Of T))
		Catch ex As Exception
			errorMessage = ex.Message
			Return Nothing
		End Try
	End Function

	''' <summary>执行并返回长整数</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteLong(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As Long
		Return client.Execte(Of Long)(command, errorMessage)
	End Function

	''' <summary>执行并检查是否成功</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteOK(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As Boolean
		Return client.Execte(Of String)(command, errorMessage) = "OK"
	End Function

	''' <summary>执行并返回字符串数组</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteStringArray(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As String()
		Return client.Execte(Of String())(command, errorMessage)
	End Function

	''' <summary>执行并返回时间戳</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteTimeStamp(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As TimeStamp
		Dim value = client.Execte(Of Long)(command, errorMessage)
		Return If(value < 0, Nothing, New TimeStamp(value, False))
	End Function

	''' <summary>执行并返回时间戳数组</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteTimeStampArray(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As TimeStamp()
		Dim value = client.Execte(Of Long())(command, errorMessage)
		Return value?.Select(Function(x) New TimeStamp(x, False)).ToArray
	End Function

	''' <summary>执行并返回基础样本</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteSampleBase(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As SampleBase
		Dim value = client.Execte(Of Object())(command, errorMessage)
		Return GetSampleBase(value)
	End Function

	''' <summary>执行并返回标准样本</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteSample(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As Sample
		Dim value = client.Execte(Of Object())(command, errorMessage)
		Return GetSample(value)
	End Function

	''' <summary>执行并返回标准数据</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteSampleData(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As SampleData
		Dim value = client.Execte(Of Object())(command, errorMessage)
		Return GetSampleData(value)
	End Function

	''' <summary>执行并返回基础样本数组</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteSampleBaseArray(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As SampleBase()
		Dim value = client.Execte(Of Object())(command, errorMessage)
		Return GetSampleBases(value)
	End Function

	''' <summary>执行并返回标准样本数组</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteSampleArray(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As Sample()
		Dim value = client.Execte(Of Object())(command, errorMessage)
		Return value?.Select(Function(x) GetSample(x)).Where(Function(x) x IsNot Nothing).ToArray
	End Function

	''' <summary>执行并返回样本数据数组</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteSampleDataArray(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As SampleData()
		Dim value = client.Execte(Of Object())(command, errorMessage)
		Return value?.Select(Function(x) GetSampleData(x)).Where(Function(x) x IsNot Nothing).ToArray
	End Function

	''' <summary>执行并返回数据集信息</summary>
	''' <param name="client">FreeRedis 客户端</param>
	''' <param name="command">命令</param>
	''' <param name="errorMessage">异常消息内容</param>
	<Extension>
	Public Function ExecteCollectionInfo(client As RedisClient, command As CommandPacket, Optional ByRef errorMessage As String = "") As CollectionInfo
		Dim value = client.Execte(Of Object())(command, errorMessage)
		If value Is Nothing OrElse value.Length < 2 Then Return Nothing

		Dim Info As New CollectionInfo
		For I = 0 To value.Length - 2 Step 2
			Dim key = value(I).ToString
			Dim item = value(I + 1)

			Select Case key
				Case "totalSamples"
					Info.TotalSamples = Convert.ToInt32(item)

				Case "memoryUsage"
					Info.MemoryUsage = Convert.ToInt32(item)

				Case "firstTimestamp"
					Info.FirstTimeStamp = New TimeStamp(Convert.ToInt64(item), False)

				Case "lastTimestamp"
					Info.LastTimeStamp = New TimeStamp(Convert.ToInt64(item), False)

				Case "retentionTime"
					Info.RetentionTime = Convert.ToInt32(item)

				Case "chunkCount"
					Info.ChunkCount = Convert.ToInt32(item)

				Case "chunkSize"
					Info.ChunkSize = Convert.ToInt32(item)

				Case "chunkType"
					Info.ChunkType = If("UNCOMPRESSED".Equals(item?.ToString, StringComparison.OrdinalIgnoreCase), EncodingEnum.UNCOMPRESSED, EncodingEnum.COMPRESSED)

				Case "duplicatePolicy"
					Info.DuplicatePolicy = GetDuplicatePolicyEnum(item)

				Case "labels"
					Info.Labels = GetLabels(item)

				Case "sourceKey"
					Info.SourceKey = item

				Case "rules"
					Info.Rules = GetRules(item)

				Case "keySelfName"
					Info.KeySelfName = item

				Case "Chunks"
					Info.Chunks = GetChunk(item)
			End Select
		Next

		Return Info
	End Function

	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

	''' <summary>获取聚合规则值</summary>
	Public Function GetAggregation(aggregation As AggregationEnum) As String
		Return aggregation.ToString.ToLower.Replace("_", ".")
	End Function

	''' <summary>获取标签组</summary>
	Private Function GetLabels(value As Object) As Dictionary(Of String, Object)
		Dim data = TryCast(value, Object())
		Return data?.Cast(Of Object()).
			Where(Function(x) x?.Length > 1).
			ToDictionary(Function(x) x(0).ToString, Function(x) x(1))
	End Function

	''' <summary>获取基础样本</summary>
	Private Function GetSampleBase(value As Object) As SampleBase
		Dim data = TryCast(value, Object())
		If data?.Length > 1 Then
			Return New SampleBase(Convert.ToDouble(data(1)), Convert.ToInt64(data(0)))
		Else
			Return Nothing
		End If
	End Function

	''' <summary>获取基础样本数组</summary>
	Private Function GetSampleBases(value As Object) As SampleBase()
		Dim data = TryCast(value, Object())
		If data?.Length > 0 Then
			Return data.Cast(Of Object()).Select(Function(x) GetSampleBase(x)).Where(Function(x) x IsNot Nothing).ToArray
		Else
			Return Nothing
		End If
	End Function

	''' <summary>获取标准样本</summary>
	Private Function GetSample(value As Object) As Sample
		Dim data = TryCast(value, Object())
		If data?.Length > 2 Then
			Dim key = data(0)?.ToString
			Dim labels = GetLabels(data(1))
			Dim values As Object() = data(2)
			If values?.Length > 1 Then Return New Sample(Convert.ToDouble(values(1)), Convert.ToInt64(values(0))) With {.Labels = labels, .Key = key}
		End If

		Return Nothing
	End Function

	''' <summary>获取样本数据</summary>
	Private Function GetSampleData(value As Object) As SampleData
		Dim data = TryCast(value, Object())

		If data?.Length > 2 Then
			Dim key = data(0)?.ToString
			Dim labels = GetLabels(data(1))
			Dim values = GetSampleBases(data(2))
			If values?.Length > 1 Then Return New SampleData With {.Labels = labels, .Key = key, .Values = values}
		End If

		Return Nothing
	End Function

	''' <summary>获取重复值枚举</summary>
	Private Function GetDuplicatePolicyEnum(value As Object) As DuplicatePolicyEnum?
		Dim name = value?.ToString
		If String.IsNullOrWhiteSpace(name) Then Return Nothing

		Select Case name.Trim.ToUpper
			Case "BLOCK"
				Return DuplicatePolicyEnum.BLOCK

			Case "FIRST"
				Return DuplicatePolicyEnum.FIRST

			Case "LAST"
				Return DuplicatePolicyEnum.LAST

			Case "MIN"
				Return DuplicatePolicyEnum.MIN

			Case "MAX"
				Return DuplicatePolicyEnum.MAX

			Case "SUM"
				Return DuplicatePolicyEnum.SUM

			Case Else
				Return Nothing
		End Select
	End Function

	''' <summary>获取采样规则枚举</summary>
	Private Function GetAggregationEnum(value As Object) As AggregationEnum?
		Dim name = value?.ToString
		If String.IsNullOrWhiteSpace(name) Then Return Nothing

		Select Case name.Trim.ToUpper
			Case "AVG"
				Return AggregationEnum.AVG

			Case "SUM"
				Return AggregationEnum.SUM

			Case "MIN"
				Return AggregationEnum.MIN

			Case "MAX"
				Return AggregationEnum.MAX

			Case "RANGE"
				Return AggregationEnum.RANGE

			Case "COUNT"
				Return AggregationEnum.COUNT

			Case "FIRST"
				Return AggregationEnum.FIRST

			Case "LAST"
				Return AggregationEnum.LAST

			Case "STD.P"
				Return AggregationEnum.STD_P

			Case "STD.S"
				Return AggregationEnum.STD_S

			Case "VAR.P"
				Return AggregationEnum.VAR_P

			Case "VAR.S"
				Return AggregationEnum.VAR_S

			Case "TWA"
				Return AggregationEnum.TWA

			Case Else
				Return Nothing
		End Select

	End Function

	''' <summary>获取规则</summary>
	Private Function GetRules(value As Object) As List(Of CollectionInfo.Rule)
		Dim data = TryCast(value, Object())
		Return data?.Cast(Of Object()).
			Where(Function(x) x?.Length > 3).
			Select(Function(x) New CollectionInfo.Rule With {
					.DestKey = x(0),
					.BucketDuration = Convert.ToInt32(x(1)),
					.Aggregation = GetAggregationEnum(x(2)),
					.Alignment = Convert.ToInt32(x(3))
				}).
			ToList
	End Function

	''' <summary>获取块信息</summary>
	Private Function GetChunk(value As Object) As List(Of CollectionInfo.Chunk)
		Dim data = TryCast(value, Object())
		Return data?.Cast(Of Object()).
			Where(Function(x) x?.Length > 1).
			Select(Function(x)
					   Dim chunk As New CollectionInfo.Chunk

					   For I = 0 To x.Length - 2 Step 2
						   Dim item = x(I + 1)

						   Select Case x(I).ToString
							   Case "startTimestamp"
								   chunk.StartTimestamp = New TimeStamp(Convert.ToInt64(item), False)

							   Case "endTimestamp"
								   chunk.EndTimestamp = New TimeStamp(Convert.ToInt64(item), False)

							   Case "samples"
								   chunk.Samples = Convert.ToInt32(item)

							   Case "size"
								   chunk.Size = Convert.ToInt32(item)

							   Case "bytesPerSample"
								   chunk.BytesPerSample = Convert.ToDouble(item)
						   End Select
					   Next

					   Return chunk
				   End Function).
			ToList
	End Function
End Module
