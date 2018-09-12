using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Implementation of <code>IAsyncOperation</code>.
    /// 
    /// See <code>IAsyncOperation</code> for more details
    /// </summary>
    /// <typeparam name="TR"></typeparam>
    /// <typeparam name="TA"></typeparam>
    public class AsyncThreadPoolOperation<TR, TA> : AsyncOperation, IAsyncOperation<TR, TA>
    {
        /// <summary>
        /// See <code>IAsyncOperation.Action</code> for more details
        /// </summary>
        public AsyncAction<TR, TA> Action { get; private set; }

        /// <summary>
        /// See <code>IAsyncOperation.Arguments</code> for more details
        /// </summary>
        public TA Arguments { get; private set; }

        /// <summary>
        /// See <code>IAsyncOperation.Results</code> for more details
        /// </summary>
        public TR Results { get; private set; }

        /// <summary>
        /// See <code>IAsyncOperation.BeginExecuteOperation</code> for more details
        /// </summary>
        /// <param name="action_"></param>
        /// <param name="args_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public void BeginExecuteOperation(AsyncAction<TR, TA> action_, TA args_, AsyncCallback callback_, object state_)
        {
            if (null == action_)
                throw new ArgumentNullException(nameof(action_));

            Action = action_;
            Arguments = args_;
            DoBeginExecuteOperation(callback_, state_);
        }

        /// <summary>
        /// See <code>IAsyncOperation.EndExecuteOperation</code> for more details
        /// </summary>
        /// <returns></returns>
        public TR EndExecuteOperation()
        {
            DoEndExecuteOperation();
            return Results;
        }

        /// <summary>
        /// See <code>IAsyncOperation.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="methodName_"></param>
        /// <returns></returns>
        public TR EndExecuteOperation(string methodName_)
        {
            if (string.IsNullOrWhiteSpace(methodName_))
                throw new ArgumentNullException(nameof(methodName_));

            if (!methodName_.Equals(Action.Method.Name)) 
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            return EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperation.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="operation_"></param>
        /// <returns></returns>
        public TR EndExecuteOperation(AsyncAction<TR, TA> operation_)
        {
            if (null == operation_)
                throw new ArgumentNullException(nameof(operation_));

            if (!operation_.Method.Equals(Action.Method))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation"); 

            return EndExecuteOperation();
        }

        /// <summary>
        /// Overrides abstract method in base class to actually perform the async operation required
        /// </summary>
        protected override void PerformOperation()
        {
            if (null == Action)
                throw new InvalidOperationException("No operation was specified!");

            Results = Action(Arguments);   
        }
    }
}