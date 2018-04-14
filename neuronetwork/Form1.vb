Imports System.ComponentModel
Imports System.IO
Public Class Form1
	Dim Th2, th3, th4 As Threading.Thread
	Delegate Sub Formstatus()
	Dim shownum As New Formstatus(AddressOf Snum)
	Dim showtext As New Formstatus(AddressOf Stext)

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		Randomize()
		Randarray(Weight_IH)
		Randarray(Weight_HO)
		imagepos = 16
		labelpos = 8
	End Sub


	'选择样本路径
	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		FolderBrowserDialog1.ShowDialog()
		imagefilename = FolderBrowserDialog1.SelectedPath
	End Sub

	Private Sub Readimage(imageindex As Integer)
		Dim image As Drawing.Bitmap =
			Drawing.Image.FromFile(imagefilename & "\TestImage_" & imageindex & ".bmp")
		Dim x, y, q As Integer
		For x = 0 To 27
			For y = 0 To 27
				q += 1
				If image.GetPixel(y, x).R < 128 Then
					Input(q) = 1
				End If
			Next y
		Next x

	End Sub '读取样本
	Private Sub Judge()
		CalOut()
		th4.Abort()
	End Sub
	Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

		For i = 1 To 2744
			Readimage(i)
			th4 = New Threading.Thread(AddressOf Judge)
			th4.Start（）****
				'下一步修改 循环移至线程内 操作文件使用委托 
			Dim result As Integer
			result = Mdata()
			If Directory.Exists(imagefilename & "\" & result) Then
			Else
				Directory.CreateDirectory(imagefilename & "\" & result)
			End If
			File.Move(imagefilename & "\TestImage_" & i & ".bmp",
				imagefilename & "\" & result & "\TestImage_" & i & ".bmp")
			Label27.Text = i
		Next i
	End Sub


	'确定结果：最大值
	Private Function Mdata() As Integer
		Dim a As Double
		Dim b As Integer
		For i As Integer = 1 To 10
			If OOutput(i) > a Then
				a = OOutput(i)
				b = i
			End If
		Next i
		Return b - 1
	End Function

	'保存网络权值
	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		Dim savefile As String = "E:\360data\重要数据\桌面\neuronetwork\weight.txt"
		Dim fsave As IO.StreamWriter = New IO.StreamWriter(savefile)
		For i As Integer = 1 To NInput
			For j As Integer = 1 To NHide
				fsave.WriteLine(Weight_IH(i, j))
			Next j
		Next i
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				fsave.WriteLine(Weight_HO(i, j))
			Next j
		Next i
		fsave.Close()
	End Sub
	'读入网络权值
	Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
		Dim savefile As String = "E:\360data\重要数据\桌面\neuronetwork\weight.txt"
		Dim fsave As IO.StreamReader = New IO.StreamReader(savefile)
		For i As Integer = 1 To NInput
			For j As Integer = 1 To NHide
				Weight_IH(i, j) = Val(fsave.ReadLine())
			Next j
		Next i
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				Weight_HO(i, j) = Val(fsave.ReadLine())
			Next j
		Next i
		fsave.Close()
	End Sub
	Public d As Int16
	Public k(9, 784) As Byte
	'载入标准训练数据
	Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
		Dim savefile As String
		Dim fsave As IO.StreamReader
		For i = 0 To 9
			savefile = "E:\360data\重要数据\桌面\neuronetwork\" & i & ".txt"
			fsave = New IO.StreamReader(savefile)
			For j As Integer = 1 To NInput
				k(i, j) = Val(fsave.ReadLine())
			Next j
			fsave.Close()
		Next i
	End Sub

	'Public zd(10) As Double 
	Dim ans(9) As Byte '每个标准的训练答案
	'一组训练
	Private Sub Looptraining()
		For i = 0 To 9
			zd(i + 1) = 0
			For j = 1 To 784
				Input(j) = k(i, j)
			Next j
			For j = 0 To 10
				Sta(j) = 0
			Next j
			Sta(i + 1) = 1
			answer = i
			CalOut()
			ans(i) = Mdata()
			'For j = 1 To 10
			'	zd(i + 1) += OOutput(j) - Sta(j)
			'Next j
			CalD()
			ChWeight()
		Next i

		CeD()
		Invoke(showtext)
	End Sub
	Private Sub Stext()
		Label26.Text = Module1.e
		Label48.Text = ans(0)
		Label47.Text = ans(1)
		Label46.Text = ans(2)
		Label45.Text = ans(3)
		Label44.Text = ans(4)
		Label43.Text = ans(5)
		Label42.Text = ans(6)
		Label41.Text = ans(7)
		Label40.Text = ans(8)
		Label39.Text = ans(9)
	End Sub

	'单次训练
	Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
		Th2 = New Threading.Thread(AddressOf Thread2)
		Th2.Start()
	End Sub
	Private Sub Thread2()
		Looptraining()
		Th2.Abort()
	End Sub
	'多次训练
	Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
		th3 = New Threading.Thread(AddressOf Thread3)
		th3.Start()
	End Sub
	Dim number As Integer
	Private Sub Thread3()
		number = 0
		For i = 1 To Val(TextBox1.Text)
			Looptraining()
			number += 1
			Invoke(shownum)
		Next i
		th3.Abort()
	End Sub
	Private Sub Snum()
		Label50.Text = Convert.ToString(number)
	End Sub



	'Private Sub Datafrompng()

	'	OpenFileDialog2.ShowDialog()
	'	Dim a As Drawing.Bitmap = Drawing.Image.FromFile(OpenFileDialog2.FileName)
	'	Dim x, y, q As Integer
	'	For x = 0 To 27
	'		For y = 0 To 27
	'			q += 1
	'			If a.GetPixel(y, x).R < 128 Then

	'				Input(q) = 1
	'			End If
	'		Next y
	'	Next x
	'	Dim p As String = Strings.Left(
	'	Strings.Right(OpenFileDialog2.FileName, 5), 1)
	'	For i = 0 To 10
	'		Sta(i) = 0
	'	Next i
	'	Sta(Val(p) + 1) = 1
	'	answer = Val(p)
	'End Sub
	'Sub Readlabel(direction As Integer)
	'	Dim line As Byte()
	'	Dim labelfile As IO.FileStream =
	'		New IO.FileStream(labelfilename, IO.FileMode.Open)
	'	Dim lre As New IO.BinaryReader(labelfile)
	'	lre.BaseStream.Position = direction
	'	line = lre.ReadBytes(1)
	'	Sta.Initialize()
	'	Sta(line(0) + 1) = 1
	'	answer = line(0)
	'	labelpos += 1
	'	labelfile.Close()
	'	lre.Close()
	'End Sub '读取答案
	'Private Sub C()


	'	'Label1.Text = OOutput(1)
	'	'Label2.Text = OOutput(2)
	'	'Label3.Text = OOutput(3)
	'	'Label4.Text = OOutput(4)
	'	'Label5.Text = OOutput(5)
	'	'Label6.Text = OOutput(6)
	'	'Label7.Text = OOutput(7)
	'	'Label8.Text = OOutput(8)
	'	'Label9.Text = OOutput(9)
	'	'Label10.Text = OOutput(10)

	'	'Label12.Text = answer
	'	'Label13.Text = Mdata()


	'	'Label37.Text = IOutput(1)
	'	'Label36.Text = IOutput(2)
	'	'Label35.Text = IOutput(3)
	'	'Label34.Text = IOutput(4)
	'	'Label33.Text = IOutput(5)
	'	'Label32.Text = IOutput(6)
	'	'Label31.Text = IOutput(7)
	'	'Label30.Text = IOutput(8)
	'	'Label29.Text = IOutput(9)
	'	'Label28.Text = IOutput(10)

	'	'Label38.Text = Weight_HO(1, 1)
	'End Sub
End Class