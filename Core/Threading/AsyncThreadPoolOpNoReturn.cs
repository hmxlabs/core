using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Implementation of <code>IAsyncOperationNoReturn</code>.
    /// 
    /// See <code>IAsyncOperationNoReturn</code> for more details
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    public class AsyncThreadPoolOpNoReturn<TA> : AsyncOperation, IAsyncOperationNoReturn<TA>
    {
        /// <summary>
        /// /// See <code>IAsyncOperationNoReturn.BeginExecuteOperation</code> for more details
        /// </summary>
        /// <param name="action_"></param>
        /// <param name="args_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public void BeginExecuteOperation(AsyncActionNoReturn<TA> action_, TA args_, AsyncCallback callback_, object state_)
        {
            if (null == action_)
                throw new ArgumentNullException(nameof(action_));

            Action = action_;
            Arguments = args_;
            DoBeginExecuteOperation(callback_, state_);
        }

        /// <summary>
        /// See <code>IAsyncOperationNoReturn.EndExecuteOperation</code> for more details
        /// </summary>
        public void EndExecuteOperation()
        {
            DoEndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationNoReturn.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="operation_"></param>
        public void EndExecuteOperation(AsyncActionNoReturn<TA> operation_)
        {
            if (null == operation_)
                throw new ArgumentNullException(nameof(operation_));

            if (!operation_.Method.Equals(Action.Method))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationNoReturn.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="methodName_"></param>
        public void EndExecuteOperation(string methodName_)
        {
            if (string.IsNullOrWhiteSpace(methodName_))
                throw new ArgumentNullException(nameof(methodName_));

            if (!methodName_.Equals(Action.Method.Name))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationNoReturn.Action</code> for more details
        /// </summary>
        public AsyncActionNoReturn<TA> Action { get; private set; }

        /// <summary>
        /// See <code>IAsyncOperationNoReturn.Arguments</code> for more details
        /// </summary>
        public TA Arguments { get; private set; }

        /// <summary>
        /// Overrides abstract method in base class to actually perform the async operation required
        /// </summary>
        protected override void PerformOperation()
        {
            if (null == Action)
                throw new InvalidOperationException("No operation was specified!");

            Action(Arguments);
        }
    }
}
