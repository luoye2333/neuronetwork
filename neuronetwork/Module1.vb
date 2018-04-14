Public Module Module1
	Public Const maxk = 10
	Public Const NInput = 28 * 28
	Public Const NHide = 25
	Public Const NOutput = 10

	Public zd(10) As Double '每个标准的误差

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

	'所有过程
	Sub MainC()
		CalOut()
		CalD()
		ChWeight()
		CeD()
	End Sub
	Sub Randarray(ByRef a(,) As Double)
		Dim l1 As Integer = UBound(a, 1)
		Dim l2 As Integer = UBound(a, 2)
		For i As Integer = 1 To l1
			For j As Integer = 1 To l2
				a(i, j) = Rnd() * 2 - 1
			Next j
		Next i
	End Sub '初始化权值

	Function FuncS(x As Double, k As Double) As Double
		Dim t As Double = x / k
		Return (2 / (1 + Math.Exp(-t))) - 1
		' f(x)=[2 / 1+e^-kx]-1
	End Function '非线性函数,k为输入量
	Function Dfunc(x As Double, k As Double) As Double
		Return k / 2 * (FuncS(x, k) + 1) * (1 - FuncS(x, k))
		' f'(x)=2*k*e^-kx/(1+e^-kx)^2
		'      =k/2*[[f(x)+1]*[2-(f(x)+1)]
	End Function
	Sub CalOut()

		For i As Integer = 1 To NHide
			IHide(i) = 0
			For j As Integer = 1 To NInput
				IHide(i) += Input(j) * Weight_IH(j, i)
			Next j
			OHide(i) = FuncS(IHide(i), 100)
		Next i

		For i As Integer = 1 To NOutput
			IOutput(i) = 0
			For j As Integer = 1 To NHide
				IOutput(i) += OHide(j) * Weight_HO(j, i)
			Next j
			OOutput(i) = FuncS(IOutput(i), 10)
		Next i
	End Sub '神经元非线性输出
	Sub CalD()
		Dim dd As Double = 0
		'For i = 1 To 10
		'	dd += (zd(i) / 10) '十组数据的平均误差
		'Next i
		For i As Integer = 1 To NOutput
			DOutput(i) = OOutput(i) - Sta(i) ' dd
		Next i
		For i As Integer = 1 To NHide
			DHide(i) = 0
			For j As Integer = 1 To NOutput
				DHide(i) += DOutput(j) * Weight_HO(i, j)
			Next j
		Next i
	End Sub '神经元误差
	Function Findstrate(x As Double) As Double
		If x < 10 ^ -2 Then
			Return 0.1
		ElseIf x < 10 ^ -1 Then
			Return 10 ^ -2
		Else
			Return 10 ^ -2 / x
		End If
	End Function '根据输出确定学习速率
	Sub ChWeight()

		For i As Integer = 1 To NInput
			For j As Integer = 1 To NHide
				Weight_IH(i, j) -= Findstrate(IHide(j)) * DHide(j) *
					Dfunc(IHide(j), 784) * Input(i)
			Next j
		Next i
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				Weight_HO(i, j) -= Findstrate(IOutput(j)) * DOutput(j) *
					Dfunc(IOutput(j), 25) * OHide(i)
			Next j
		Next i
	End Sub '改变权值
	Sub CeD()
		e = 0
		For i As Integer = 1 To NOutput
			e += 0.5 * ((OOutput(i) - Sta(i)) ^ 2)
		Next i
	End Sub '整体误差

End Module
