using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace DecimalAsEncryptedString {
    [TestFixture]
    public class Test {
        [Test]
        public void Run() {
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(AutoCreateOption.DatabaseAndSchema);
            XpoDefault.Session.ClearDatabase();

            const decimal TestValue = -12.345678m;

            using(UnitOfWork uow = new UnitOfWork()) {
                TestObject obj = new TestObject(uow);
                obj.Value = TestValue;
                uow.CommitChanges();
            }

            using(UnitOfWork uow = new UnitOfWork()) {
                TestObject obj = uow.FindObject<TestObject>(null);
                Assert.AreEqual(TestValue, obj.Value);
            }
        }
    }
}
