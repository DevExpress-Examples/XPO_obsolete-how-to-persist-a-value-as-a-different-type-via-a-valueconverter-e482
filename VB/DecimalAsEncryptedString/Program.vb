Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata

Namespace DecimalAsEncryptedString
    Public Class Program
        Shared Sub Main()
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(AutoCreateOption.DatabaseAndSchema)
            XpoDefault.Session.ClearDatabase()

            Const TestValue As Decimal = -12.345678D

            Using uow As New UnitOfWork()
                Dim obj As New TestObject(uow)
                obj.Value = TestValue
                uow.CommitChanges()
            End Using

            Using uow As New UnitOfWork()
                Dim obj As TestObject = uow.FindObject(Of TestObject)(Nothing)
                System.Diagnostics.Debug.Assert(Equals(TestValue, obj.Value))
            End Using
        End Sub
    End Class
    Public Class TestObject
        Inherits XPObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Private _Value As Decimal
        <ValueConverter(GetType(DecimalEncoderConverter))> _
        Public Property Value() As Decimal
            Get
                Return _Value
            End Get
            Set(ByVal value As Decimal)
                _Value = value
            End Set
        End Property
    End Class

    Public Class DecimalEncoderConverter
        Inherits ValueConverter

        Public Overrides ReadOnly Property StorageType() As Type
            Get
                Return GetType(String)
            End Get
        End Property
        Public Overrides Function ConvertFromStorageType(ByVal value As Object) As Object
            Try
                Dim encoded As String = DirectCast(value, String)
                Dim decoded As Decimal = Convert.ToDecimal(encoded)
                Return decoded
            Catch
                Return 0D
            End Try
        End Function
        Public Overrides Function ConvertToStorageType(ByVal value As Object) As Object
            Dim decoded As Decimal = DirectCast(value, Decimal)
            Dim encoded As String = decoded.ToString()
            Return encoded
        End Function
    End Class
End Namespace
