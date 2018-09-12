using System;
using System.Threading;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// An abstract base implementation of an <code>IAsyncResult</code>
    /// that is used as the base class a number of concrete implementations
    /// provided in this namespace.
    /// 
    /// NOTE: This code pre dates the TPL and the Task based async model now
    /// available in .NET. You should probably be using that in preference
    /// to this class.
    /// </summary>
    public abstract class AsyncResultBase : IAsyncResult
    {
        /// <summary>
        /// The async state to be mainted for the client code.
        /// 
        /// See <code>IAsyncResult.AsyncState</code> for more details
        /// </summary>
        public object AsyncState { get; protected set; }

        /// <summary>
        /// The callback to notify the caller on upon completion.
        /// 
        /// See <code>IAsyncResult.IsCompleted</code> for more details
        /// </summary>
        public AsyncCallback Callback { get; protected set; }

        /// <summary>
        /// Property indicating of the async operation has been completed.
        /// 
        /// See <code>IAsyncResult.AsyncState</code> for more details
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                lock (_lock)
                {
                    return _isComplete;
                }
            }

            protected set
            {
                lock (_lock)
                {
                    _isComplete = value;
                }
            }
        }

        /// <summary>
        /// See <code>IAsyncResult.AsyncWaitHandle</code> for more details
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
        }

        /// <summary>
        /// Property indicating if the operation completed synchronously.
        /// 
        /// See <code>IAsyncResult.CompletedSynchronously</code> for more details
        /// </summary>
        public bool CompletedSynchronously { get; protected set; }

        /// <summary>
        /// Provide access to the ManualResetEvent used as the WaitHandle to derived classes
        /// </summary>
        protected ManualResetEvent WaitHandle
        {
            get { return _waitHandle; }
        }

        /// <summary>
        /// Protected property used to store the exception (if one is thrown) during the async operation.
        /// </summary>
        protected Exception AsyncException { get; set; }

        /// <summary>
        /// End the operation. This will signal the WaitHandle.
        /// 
        /// If an exception was raised during the async operation, this will be rethrown here.
        /// </summary>
        protected void DoEndExecuteOperation()
        {
            WaitHandle.WaitOne(); // Ensure the operation has completed before returning.
            if (null == AsyncException)
                return;

            var errMessage = $"The async operation threw an exception: {AsyncException}";
            throw new Exception(errMessage, AsyncException);
        }

        /// <summary>
        /// Signal completion of the operation and invoke the client callabck
        /// </summary>
        protected void OnOperationCompleted()
        {
            IsCompleted = true;
            WaitHandle.Set();

            Callback?.Invoke(this);
        }

        private bool _isComplete;
        private readonly object _lock = new object();
        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);
    }
}
