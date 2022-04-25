<<<<<<< HEAD:KuCoinApiClient/Utils/FixedSizeQueue.cs
﻿using System.Collections.Generic;

namespace KuCoinApiClient.Utils
{
=======
﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// https://jonlabelle.com/snippets/view/csharp/fixed-size-queue

namespace KuCoinApiClient.Utils
{

>>>>>>> MaxSizeBuffer:KukoinServer/Utils/FixedSizeQueue.cs
    /// <summary>
    /// A light fixed size queue.
    /// If Enqueue is called and queue's limit has reached the last item will be removed.
    /// This data structure is thread safe.
    /// </summary>
    public class FixedSizeQueue<T>
    {
        private readonly int _maxSize;
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _queueLockObj = new object();

        public FixedSizeQueue(int maxSize)
        {
            _maxSize = maxSize;
        }

        public void Enqueue(T item)
        {
            lock (_queueLockObj)
            {
                if (_queue.Count == _maxSize)
                {
                    _queue.Dequeue();
                }

                _queue.Enqueue(item);
            }
        }

        public bool Contains(T item)
        {
            lock (_queueLockObj)
            {
                return _queue.Contains(item);
            }
        }

        public T[] ToArray()
        {
            return _queue.ToArray();
        }

        public int Count => _queue.Count;
    }
}
