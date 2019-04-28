using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace Xpo.RestDataStore
{
    public class RestObjectLayer : SimpleObjectLayer
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RestObjectLayer"/> class with a data access layer.</para>
        /// </summary>
        /// <param name="dataLayer">An object which implements the <see cref="DevExpress.Xpo.IDataLayer"/> interface.</param>
        public RestObjectLayer(IDataLayer dataLayer) : base(dataLayer)
        {
        }

        private ObjectsQuery[] PatchCriteria(ObjectsQuery[] queries)
        {
            for (int i = 0; i < queries.Length; i++)
            {
                queries[i] = PatchCriteria(queries[i]);
            }
            return queries;
        }

        private ObjectsQuery PatchCriteria(ObjectsQuery query)
        {
            // query.Criteria = GroupOperator.Combine(GroupOperatorType.And, query.Criteria, new BinaryOperator("CompanyID", "test"));
            return query;
        }
    }
}