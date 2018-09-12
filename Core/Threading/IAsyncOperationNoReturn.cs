using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Function pointer that takes an argument but has not return value
    /// </summary>
    /// <typeparam name="TA">The type of the parameter to take</typeparam>
    /// <param name="args_">The arguments to the function pointer</param>
    public delegate void AsyncActionNoReturn<in TA>(TA args_);

    /// <summary>
    /// Extension to <code>IAsyncResult</code> and a sibling of <code>IAsyncOperation</code>
    /// for code that takes an argument but returns nothing.
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    public interface IAsyncOperationNoReturn<TA> : IAsyncResult
    {
        /// <summary>
        /// Starts execution of the synchronous code provided using the <code>AsyncAction</code> delegate.
        /// </summary>
        /// <param name="action_">A function pointer to the code to execute async</param>
        /// <param name="args_">The parameters to pass to that function</param>
        /// <param name="callback_">A callback to be notified on upon completion</param>
        /// <param name="state_">User/client state to maintain</param>
        void BeginExecuteOperation(AsyncActionNoReturn<TA> action_, TA args_, AsyncCallback callback_, object state_);

        /// <summary>
        /// End the operation. This is the least safe version of this overload as it will not check if you are indeed
        /// attempting to end the same peice of code that was invoked.
        /// </summary>
        /// <returns></returns>
        void EndExecuteOperation();

        /// <summary>
        /// End the operation and validate that this async state does belong to the same function that it was invoked
        /// for in the first instance
        /// </summary>
        /// <param name="operation_"></param>
        /// <returns>The results of the async operation</returns>
        void EndExecuteOperation(AsyncActionNoReturn<TA> operation_);

        /// <summary>
        /// End the operation and validate that this async state does belong to the same function that it was invoked
        /// for in the first instance, however only the name of the function is verified.
        /// </summary>
        /// <param name="methodName_"></param>
        /// <returns>The results of the async operation</returns>
        void EndExecuteOperation(string methodName_);

        /// <summary>
        /// Read only property of tThe async code to execute
        /// </summary>
        AsyncActionNoReturn<TA> Action { get; }

        /// <summary>
        /// Read only property providng the async callback to notify on upon completion
        /// </summary>
        AsyncCallback Callback { get; }

        /// <summary>
        /// The parameters to provide to the async action.
        /// </summary>
        TA Arguments { get; }
    }
}
