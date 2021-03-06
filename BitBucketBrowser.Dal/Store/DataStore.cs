﻿namespace BitBucketBrowser.Dal.Store
{
    using System;

    using BitBucketBrowser.Common.Interfaces;
    using BitBucketBrowser.Common.Properties;
    using BitBucketBrowser.Common.Service.Interfaces;

    using Raven.Client;
    using Raven.Client.Embedded;
    using Raven.Client.Listeners;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DataStore : IDisposable, IDataStore
    {
        private readonly EmbeddableDocumentStore documentStore;

        public DataStore(IAppConfigService appConfigService)
        {                
            // ReSharper disable once UseObjectOrCollectionInitializer
            this.documentStore = new EmbeddableDocumentStore
                                    {
                                        DataDirectory = appConfigService.GetDatabaseConnectionString(),                                                                                
                                    };

#if DEBUG
            this.documentStore.UseEmbeddedHttpServer = true;
#else 
            this.documentStore.UseEmbeddedHttpServer = false;
#endif

            this.documentStore.Initialize();
            this.documentStore.RegisterListener(new NonStaleQueryListener());
        }

        public void Dispose()
        {
            this.documentStore.Dispose();
        }

        public IDocumentSession OpenSession()
        {
            return this.documentStore.OpenSession();
        }
        
        public class NonStaleQueryListener : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
            {
                customization.WaitForNonStaleResults();
            }
        }
    }
}