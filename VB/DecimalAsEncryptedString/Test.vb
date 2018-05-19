Imports System
Imports System.Collections.Generic
Imports System.Text
Imports NUnit.Framework
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB

Namespace DecimalAsEncryptedString
    <TestFixture> _
    Public Class Test
        <Test> _
        Public Sub Run()
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
                Assert.AreEqual(TestValue, obj.Value)
            End Using
        End Sub
    End Class
End Namespace
