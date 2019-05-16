using DevExpress.Xpo;
using System;

namespace Xpo.RestDataStoreClient
{
    [Persistent("TestTable_Cat")]
    public class Category : XPObject
    {
        public Category(Session session) : base(session)
        { }

        private string code;
        private string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }
    }
}