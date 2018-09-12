using System;
using System.Threading;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Simply an <code>IAsyncResult</code> wrapped to allow additional (typed!) state to be carried
    /// beyond the singular state object provided by IAsyncResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WrappedAsyncResult<T> : IAsyncResult
    {
        /// <summary>
        /// Constructor. The <code>asyncResult_</code> parametere can not be null.
        /// </summary>
        /// <param name="asyncResult_"></param>
        /// <param name="additionalState_"></param>
        public WrappedAsyncResult(IAsyncResult asyncResult_, T additionalState_)
        {
            if (null == asyncResult_)
                throw new ArgumentNullException(nameof(asyncResult_));

            AsyncResult = asyncResult_;
            AdditionalState = additionalState_;
        }

        /// <summary>
        /// Pass through call to the underlying <code>IAsyncResult.IsCompleted</code>
        /// </summary>
        public bool IsCompleted => AsyncResult.IsCompleted;

        /// <summary>
        /// Pass through call to the underlying <code>IAsyncResult.AsyncWaitHandle</code>
        /// </summary>
        public WaitHandle AsyncWaitHandle => AsyncResult.AsyncWaitHandle;

        /// <summary>
        /// Pass through call to the underlying <code>IAsyncResult.AsyncState</code>
        /// </summary>
        public object AsyncState => AsyncResult.AsyncState;

        /// <summary>
        /// Pass through call to the underlying <code>IAsyncResult.CompletedSynchronously</code>
        /// </summary>
        public bool CompletedSynchronously => AsyncResult.CompletedSynchronously;

        /// <summary>
        /// The IAsyncResult that this object wraps
        /// </summary>
        public IAsyncResult AsyncResult { get; }

        /// <summary>
        /// The additional state to be carried
        /// </summary>
        public T AdditionalState { get; set; }
    }
}
