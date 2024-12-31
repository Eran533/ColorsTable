Imports System.Web.Services
Imports Npgsql
Imports System.Configuration

Public Class ColorTable
    Inherits System.Web.UI.Page

    Private Shared ReadOnly connString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
    <WebMethod>
    Public Shared Function GetColors() As List(Of ColorItem)
        Dim colors As New List(Of ColorItem)

        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand("SELECT * FROM Colors ORDER BY DisplayOrder", conn)
                conn.Open()
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        colors.Add(New ColorItem With {
                            .Id = Convert.ToInt32(reader("Id")),
                            .ColorName = reader("ColorName").ToString(),
                            .Price = Convert.ToDecimal(reader("Price")),
                            .DisplayOrder = Convert.ToInt32(reader("DisplayOrder")),
                            .InStock = Convert.ToBoolean(reader("InStock"))
                        })
                    End While
                End Using
            End Using
        End Using

        Return colors
    End Function

    <WebMethod>
    Public Shared Sub AddColor(color As ColorItem)
        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand("INSERT INTO Colors (ColorName, Price, DisplayOrder, InStock) VALUES (@ColorName, @Price, @DisplayOrder, @InStock)", conn)
                cmd.Parameters.AddWithValue("@ColorName", color.ColorName)
                cmd.Parameters.AddWithValue("@Price", color.Price)
                cmd.Parameters.AddWithValue("@DisplayOrder", color.DisplayOrder)
                cmd.Parameters.AddWithValue("@InStock", color.InStock)

                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    <WebMethod>
    Public Shared Sub UpdateColor(color As ColorItem)
        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand("UPDATE Colors SET ColorName = @ColorName, Price = @Price, DisplayOrder = @DisplayOrder, InStock = @InStock WHERE Id = @Id", conn)
                cmd.Parameters.AddWithValue("@Id", color.Id)
                cmd.Parameters.AddWithValue("@ColorName", color.ColorName)
                cmd.Parameters.AddWithValue("@Price", color.Price)
                cmd.Parameters.AddWithValue("@DisplayOrder", color.DisplayOrder)
                cmd.Parameters.AddWithValue("@InStock", color.InStock)

                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    <WebMethod>
    Public Shared Sub DeleteColor(id As Integer)
        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand("DELETE FROM Colors WHERE Id = @Id", conn)
                cmd.Parameters.AddWithValue("@Id", id)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    <WebMethod>
    Public Shared Sub UpdateDisplayOrder(colors As List(Of DisplayOrderItem))
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Using transaction As NpgsqlTransaction = conn.BeginTransaction()
                Try
                    For Each color In colors
                        Using cmd As New NpgsqlCommand("UPDATE Colors SET DisplayOrder = @DisplayOrder WHERE Id = @Id", conn)
                            cmd.Parameters.AddWithValue("@Id", color.Id)
                            cmd.Parameters.AddWithValue("@DisplayOrder", color.DisplayOrder)
                            cmd.ExecuteNonQuery()
                        End Using
                    Next
                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

End Class

Public Class ColorItem
    Public Property Id As Integer
    Public Property ColorName As String
    Public Property Price As Decimal
    Public Property DisplayOrder As Integer
    Public Property InStock As Boolean
End Class

Public Class DisplayOrderItem
    Public Property Id As Integer
    Public Property DisplayOrder As Integer
End Class
