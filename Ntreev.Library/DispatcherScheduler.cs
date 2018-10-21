﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.Library
{
    class DispatcherScheduler : TaskScheduler
    {
        private static readonly object lockobj = new object();
        private readonly CancellationToken cancellation;
        private readonly BlockingCollection<Task> taskQueue = new BlockingCollection<Task>();
        private ManualResetEvent eventSet = new ManualResetEvent(false);
        private bool isExecuting;

        public DispatcherScheduler(CancellationToken cancellation)
        {
            this.cancellation = cancellation;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return null;
        }

        protected override void QueueTask(Task task)
        {
            this.taskQueue.Add(task, this.cancellation);
            this.eventSet.Set();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued)
                return false;

            return this.isExecuting && TryExecuteTask(task);
        }

        internal void Continue()
        {
            this.eventSet.Set();
        }

        internal void Run()
        {
            while (true)
            {
                if (this.taskQueue.TryTake(out var task) == true)
                {
                    this.isExecuting = true;
                    this.TryExecuteTask(task);
                    this.isExecuting = false;
                    this.eventSet.Set();
                }
                else if (this.cancellation.IsCancellationRequested == true)
                {
                    break;
                }
                else
                {
                    eventSet.WaitOne();
                    eventSet.Reset();
                }
            }
        }
    }
}
