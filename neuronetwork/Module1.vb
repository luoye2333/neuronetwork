Public Module Module1
	Public Const StRate = 0.3
	Public Const Dif = 0.01
	Public Const maxk = 10
	Public Const NInput = 28 * 28
	Public Const NHide = 30
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

	Sub Randarray(ByRef a(,) As Double)
		Dim l1 As Integer = UBound(a, 1)
		Dim l2 As Integer = UBound(a, 2)
		For i As Integer = 1 To l1
			For j As Integer = 1 To l2
				a(i, j) = Rnd() * 2 - 1
			Next j
		Next i
	End Sub
	Sub Initialize()
		e = 0
		Sta.Initialize()
		IHide.Initialize()
		IOutput.Initialize()
		DOutput.Initialize()
		DHide.Initialize()
	End Sub
	Sub Readimage(direction As Integer)
		Dim imagefile As IO.FileStream =
			New IO.FileStream(imagefilename, IO.FileMode.Open)
		Dim ire As New IO.BinaryReader(imagefile)
		Dim line As Byte()
		ire.BaseStream.Position = direction
		line = ire.ReadBytes(784)
		For i As Integer = 1 To 784
			If line(i - 1) < 200 Then
				Input(i) = 1
			Else
				Input(i) = 0
			End If '规格化 归一化
		Next i
		imagepos += 784
	End Sub
	Sub Readlabel(direction As Integer)
		Dim line As Byte()
		Dim labelfile As IO.FileStream =
			New IO.FileStream(labelfilename, IO.FileMode.Open)
		Dim lre As New IO.BinaryReader(labelfile)
		lre.BaseStream.Position = direction
		line = lre.ReadBytes(1)
		Sta(line(0) + 1) = 1
		answer = line(0)
		labelpos += 1
	End Sub
	Function FuncSig(x As Double) As Double
		Return 1 / (1 + Math.Exp(-x))
	End Function
	Sub CalOut()
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NInput
				IHide(i) += Input(j) * Weight_IH(j, i)
			Next j
			OHide(i) = FuncSig(IHide(i))
		Next i

		For i As Integer = 1 To NOutput
			For j As Integer = 1 To NHide
				IOutput(i) += OHide(j) * Weight_HO(j, i)
			Next j
			OOutput(i) = FuncSig(IOutput(i))
		Next i
	End Sub
	Sub CalD()
		For i As Integer = 1 To NOutput
			DOutput(i) = OOutput(i) - Sta(i)
		Next i

		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				DHide(i) += DOutput(j) * Weight_HO(i, j)
			Next j
		Next i
	End Sub
	Function Dfunc(x As Double) As Double
		Dim y0, y1 As Double
		y0 = FuncSig(x)
		y1 = FuncSig(x + Dif)
		Return (y1 - y0) / Dif
	End Function
	Function DfuncSig(x) As Double
		Return Math.Exp(-x) / ((1 + Math.Exp(-x)) ^ 2)
	End Function
	Sub ChWeight()
		For i As Integer = 1 To NInput
			For j As Integer = 1 To NHide
				Weight_IH(i, j) += StRate * DHide(j) * DfuncSig(IHide(j)) * Input(i)
			Next j
		Next i
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				Weight_HO(i, j) += StRate * DOutput(j) * DfuncSig(IOutput(j)) * OHide(i)
			Next j
		Next i
	End Sub
	Sub CeD()
		For i As Integer = 1 To NOutput
			e += 0.5 * ((OOutput(i) - Sta(i)) ^ 2)
		Next i
	End Sub
End Module
