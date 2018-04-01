Public Class Matrix
	Private mx(,) As Double
	Private row As Integer
	Private column As Integer

	Public Sub New(rowlength, columnlength)
		row = rowlength - 1 : column = columnlength - 1
		ReDim mx(row, column)
	End Sub '构造 宽度长度-1
	Public Sub SetAll(datain(,) As Double)
		Dim i, j As Integer
		For i = 0 To row
			For j = 0 To column
				mx(i, j) = datain(i, j)
			Next j
		Next i
	End Sub '输入二维数组
	Public Sub SetOne(number As Double, row As)

	End Sub

	Public Sub GetAll(dataout(,) As Double)
		Dim i, j As Integer
		For i = 0 To row
			For j = 0 To column
				dataout(i, j) = mx(i, j)
			Next j
		Next i
	End Sub '输出二维数组

	Public Function GetTranspose() As Matrix
		Dim outmx As New Matrix(column + 1, row + 1)

	End Function
End Class
