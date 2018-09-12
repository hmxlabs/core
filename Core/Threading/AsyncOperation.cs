using System;
using System.Threading;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// An abstract class that Derives from <code>AsyncResultBase</code> to provide some basic functionality
    /// to start and stop the async operation.
    /// </summary>
    public abstract class AsyncOperation : AsyncResultBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        protected AsyncOperation()
        {
            CompletedSynchronously = false;
        }

        /// <summary>
        /// A function that must be implemented by derived classes. This defines the work to be done async.
        /// </summary>
        protected abstract void PerformOperation();

        /// <summary>
        /// Starts the work to be done async on the default threadpool.
        /// </summary>
        protected virtual void StartOperation()
        {
            ThreadPool.QueueUserWorkItem(OperationStart, this);
        }

        /// <summary>
        /// Sets up the async result and then calls through to <code>StartOperation</code>
        /// </summary>
        /// <param name="callback_">The callback to be notified on upon completion</param>
        /// <param name="state_">User state to be maintained</param>
        protected void DoBeginExecuteOperation(AsyncCallback callback_, object state_)
        {
            Callback = callback_;
            AsyncState = state_;
            StartOperation();
        }

        /// <summary>
        /// Static method used by the <code>StartOperation</code> method as the delegate target.
        /// 
        /// Performs the async operation (by calling <code>PerformOperation</code> and handles exceptions
        /// and notifies completion on the provided callback.
        /// </summary>
        /// <param name="operation_"></param>
        protected static void OperationStart(object operation_)
        {
            var asyncOp = operation_ as AsyncOperation;
            if (null == asyncOp)
                throw new ArgumentException("The parameter was not of type AsyncThreadPoolOperation<T>");

            try
            {
                asyncOp.PerformOperation();
            }
            catch (Exception exp)
            {
                asyncOp.AsyncException = exp;
            }
            finally
            {
                asyncOp.OnOperationCompleted();
            }
        }
    }
}
