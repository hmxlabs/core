using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Simplest possible concrete implementation of the <code>IAsyncResultBase</code>
    /// 
    /// Provide no additional functionality and simply exposes the raw operations as public
    /// methods.
    /// </summary>
    public class AsyncResult : AsyncResultBase
    {
        /// <summary>
        /// Constructor taking the callback and client state to maintain.
        /// </summary>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public AsyncResult(AsyncCallback callback_, object state_)
        {
            Callback = callback_;
            AsyncState = state_;
        }

        /// <summary>
        /// End the async operation.
        /// </summary>
        public void EndOperation()
        {
            DoEndExecuteOperation();
        }

        /// <summary>
        /// Signal completion of the async operation with an exception
        /// </summary>
        /// <param name="exception_"></param>
        public void CompleteOperation(Exception exception_)
        {
            AsyncException = exception_;
            OnOperationCompleted();
        }

        /// <summary>
        /// Signal completion of the async operation normally.
        /// </summary>
        public void CompleteOperation()
        {
            OnOperationCompleted();
        }
    }
}
