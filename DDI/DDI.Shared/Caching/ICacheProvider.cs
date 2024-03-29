﻿using System;

namespace DDI.Shared.Caching
{
    public interface ICacheProvider
    {
        T GetEntry<T>(string key, int timeoutSeconds, bool isSlidingTimeout, Func<T> entryProvider, Action<T> callback) where T : class;
        void SetEntry<T>(string key, T entry, int _timeoutSecs, bool isSlidingTimeout, Action<T> callback) where T : class;
        void RemoveEntry(string key);
    }
}
