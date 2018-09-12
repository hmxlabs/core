using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Implementation of <code>IAsyncOperationSimple</code>.
    /// 
    /// See <code>IAsyncOperationSimple</code> for more details
    /// </summary>
    public class AsyncThreadPoolOpSimple : AsyncOperation, IAsyncOperationSimple
    {
        /// <summary>
        /// See <code>IAsyncOperationSimple.BeginExecuteOperation</code> for more details
        /// </summary>
        /// <param name="action_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public void BeginExecuteOperation(AsyncActionSimple action_, AsyncCallback callback_, object state_)
        {
            if (null == action_)
                throw new ArgumentNullException("action_");

            Action = action_;
            DoBeginExecuteOperation(callback_, state_);
        }

        /// <summary>
        /// See <code>IAsyncOperationSimple.EndExecuteOperation</code> for more details
        /// </summary>
        public void EndExecuteOperation()
        {
            DoEndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationSimple.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="operation_"></param>
        public void EndExecuteOperation(AsyncActionSimple operation_)
        {
            if (null == operation_)
                throw new ArgumentNullException("operation_");

            if (!operation_.Method.Equals(Action.Method))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationSimple.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="methodName_"></param>
        public void EndExecuteOperation(string methodName_)
        {
            if (string.IsNullOrWhiteSpace(methodName_))
                throw new ArgumentNullException("methodName_");

            if (!methodName_.Equals(Action.Method.Name))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationSimple..Action</code> for more details
        /// </summary>
        public AsyncActionSimple Action { get; private set; }

        /// <summary>
        /// Overrides abstract method in base class to actually perform the async operation required
        /// </summary>
        protected override void PerformOperation()
        {
            if (null == Action)
                throw new InvalidOperationException("No operation was specified!");

            Action();
        }
    }
}
