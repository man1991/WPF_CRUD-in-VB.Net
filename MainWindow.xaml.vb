Imports System.Data
Imports System.Data.SqlClient
Imports CRUD_Gridvb.Employee


Class MainWindow

    Dim connectionString As String = "Data Source=ADMINRG-TSF729J\SQLEXPRESS;Initial Catalog=TestDB; Integrated Security=True;"
    Dim SqlCon As SqlConnection
    Dim SqlCmd As New SqlCommand
    Dim SqlDa As SqlDataAdapter
    Dim Dt As DataTable
    Dim Query As String
    Dim ID As String

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Load_Grid()
    End Sub

    Public Sub Load_Grid()
        Try
            SqlCon = New SqlConnection(connectionString)
            SqlCmd.Connection = SqlCon
            SqlCmd.CommandText = "EmpMaster_SP"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.AddWithValue("Mode", "GET")
            SqlCon.Open()
            SqlDa = New SqlDataAdapter(SqlCmd)
            Dt = New DataTable("Employee")
            SqlDa.Fill(Dt)
            dgEmp.ItemsSource = Dt.DefaultView
            SqlCmd.Parameters.Clear()
            SqlCon.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        End Try

    End Sub


    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click
        If (txtCode.Text = String.Empty) Then
            MessageBox.Show("Enter the Employee Code")
            Return
        End If

        If (txtName.Text = String.Empty) Then
            MessageBox.Show("Enter the Employee Name")
            Return
        End If

        If (txtDate.Text = String.Empty) Then
            MessageBox.Show("Enter the Employee Name")
            Return
        End If
        Dim EmpAddress As String
        EmpAddress = New TextRange(rtxtAddress.Document.ContentStart, rtxtAddress.Document.ContentEnd).Text.ToString()
        If (EmpAddress = String.Empty) Then
            MessageBox.Show("Enter the Employee Name")
            Return
        End If

        Try
            Dim Emp As New Employee
            Emp.EmployeeCode = Convert.ToInt32(txtCode.Text)
            Emp.EmployeeName = UCase(txtName.Text.Trim())
            Emp.DOB = Convert.ToDateTime(txtDate.Text)
            Emp.Address = EmpAddress
            SqlCon = New SqlConnection(connectionString)
            SqlCmd.Connection = SqlCon
            SqlCmd.CommandText = "EmpMaster_SP"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.AddWithValue("Mode", "ADD")
            SqlCmd.Parameters.AddWithValue("EmpCode", Emp.EmployeeCode)
            SqlCmd.Parameters.AddWithValue("EmpName", Emp.EmployeeName)
            SqlCmd.Parameters.AddWithValue("DOB", Emp.DOB)
            SqlCmd.Parameters.AddWithValue("Address", Emp.Address)
            SqlCon.Open()
            SqlCmd.ExecuteNonQuery()
            SqlCmd.Parameters.Clear()
            SqlCon.Close()
            Load_Grid()
            MessageBox.Show("Added Successfully")

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        End Try

    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs) Handles btnUpdate.Click
        If (txtCode.Text = String.Empty) Then
            MessageBox.Show("Enter the Employee Code")
            Return
        End If

        If (txtName.Text = String.Empty) Then
            MessageBox.Show("Enter the Employee Name")
            Return
        End If

        If (txtDate.Text = String.Empty) Then
            MessageBox.Show("Enter the Date of Birth")
            Return
        End If

        Dim EmpAddress As String
        EmpAddress = New TextRange(rtxtAddress.Document.ContentStart, rtxtAddress.Document.ContentEnd).Text.ToString()
        If (EmpAddress = String.Empty) Then
            MessageBox.Show("Enter the Employee Address")
            Return
        End If

        Try
            Dim Emp As New Employee
            Emp.EmployeeCode = Convert.ToInt32(txtCode.Text)
            Emp.EmployeeName = UCase(txtName.Text.Trim())
            Emp.DOB = Convert.ToDateTime(txtDate.Text)
            Emp.Address = EmpAddress
            SqlCon = New SqlConnection(connectionString)
            SqlCmd.Connection = SqlCon
            SqlCmd.CommandText = "EmpMaster_SP"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.AddWithValue("Mode", "EDIT")
            SqlCmd.Parameters.AddWithValue("EmpCode", Emp.EmployeeCode)
            SqlCmd.Parameters.AddWithValue("EmpName", Emp.EmployeeName)
            SqlCmd.Parameters.AddWithValue("DOB", Emp.DOB)
            SqlCmd.Parameters.AddWithValue("Address", Emp.Address)
            SqlCmd.Parameters.AddWithValue("ID", Convert.ToInt32(lblEmpId.Content))
            SqlCon.Open()
            SqlCmd.ExecuteNonQuery()
            SqlCmd.Parameters.Clear()
            SqlCon.Close()
            Load_Grid()
            MessageBox.Show("Updated Successfully")

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click

        Try
            Dim Emp As New Employee
            Emp.EmployeeCode = Convert.ToInt32(txtCode.Text)
            Emp.EmployeeName = UCase(txtName.Text.Trim())
            Emp.DOB = Convert.ToDateTime(txtDate.Text)
            SqlCon = New SqlConnection(connectionString)
            SqlCmd.Connection = SqlCon
            SqlCmd.CommandText = "EmpMaster_SP"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.AddWithValue("Mode", "DELETE")
            SqlCmd.Parameters.AddWithValue("ID", Convert.ToInt32(lblEmpId.Content))
            SqlCon.Open()
            SqlCmd.ExecuteNonQuery()
            SqlCmd.Parameters.Clear()
            SqlCon.Close()
            Load_Grid()
            MessageBox.Show("Deleted Successfully")

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        End Try

    End Sub


    Private Sub dgEmp_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles dgEmp.MouseDoubleClick
        Try
            SqlCon = New SqlConnection(connectionString)
            Dim Drv As DataRowView = DirectCast(dgEmp.SelectedItem, DataRowView)
            Dim Fd As New FlowDocument
            Dim Pg As New Paragraph

            SqlCmd.Connection = SqlCon
            SqlCmd.CommandText = "EmpMaster_SP"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.AddWithValue("Mode", "GETID")
            SqlCmd.Parameters.AddWithValue("ID", Convert.ToInt32(Drv("ID")))
            SqlCon.Open()


            Dim sqlReader As SqlDataReader = SqlCmd.ExecuteReader()
            If sqlReader.HasRows Then
                While (sqlReader.Read())
                    lblEmpId.Content = sqlReader.GetValue(0).ToString()
                    txtCode.Text = sqlReader.GetValue(1)
                    txtName.Text = sqlReader.GetString(2)
                    txtDate.Text = sqlReader.GetDateTime(3)
                    Pg.Inlines.Add(New Run(sqlReader.GetString(4).ToString()))
                    Fd.Blocks.Add(Pg)
                    rtxtAddress.Document = Fd

                End While

            End If

            SqlCmd.Parameters.Clear()
            SqlCon.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        End Try
    End Sub
End Class
