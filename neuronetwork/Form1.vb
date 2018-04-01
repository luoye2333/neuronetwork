Public Class Form1
	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		Randomize()
		Randarray(Weight_IH)
		Randarray(Weight_HO)
		imagepos = 16
		labelpos = 8
	End Sub
	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		'Readimage(imagepos)
		'Readlabel(labelpos)


	End Sub
	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		OpenFileDialog1.ShowDialog()
		imagefilename = OpenFileDialog1.FileName
		OpenFileDialog1.ShowDialog()
		labelfilename = OpenFileDialog1.FileName
	End Sub
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

	Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
		Dim labelfile As IO.FileStream =
			IO.File.Create("E:\360data\重要数据\桌面\dataset\标准5.txt")
		Dim lre As New IO.BinaryWriter(labelfile)
		lre.BaseStream.Position = 8
		For i = 1 To 100
			lre.Write(84215045)
		Next i
		lre.Flush()
		lre.Close()
		labelfile.Close()
	End Sub

	Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
		C()
	End Sub
	Private Sub C()
		MainC()
		Label1.Text = OOutput(1)
		Label2.Text = OOutput(2)
		Label3.Text = OOutput(3)
		Label4.Text = OOutput(4)
		Label5.Text = OOutput(5)
		Label6.Text = OOutput(6)
		Label7.Text = OOutput(7)
		Label8.Text = OOutput(8)
		Label9.Text = OOutput(9)
		Label10.Text = OOutput(10)

		Label12.Text = answer
		Label13.Text = Mdata()
		Label26.Text = Module1.e

		Label37.Text = IOutput(1)
		Label36.Text = IOutput(2)
		Label35.Text = IOutput(3)
		Label34.Text = IOutput(4)
		Label33.Text = IOutput(5)
		Label32.Text = IOutput(6)
		Label31.Text = IOutput(7)
		Label30.Text = IOutput(8)
		Label29.Text = IOutput(9)
		Label28.Text = IOutput(10)

		Label38.Text = Weight_HO(1, 1)
	End Sub
	Public d As Int16
	Public k(9, 784) As Byte
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
	Public zd(10) As Double
	Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
		Dim ans(9) As Byte

		For i = 0 To 9
			For j = 1 To 784
				Input(j) = k(i, j)
			Next j
			For j = 0 To 10
				Sta(j) = 0
			Next j
			Sta(i + 1) = 1
			answer = i
			C()
			ans(i) = Mdata()

			For j = 1 To 10
				zd(i + 1) += OOutput(j) - Sta(j)
			Next j
			zd(i) = zd(i) / 10

		Next i

		CalD()
		ChWeight()
		CeD()

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

	Private Sub Datafrompng()

		OpenFileDialog2.ShowDialog()
		Dim a As Drawing.Bitmap = Drawing.Image.FromFile(OpenFileDialog2.FileName)
		Dim x, y, q As Integer
		For x = 0 To 27
			For y = 0 To 27
				q += 1
				If a.GetPixel(y, x).R < 128 Then

					Input(q) = 1
				End If
			Next y
		Next x
		Dim p As String = Strings.Left(
		Strings.Right(OpenFileDialog2.FileName, 5), 1)
		For i = 0 To 10
			Sta(i) = 0
		Next i
		Sta(Val(p) + 1) = 1
		answer = Val(p)
	End Sub

	Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
		For i = 1 To Val(TextBox1.Text)
			Button8_Click(Nothing, Nothing)
		Next i
	End Sub
End Class