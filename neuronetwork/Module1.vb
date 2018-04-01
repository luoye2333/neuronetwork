Public Module Module1
	Public Const maxk = 10
	Public Const NInput = 28 * 28
	Public Const NHide = 25
	Public Const NOutput = 10

	Public k As Integer '样本数
	Public Input(NInput) As Byte
	Public IHide(NHide) As Double
	Public IOutput(NOutput) As Double
	Public OHide(NHide) As Double
	Public OOutput(NOutput) As Double
	Public Weight_IH(NInput, NHide) As Double
	Public Weight_HO(NHide, NOutput) As Double
	Public DOutput(NOutput) As Double
	Public DHide(NHide) As Double
	Public Sta(NOutput) As Byte
	Public e As Double '全局误差
	Public imagefilename As String
	Public labelfilename As String
	Public imagepos As Integer
	Public labelpos As Integer
	Public answer As Byte

	'Public standard(10, 784) As Byte

	Sub MainC()
		'While (e > 0.5) Or (k < maxk)
		'k += 1

		CalOut()
		CalD()

		'End While
	End Sub
	Sub Randarray(ByRef a(,) As Double)
		Dim l1 As Integer = UBound(a, 1)
		Dim l2 As Integer = UBound(a, 2)
		For i As Integer = 1 To l1
			For j As Integer = 1 To l2
				a(i, j) = Rnd() * 2 - 1
			Next j
		Next i
	End Sub

	Sub Readimage(direction As Integer)
		Dim imagefile As IO.FileStream =
			New IO.FileStream(imagefilename, IO.FileMode.Open)
		Dim ire As New IO.BinaryReader(imagefile)
		Dim line As Byte()
		ire.BaseStream.Position = direction
		line = ire.ReadBytes(784)
		For i As Integer = 1 To 784
			If line(i - 1) > 100 Then
				Input(i) = 1
			Else
				Input(i) = 0
			End If '规格化 归一化
		Next i
		imagepos += 784
		imagefile.Close()
		ire.Close()
	End Sub '读取样本
	Sub Readlabel(direction As Integer)
		Dim line As Byte()
		Dim labelfile As IO.FileStream =
			New IO.FileStream(labelfilename, IO.FileMode.Open)
		Dim lre As New IO.BinaryReader(labelfile)
		lre.BaseStream.Position = direction
		line = lre.ReadBytes(1)
		Sta.Initialize()
		Sta(line(0) + 1) = 1
		answer = line(0)
		labelpos += 1
		labelfile.Close()
		lre.Close()
	End Sub '读取答案

	Function FuncS(x As Double, k As Double) As Double
		Dim t As Double = x / k
		Return (Math.Pow(t, 3) + 0.2 * t) / 1.2
	End Function '非线性函数
	Function Dfunc(x As Double, k As Double) As Double
		Return (3 * Math.Pow(x, 2) + 0.2) / k / 1.2
	End Function
	Sub CalOut()

		For i As Integer = 1 To NHide
			IHide(i) = 0
			For j As Integer = 1 To NInput
				IHide(i) += Input(j) * Weight_IH(j, i)
			Next j
			OHide(i) = FuncS(IHide(i), 784)
		Next i

		For i As Integer = 1 To NOutput
			IOutput(i) = 0
			For j As Integer = 1 To NHide
				IOutput(i) += OHide(j) * Weight_HO(j, i)
			Next j
			OOutput(i) = FuncS(IOutput(i), 25)
		Next i
	End Sub '非线性的输出
	Sub CalD()
		For i As Integer = 1 To NOutput
			DOutput(i) = Form1.zd(i)
		Next i
		For i As Integer = 1 To NHide
			DHide(i) = 0
			For j As Integer = 1 To NOutput
				DHide(i) += DOutput(j) * Weight_HO(i, j)
			Next j
		Next i
	End Sub '计算误差

	Sub ChWeight()
		Dim strate As Double

		If IOutput(1) < 10 ^ -2 Then
			strate = 0.01
		ElseIf IOutput(1) < 10 ^ -1 Then
			strate = 10 ^ -3
		Else
			strate = 10 ^ -3 / IOutput(1)


		End If


		For i As Integer = 1 To NInput
			For j As Integer = 1 To NHide
				Weight_IH(i, j) -= StRate * DHide(j) *
					Dfunc(IHide(j), 784) * Input(i)
			Next j
		Next i
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				Weight_HO(i, j) -= StRate * DOutput(j) *
					Dfunc(IOutput(j), 25) * OHide(i)
			Next j
		Next i
	End Sub '改变权值
	Sub CeD()
		e = 0
		For i As Integer = 1 To NOutput
			e += 0.5 * ((OOutput(i) - Sta(i)) ^ 2)
		Next i
	End Sub '总误差

End Module
