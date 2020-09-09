using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using MiniProject.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniProject.BusinessLogic.Infrastructure
{
    abstract class ModelsCache<T>
        where T : class, IModel
    {
        protected static readonly Lazy<ICacheManager<IEnumerable<T>>> CacheManager;
        protected static readonly object DataLock = new object();
        protected static readonly object StateLock = new object();
        protected static DateTime RefreshedOn = DateTime.MinValue;

        protected enum State
        {
            Empty,
            OnLine,
            Expired,
            Refreshing
        }

        static ModelsCache()
        {
            try
            {
                CacheManager = new Lazy<ICacheManager<IEnumerable<T>>>(
                                    CacheFactory.Build<IEnumerable<T>>($"{nameof(T)}Cache", p => p.WithDictionaryHandle()));
            }
            catch { }
        }

        protected ModelsCache(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        static volatile State CacheCurrentState = State.Empty;

        protected abstract string GetEndPointName();

        protected static TimeSpan GetLifetime() => TimeSpan.FromHours(12);
        protected IConfiguration Configuration { get; }

        protected ICacheManager<IEnumerable<T>> Cache
        {
            get
            {
                switch (CacheCurrentState)
                {
                    case State.OnLine:
                        var timeSpentInCache = (DateTime.UtcNow - RefreshedOn);
                        if (timeSpentInCache > GetLifetime())
                        {
                            lock (StateLock)
                            {
                                if (CacheCurrentState == State.OnLine) CacheCurrentState = State.Expired;
                            }
                        }
                        break;

                    case State.Empty:
                        lock (DataLock)
                        {
                            lock (StateLock)
                            {
                                if (CacheCurrentState == State.Empty)
                                {
                                    GetData();
                                    RefreshedOn = DateTime.UtcNow;
                                    CacheCurrentState = State.OnLine;
                                }
                            }
                        }
                        break;

                    case State.Expired:
                        lock (StateLock)
                        {
                            if (CacheCurrentState == State.Expired)
                            {
                                CacheCurrentState = State.Refreshing;
                                Refresh();
                            }
                        }
                        break;

                }

                return CacheManager.Value;
            }
        }

        protected void Refresh()
        {
            if (CacheCurrentState == State.Refreshing)
            {
                lock (StateLock)
                {
                    lock (DataLock)
                    {
                        ClearCache();
                        GetData();
                        RefreshedOn = DateTime.UtcNow;
                        CacheCurrentState = State.OnLine;
                    }
                }
            }
        }

        protected void GetData()
        {
            var endpointAddres = Configuration.GetValue<string>($"RemoteEndPoints:{GetEndPointName()}");
            var service = GetServiceConsumer(endpointAddres);
            var items = service.GetAllAsync().Result;
            if (!items.Any()) return;
            AddItems(items);
        }

        protected abstract IServiceConsumer<T> GetServiceConsumer(string endpointAddress);

        protected void ClearCache()
        {
            CacheManager.Value.Clear();
        }

        protected abstract void AddItems(IEnumerable<T> item);

        public IEnumerable<T> TryGetItemsByKey(string key)
        {
            return Cache.Get(key);
        }
    }
}
