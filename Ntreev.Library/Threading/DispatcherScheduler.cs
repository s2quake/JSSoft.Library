﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.Library.Threading
{
    public sealed class DispatcherScheduler : TaskScheduler
    {
        private static readonly object lockobj = new object();
        private readonly Dispatcher dispatcher;
        private readonly CancellationToken cancellation;
        private readonly BlockingCollection<Task> taskQueue = new BlockingCollection<Task>();
        private readonly ManualResetEvent eventSet = new ManualResetEvent(false);
        private bool isExecuting;
        private bool isRunning;

        internal DispatcherScheduler(Dispatcher dispatcher, CancellationToken cancellation)
        {
            this.dispatcher = dispatcher;
            this.cancellation = cancellation;
        }

        public int ProcessAll()
        {
            return this.ProcessAll(int.MaxValue);
        }

        public int ProcessAll(int milliseconds)
        {
            this.dispatcher.VerifyAccess();
            if (this.isRunning == true)
                throw new InvalidOperationException("scheduler is already running.");
            if (this.eventSet.WaitOne(0) == false)
                return 0;

            var dateTime = DateTime.Now;
            var count = 0;
            while (this.taskQueue.TryTake(out var task))
            {
                this.isExecuting = true;
                this.TryExecuteTask(task);
                this.isExecuting = false;
                count++;
                var span = DateTime.Now - dateTime;
                if (span.TotalMilliseconds > milliseconds)
                    break;
            }
            return count;
        }

        public bool ProcessOnce()
        {
            this.dispatcher.VerifyAccess();
            if (this.isRunning == true)
                throw new InvalidOperationException("scheduler is already running.");
            if (this.eventSet.WaitOne(0) == false)
                return false;
            if (this.taskQueue.TryTake(out var task) == true)
            {
                this.isExecuting = true;
                this.TryExecuteTask(task);
                this.isExecuting = false;
            }
            return this.taskQueue.Count != 0;
        }

        public new static DispatcherScheduler Current => Dispatcher.Current.Scheduler;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return null;
        }

        protected override void QueueTask(Task task)
        {
            lock (lockobj)
            {
                this.taskQueue.Add(task, this.cancellation);
                this.eventSet.Set();
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued)
                return false;

            return this.isExecuting && TryExecuteTask(task);
        }

        internal void Proceed()
        {
            this.eventSet.Set();
        }

        internal bool Continue { get; set; } = true;

        internal void Run()
        {
#if DEBUG
            // 프로그램 종료시에도 Dispatcher가 여전히 실행중이라면 아래의 변수를 참고하여 
            // Dispatcher가 정상적으로 Dispose 되는지 확인합니다.
            var owner = this.dispatcher.Owner;
            var stackStace = this.dispatcher.StackTrace;
#endif
            this.isRunning = true;
            while (true)
            {
                if (this.taskQueue.TryTake(out var task) == true)
                {
                    this.isExecuting = true;
                    this.TryExecuteTask(task);
                    this.isExecuting = false;
                    this.eventSet.Set();
                }
                else if (this.Continue == false)
                {
                    break;
                }
                else
                {
                    this.eventSet.WaitOne();
                    this.eventSet.Reset();
                }
            }
            this.isRunning = false;
        }
    }
}
