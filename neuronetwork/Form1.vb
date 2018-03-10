Public Class Form1
	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		Randomize()
		Randarray(Weight_IH)
		Randarray(Weight_HO)
		imagepos = 16
		labelpos = 8
	End Sub
	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		'While (e > 0.5) Or (k < maxk)
		'k += 1
		Initialize()
		Readimage(imagepos)
		Readlabel(labelpos)
		CalOut()
		CalD()
		ChWeight()
		CeD()
		'End While
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
				Weight_IH(i, j) = fsave.ReadLine()
			Next j
		Next i
		For i As Integer = 1 To NHide
			For j As Integer = 1 To NOutput
				Weight_HO(i, j) = fsave.ReadLine()
			Next j
		Next i
		fsave.Close()
	End Sub
End Class