﻿using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Xpo.RestDataStore
{
    public interface IClientId
    {
        string ClientId { get; set; }
    }


    //HACK the base code for this object is here https://www.devexpress.com/Support/Center/Question/Details/Q446637/how-can-i-filter-data-according-user-permission
    public class RestObjectLayer : SimpleObjectLayer, IObjectLayer
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RestObjectLayer"/> class with a data access layer.</para>
        /// </summary>
        /// <param name="dataLayer">An object which implements the <see cref="DevExpress.Xpo.IDataLayer"/> interface.</param>
        public RestObjectLayer(IDataLayer dataLayer) : base(dataLayer)
        {
        }

        public string ClientId { get; set; }

        public RestObjectLayer(IDataLayer dataLayer, string ClientId) : base(dataLayer)
        {
            //the idea here is to auto set who is the owner of the information
            this.ClientId = ClientId;
        }

        #region IObjectLayer

        void IObjectLayer.CommitChanges(Session session, ICollection fullListForDelete, ICollection completeListForSave)
        {
            base.CommitChanges(session, fullListForDelete, PatchModification(completeListForSave));
        }

        private ICollection PatchModification(ICollection completeListForSave)
        {
            foreach (object item in completeListForSave)
            {
                IClientId clientId = (item as IClientId);
                if (clientId != null)
                {
                    clientId.ClientId = this.ClientId;
                }
            }
            return completeListForSave;
        }

        object IObjectLayer.CommitChangesAsync(Session session, ICollection fullListForDelete, ICollection completeListForSave, AsyncCommitCallback callback)
        {
            return base.CommitChangesAsync(session, fullListForDelete, completeListForSave, callback);
        }

        System.Collections.ICollection[] IObjectLayer.LoadObjects(Session session, ObjectsQuery[] queries)
        {
            return base.LoadObjects(session, PatchCriteria(queries));
        }

        object IObjectLayer.LoadObjectsAsync(Session session, ObjectsQuery[] queries, AsyncLoadObjectsCallback callback)
        {
            return base.LoadObjectsAsync(session, PatchCriteria(queries), callback);
        }

        List<object[]> IObjectLayer.SelectData(Session session, ObjectsQuery query, CriteriaOperatorCollection properties, CriteriaOperatorCollection groupProperties, CriteriaOperator groupCriteria)
        {
            return base.SelectData(session, PatchCriteria(query), properties, groupProperties, groupCriteria);
        }

        object IObjectLayer.SelectDataAsync(Session session, ObjectsQuery query, CriteriaOperatorCollection properties, CriteriaOperatorCollection groupProperties, CriteriaOperator groupCriteria, AsyncSelectDataCallback callback)
        {
            return base.SelectDataAsync(session, PatchCriteria(query), properties, groupProperties, groupCriteria, callback);
        }

        #endregion IObjectLayer

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
            if (typeof(IClientId).IsAssignableFrom(query.ClassInfo.ClassType))
            {
                //the idea here is to query with the owner of the information
                var test = 1;
            }
            // query.Criteria = GroupOperator.Combine(GroupOperatorType.And, query.Criteria, new BinaryOperator("CompanyID", "test"));
            return query;
        }
    }
}