'本类我是用VB.NET编写的类， 不管用什么语言， 算法都类似， 注释写得都挺详尽的了， 而且我也通过了调试。

Public Class Matrix1

	Private matrix(,) As Double '定义矩阵变量
	Private rowbound As Integer '定义数组行数，从0开始
	Private colbound As Integer '定义数组列数，从0开始

	Public Sub New(ByVal data(,) As Double)
		'首先获得矩阵大小
		rowbound = data.GetUpperBound(0)
		colbound = data.GetUpperBound(1)
		ReDim matrix(rowbound, colbound)

		Dim i As Integer, j As Integer '数组赋值循环变量
		For i = 0 To rowbound '赋值到矩阵变量
			For j = 0 To colbound
				matrix(i, j) = data(i, j)
			Next j
		Next i

	End Sub '构造函数
	'构造函数
	Public Sub LetMatrix(ByRef output_(,) As Double)
		Dim i As Integer, j As Integer
		For i = 0 To rowbound
			For j = 0 To colbound
				output_(i, j) = matrix(i, j)
			Next j
		Next i
	End Sub  '把类中的矩阵输出到一个二维数组中
	'把类中的矩阵输出到一个二维数组中
	Public Sub SetMatrix(ByVal input_(,) As Double)
		ReDim matrix(input_.GetUpperBound(0), input_.GetUpperBound(1))

		Dim i As Integer, j As Integer '数组赋值循环变量
		For i = 0 To input_.GetUpperBound(0) '赋值到矩阵变量
			For j = 0 To input_.GetUpperBound(1)
				matrix(i, j) = input_(i, j)
			Next j
		Next i
	End Sub '把一个二维数组中数据输出到类中
	'把一个二维数组中数据输出到类中
	Public Function GetTranspose() As Matrix1
		Dim output_(,) As Double
		ReDim output_(rowbound, colbound)
		LetMatrix(output_)

		Dim i As Integer, j As Integer, t As Double
		For i = 0 To rowbound
			For j = i To colbound
				t = output_(i, j)
				output_(i, j) = output_(j, i)
				output_(j, i) = t
			Next
		Next

		Return New Matrix1(output_)
	End Function '得到原矩阵的转置
	'得到原矩阵的转置
	Public Function GetValue() As Double '求行列式的值,矩阵必须是方阵
		Dim value As Double '用于保存行列式的值
		value = 1

		'求值基本思路：通过高斯消去法进行矩阵变换，形成三角行列式
		Dim Ri As Integer, Rj As Integer '用于行列之间循环,Ri为行
		Dim t As Integer '临时变量t用于循环
		Ri = 0
		Rj = 0

		Dim minus As Integer '记录矩阵变换后的正负号
		minus = 1

		Dim k As Double '为了将行列式某些值变成0，必须配个比例系数k，使得 R(a)-k*R(b)=0

		Dim matrix_(,) As Double '定义临时矩阵
		ReDim matrix_(rowbound, colbound)
		For Ri = 0 To rowbound '为了防止原始矩阵被修改，赋值到临时矩阵
			For Rj = 0 To colbound
				matrix_(Ri, Rj) = matrix(Ri, Rj)
			Next Rj
		Next Ri


		For Rj = 0 To rowbound '从最左行开始化上三角
			For Ri = rowbound To Rj + 1 Step -1 '从最下边开始化上三角
				If matrix_(Ri - 1, Rj) = 0 Then
					SwapRow(matrix_, Ri - 1, Ri) '如果上一行的对应数字为0，则交换这两行行列式
					minus = minus * -1
				End If
				If matrix_(Ri - 1, Rj) <> 0 Then k = -1 * matrix_(Ri, Rj) / matrix_(Ri - 1, Rj) '比例系数则等于下行数字除以上行
				For t = Rj To rowbound
					matrix_(Ri, t) = matrix_(Ri, t) + k * matrix_(Ri - 1, t) '高斯变换原理
				Next t
			Next Ri
		Next Rj

		For t = 0 To rowbound '计算上三角行列式的值
			value = value * matrix_(t, t)
		Next
		value = value * minus
		'If value=#IND then value=0
		Return value
	End Function '输出行列式的值
	'输出行列式的值
	Public Function GetInverse() As Matrix1  '求逆矩阵
		'定义类和数据，进行返回
		Dim result As Matrix1
		Dim data(,) As Double
		ReDim data(rowbound, colbound)

		Dim matrix_(,) As Double '定义临时矩阵
		Dim tmatrix As Matrix1 '定义临时矩阵类
		Dim i As Integer, j As Integer '临时变量用于循环
		ReDim matrix_(rowbound, colbound)
		For i = 0 To rowbound '为了防止原始矩阵被修改，赋值到临时矩阵
			For j = 0 To colbound
				matrix_(i, j) = matrix(i, j)
			Next j
		Next i

		If GetValue() = 0 Then
			Debug.Print("该矩阵没有逆矩阵，因为行列式的值为0")
			Return Nothing
		ElseIf rowbound <> colbound Then
			Debug.Print("该矩阵没有逆矩阵，因为不是方阵")
			Return Nothing
		End If


		'接下来获得伴随矩阵和逆矩阵
		For i = 0 To rowbound
			For j = 0 To colbound
				tmatrix = New Matrix1(matrix) '实例化临时矩阵
				tmatrix = tmatrix.DelRow(tmatrix.matrix, i)
				tmatrix = tmatrix.DelCol(tmatrix.matrix, j) '先删除某一行，再删除某一列，得到伴随矩阵子块
				matrix_(j, i) = ((tmatrix.GetValue) * (-1) ^ (i + j + 2)) / GetValue()
				'去掉某行某列以得到伴随矩阵子块，并求出其值，除以原矩阵的行列式则得到逆矩阵
			Next j
		Next i

		result = New Matrix1(matrix_)

		Return result
	End Function '输出逆矩阵
	'输出逆矩阵
	Public Shared Operator *(ByVal m1 As Matrix1, ByVal m2 As Matrix1) As Matrix1
		Dim md1(,) As Double, md2(,) As Double  '存储m1,m2矩阵数据
		Dim result(,) As Double '存储运算结果
		Dim t As Double '存储每一个值的临时结果
		Dim i As Integer, j As Integer, k As Integer
		'注意：仅当左矩阵列数等于右矩阵行数时才可以进行乘法

		If m1.colbound <> m2.rowbound Then
			Debug.Print("无法进行矩阵乘法运算，矩阵一的列数不等于矩阵二的行数。")
			Return Nothing
		End If

		ReDim md1(m1.rowbound, m1.colbound)
		ReDim md2(m2.rowbound, m2.colbound)
		ReDim result(m1.rowbound, m2.colbound)
		m1.LetMatrix(md1)
		m2.LetMatrix(md2)
		For i = 0 To m2.colbound
			For j = 0 To m1.rowbound
				t = 0
				For k = 0 To m1.colbound
					t = t + md1(j, k) * md2(k, i)
				Next
				result(j, i) = t
			Next j
		Next i
		Return New Matrix1(result)
	End Operator '重载*运算符，定义矩阵间的乘法运算，计算m1 * m2
	'重载*运算符，定义矩阵间的乘法运算，计算m1 * m2
	Public Shared Operator +(ByVal m1 As Matrix1, ByVal m2 As Matrix1) As Matrix1
		Dim md1(,) As Double, md2(,) As Double  '存储m1,m2矩阵数据
		Dim result(,) As Double '存储运算结果
		Dim i As Integer, j As Integer
		'注意：只有两个矩阵的长宽一样才可以进行运算

		If m1.rowbound <> m2.rowbound Or m1.colbound <> m2.colbound Then
			Debug.Print("无法进行矩阵加法运算，长宽不一致。")
			Return Nothing
		End If

		ReDim result(m1.rowbound, m1.colbound)
		ReDim md1(m1.rowbound, m1.colbound)
		ReDim md2(m2.rowbound, m2.colbound)
		m1.LetMatrix(md1)
		m2.LetMatrix(md2)

		For i = 0 To m1.rowbound
			For j = 0 To m1.colbound
				result(i, j) = md1(i, j) + md2(i, j)
			Next j
		Next i
		Return New Matrix1(result)
	End Operator '重载+运算符，定义矩阵间的加法运算，计算m1 + m2
	'重载+运算符，定义矩阵间的加法运算，计算m1 + m2
	Public Shared Operator -(ByVal m1 As Matrix1, ByVal m2 As Matrix1) As Matrix1
		Dim md1(,) As Double, md2(,) As Double  '存储m1,m2矩阵数据
		Dim result(,) As Double '存储运算结果
		Dim i As Integer, j As Integer
		'注意：只有两个矩阵的长宽一样才可以进行运算

		If m1.rowbound <> m2.rowbound Or m1.colbound <> m2.colbound Then
			Debug.Print("无法进行矩阵减法运算，长宽不一致。")
			Return Nothing
		End If

		ReDim result(m1.rowbound, m1.colbound)
		ReDim md1(m1.rowbound, m1.colbound)
		ReDim md2(m2.rowbound, m2.colbound)
		m1.LetMatrix(md1)
		m2.LetMatrix(md2)

		For i = 0 To m1.rowbound
			For j = 0 To m1.colbound
				result(i, j) = md1(i, j) - md2(i, j)
			Next j
		Next i
		Return New Matrix1(result)
	End Operator '重载-运算符，定义矩阵间的减法运算，计算m1 - m2
	'重载-运算符，定义矩阵间的减法运算，计算m1 - m2
	Private Function DelRow(ByVal matrix_(,) As Double, ByVal r As Integer) As Matrix1  '删除某行
		Dim i As Integer, j As Integer
		For i = r To matrix_.GetUpperBound(0) - 1
			For j = 0 To matrix_.GetUpperBound(1)
				matrix_(i, j) = matrix_(i + 1, j) '把下面的值赋给上面的值
			Next j
		Next i

		Dim result(,) As Double
		ReDim result(matrix_.GetUpperBound(0) - 1, matrix_.GetUpperBound(1)) '重新修改边界
		For i = 0 To matrix_.GetUpperBound(0) - 1
			For j = 0 To matrix_.GetUpperBound(1)
				result(i, j) = matrix(i, j)
			Next j
		Next i

		Return New Matrix1(result)

	End Function '删除某一行
	'删除某一行
	Private Function DelCol(ByVal matrix_(,) As Double, ByVal c As Integer) As Matrix1  '删除某列
		Dim i As Integer, j As Integer
		For i = 0 To matrix_.GetUpperBound(0)
			For j = c To matrix_.GetUpperBound(1) - 1
				matrix_(i, j) = matrix_(i, j + 1) '把右边的值赋给左边的值
			Next j
		Next i

		Dim result(,) As Double
		ReDim result(matrix_.GetUpperBound(0), matrix_.GetUpperBound(1) - 1) '重新修改边界
		For i = 0 To matrix_.GetUpperBound(0)
			For j = 0 To matrix_.GetUpperBound(1) - 1
				result(i, j) = matrix(i, j)
			Next j
		Next i

		Return New Matrix1(result)

	End Function '删除某一列
	'删除某一列
	Private Sub SwapRow(ByRef matrix_(,) As Double, ByVal i As Integer, ByVal j As Integer) '交换矩阵两列
		Dim t As Integer '临时变量t，用于循环
		Dim temp As Double 'temp变量用于交换
		For t = 0 To rowbound
			temp = matrix_(i, t)
			matrix_(i, t) = matrix_(j, t)
			matrix_(j, t) = temp
		Next
	End Sub '交换某两行
	'交换某两行

End Class '矩阵类，进行简单矩阵计算