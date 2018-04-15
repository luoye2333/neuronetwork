Imports System.ComponentModel
Imports System.IO
Imports System.Drawing
Public Class Form1
	Dim Th2, th3, th4, th5, th6 As Threading.Thread
	Delegate Sub FormText()
	Dim showtext1 As New FormText(AddressOf Stext)
	Dim showtext2 As New FormText(AddressOf Stext2)
	Delegate Sub formstatus(i As Integer)
	Dim shownum As New Formstatus(AddressOf Snum)
	Delegate Sub formpicture(i As Drawing.Image)
	Dim showpic As New formpicture(AddressOf Spic)

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		Initializeweight()
	End Sub

	'样本训练-----------------------------------
	'获取路径
	Dim sampleimagename As String
	Dim samplelabelname As String
	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		OpenFileDialog1.ShowDialog()
		OpenFileDialog2.ShowDialog()
		sampleimagename = OpenFileDialog1.FileName
		samplelabelname = OpenFileDialog2.FileName
		imagepos = 16
		labelpos = 8
	End Sub
	'读取
	Dim imagepos As Integer '记录已读取的位置
	Dim labelpos As Integer
	Dim answer As Byte
	Sub Readimage(direction As Integer)
		Dim line As Byte()
		Dim imagefile As FileStream =
			New FileStream(sampleimagename, IO.FileMode.Open)
		Dim lre As New BinaryReader(imagefile)
		lre.BaseStream.Position = direction
		line = lre.ReadBytes(784)

		Dim image As Bitmap = New Bitmap(28, 28)
		Dim x, y, q As Integer
		For x = 0 To 27
			For y = 0 To 27
				q += 1
				'输入转化到0~1上
				Input(q) = （line(x * 28 + y)） / 255
				If Input(q) > 0.5 Then
					image.SetPixel(y, x, Color.Black)
				End If
			Next y
		Next x
		Invoke(showpic, image)

		imagepos += 784
		imagefile.Close()
		lre.Close()
	End Sub
	Sub Readlabel(direction As Integer)
		Dim line As Byte()
		Dim labelfile As FileStream =
			New FileStream(samplelabelname, IO.FileMode.Open)
		Dim lre As New BinaryReader(labelfile)
		lre.BaseStream.Position = direction
		line = lre.ReadBytes(1)
		For i = 0 To 10
			Sta(i) = 0
		Next i
		Sta(line(0) + 1) = 1

		answer = line(0)

		labelpos += 1
		labelfile.Close()
		lre.Close()
	End Sub
	Sub Spic(i As Drawing.Image)
		PictureBox1.Image = i
	End Sub
	Private Sub Sampletraining()
		For i = 1 To 1000
			Readimage(imagepos)
			Readlabel(labelpos)
			CalOut()
			CalD()
			ChWeight()
			CeD()
			Invoke(shownum, i)
			Invoke(showtext2)
		Next i
		th6.Abort()
	End Sub
	Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
		th6 = New Threading.Thread(AddressOf Sampletraining)
		th6.Start()
	End Sub
	Private Sub Stext2()
		Label12.Text = answer
		Label13.Text = Mdata()
		Label26.Text = Module1.e
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


	'判断-----------------------------------------
	'
	'选择判断文件的路径
	Public imagefilename As String
	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		FolderBrowserDialog1.ShowDialog()
		imagefilename = FolderBrowserDialog1.SelectedPath
	End Sub
	'读取图片转为输入
	Private Sub Readimage2(imageindex As Integer)
		Dim image As Drawing.Bitmap =
			Drawing.Image.FromFile(imagefilename & "\TestImage_" & imageindex & ".bmp")
		Dim x, y, q As Integer
		For x = 0 To 27
			For y = 0 To 27
				q += 1
				'输入转化到0~1上
				Input(q) = （255 - image.GetPixel(y, x).R） / 255
			Next y
		Next x
		image.Dispose()
	End Sub
	'判断
	Private Sub Judge()
		For i = 1 To 2744
			Readimage2(i)
			CalOut()
			Dim result As Integer
			result = Mdata()
			If Directory.Exists(imagefilename & "\" & result) Then
			Else
				Directory.CreateDirectory(imagefilename & "\" & result)
			End If
			File.Move(imagefilename & "\TestImage_" & i & ".bmp",
			imagefilename & "\" & result & "\TestImage_" & i & ".bmp")
			Invoke(shownum, i)
		Next i
		th4.Abort()
	End Sub
	Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
		th4 = New Threading.Thread(AddressOf Judge)
		th4.Start()
	End Sub


	'标准训练--------------------------------------
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
	Dim ans(9) As Byte '每个标准的答案
	'一组训练
	Private Sub Looptraining()
		For i = 0 To 9
			'zd(i + 1) = 0
			For j = 1 To 784
				Input(j) = k(i, j)
			Next j
			For j = 0 To 10
				Sta(j) = 0
			Next j
			Sta(i + 1) = 1
			'answer = i
			CalOut()
			ans(i) = Mdata()
			'For j = 1 To 10
			'	zd(i + 1) += OOutput(j) - Sta(j)
			'Next j
			CalD()
			ChWeight()
		Next i

		CeD()
		Invoke(showtext1)
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
	Private Sub Thread3()
		Dim number As Integer = 0
		For i = 1 To Val(TextBox1.Text)
			Looptraining()
			number += 1
			Invoke(shownum, number)
		Next i
		th3.Abort()
	End Sub

	'被委托方法
	Private Sub Snum(i As Integer)
		Label50.Text = i
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