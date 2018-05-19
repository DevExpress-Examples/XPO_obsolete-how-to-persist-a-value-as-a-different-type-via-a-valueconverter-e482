using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace DecimalAsEncryptedString {
    public class Program {
        static void Main() {
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
                System.Diagnostics.Debug.Assert(Equals(TestValue, obj.Value));
            }
        }
    }
    public class TestObject: XPObject {
        public TestObject(Session session)
            : base(session) { }

        private decimal _Value;
        [ValueConverter(typeof(DecimalEncoderConverter))]
        public decimal Value {
            get { return _Value; }
            set {
                _Value = value;
            }
        }
    }

    public class DecimalEncoderConverter : ValueConverter{
        public override Type StorageType {
            get { return typeof(string); }
        }
        public override object ConvertFromStorageType(object value) {
            try {
                string encoded = (string)value;
                decimal decoded = Convert.ToDecimal(encoded);
                return decoded;
            }
            catch {
                return 0m;
            }
        }
        public override object ConvertToStorageType(object value) {
            decimal decoded = (decimal)value;
            string encoded = decoded.ToString();
            return encoded;
        }
    }
}
