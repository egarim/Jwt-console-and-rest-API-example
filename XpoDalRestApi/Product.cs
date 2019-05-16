using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;
using Xpo.RestDataStore;

namespace Xpo.RestDataStoreClient
{
    [Persistent("TestTable_Prod")]
    public class Product : XPObject, IClientId
    {
        public Product(Session session) : base(session)
        { }

        private string clientId;
        private bool discontinued;
        private Category category;
        private string description;
        private string code;
        private string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ClientId
        {
            get => clientId;
            set => SetPropertyValue(nameof(ClientId), ref clientId, value);
        }

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

        [Size(500)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        public Category Category
        {
            get => category;
            set => SetPropertyValue(nameof(Category), ref category, value);
        }

        public bool Discontinued
        {
            get => discontinued;
            set => SetPropertyValue(nameof(Discontinued), ref discontinued, value);
        }
    }
}