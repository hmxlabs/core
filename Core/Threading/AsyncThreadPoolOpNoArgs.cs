using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Implemenataion of <code>IAsyncOperationNoArgs</code>.
    /// 
    /// See <code>IAsyncOperationNoArgs</code> for more details
    /// </summary>
    /// <typeparam name="TR"></typeparam>
    public class AsyncThreadPoolOpNoArgs<TR> : AsyncOperation, IAsyncOperationNoArgs<TR>
    {
        /// <summary>
        /// See <code>IAsyncOperationNoArgs.BeginExecuteOperation</code> for more details
        /// </summary>
        /// <param name="action_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public void BeginExecuteOperation(AsyncActionNoArgs<TR> action_, AsyncCallback callback_, object state_)
        {
            if (null == action_) throw new ArgumentNullException("action_");

            Action = action_;
            DoBeginExecuteOperation(callback_, state_);
        }

        /// <summary>
        /// See <code>IAsyncOperationNoArgs.EndExecuteOperation</code> for more details
        /// </summary>
        /// <returns></returns>
        public TR EndExecuteOperation()
        {
            DoEndExecuteOperation();
            return Results;
        }

        /// <summary>
        /// See <code>IAsyncOperationNoArgs.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="operation_"></param>
        /// <returns></returns>
        public TR EndExecuteOperation(AsyncActionNoArgs<TR> operation_)
        {
            if (null == operation_)
                throw new ArgumentNullException("operation_");

            if (!operation_.Method.Equals(Action.Method))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            return EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationNoArgs.EndExecuteOperation</code> for more details
        /// </summary>
        /// <param name="methodName_"></param>
        /// <returns></returns>
        public TR EndExecuteOperation(string methodName_)
        {
            if (string.IsNullOrWhiteSpace(methodName_))
                throw new ArgumentNullException("methodName_");

            if (!methodName_.Equals(Action.Method.Name))
                throw new ArgumentException("Attempt to end operation with non corresponding instance of AsyncThreadPoolOperation");

            return EndExecuteOperation();
        }

        /// <summary>
        /// See <code>IAsyncOperationNoArgs.Action</code> for more details
        /// </summary>
        public AsyncActionNoArgs<TR> Action { get; private set; }

        /// <summary>
        /// See <code>IAsyncOperationNoArgs.Results</code> for more details
        /// </summary>
        public TR Results { get; private set; }

        /// <summary>
        /// Overrides abstract method in base class to actually perform the async operation required
        /// </summary>
        protected override void PerformOperation()
        {
            if (null == Action)
                throw new InvalidOperationException("No operation was specified!");

            Results = Action();
        }
    }
}
