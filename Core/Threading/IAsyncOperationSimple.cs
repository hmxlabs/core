using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// Function pointer to code that takes no arguments and return nothing. Used by
    /// <code>IAsyncOperationSimple</code> as the code it will execute async.
    /// </summary>
    public delegate void AsyncActionSimple();

    /// <summary>
    /// Extension to <code>IAsyncResult</code> and a sibling of <code>IAsyncOperation</code>
    /// for code that takes no arguments and returns nothing
    /// </summary>
    public interface IAsyncOperationSimple : IAsyncResult
    {
        /// <summary>
        /// Starts execution of the synchronous code provided using the <code>AsyncAction</code> delegate.
        /// </summary>
        /// <param name="action_">A function pointer to the code to execute async</param>
        /// <param name="callback_">A callback to be notified on upon completion</param>
        /// <param name="state_">User/client state to maintain</param>
        void BeginExecuteOperation(AsyncActionSimple action_, AsyncCallback callback_, object state_);

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
        void EndExecuteOperation(AsyncActionSimple operation_);

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
        AsyncActionSimple Action { get; }

        /// <summary>
        /// Read only property providng the async callback to notify on upon completion
        /// </summary>
        AsyncCallback Callback { get; }
    }
}