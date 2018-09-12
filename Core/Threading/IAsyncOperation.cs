using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// A templated function pointer used to provide the code to be executed asynchronously.
    /// </summary>
    /// <typeparam name="TR">The return type of the function</typeparam>
    /// <typeparam name="TA">The parameter type of the function</typeparam>
    /// <param name="args_">The parameters to pass to the function</param>
    /// <returns></returns>
    public delegate TR AsyncAction<out TR, in TA>(TA args_);

    /// <summary>
    /// A templated interface extending IAsyncResult that is used to help create async versions of
    /// functions.
    /// 
    /// This effectively wraps the synchronous code and acts as the IAsyncResult also.
    /// 
    /// NOTE: This code pre dates the TPL and the Task based async model now
    /// available in .NET. You should probably be using that in preference
    /// to this class.
    /// </summary>
    /// <typeparam name="TR">The return type of the function to be made async</typeparam>
    /// <typeparam name="TA">The arguments of the function to be made async</typeparam>
    public interface IAsyncOperation<TR, TA> : IAsyncResult
    {
        /// <summary>
        /// Starts execution of the synchronous code provided using the <code>AsyncAction</code> delegate.
        /// </summary>
        /// <param name="action_">A function pointer to the code to execute async</param>
        /// <param name="args_">The parameters to pass to that function</param>
        /// <param name="callback_">A callback to be notified on upon completion</param>
        /// <param name="state_">User/client state to maintain</param>
        void BeginExecuteOperation(AsyncAction<TR, TA> action_, TA args_, AsyncCallback callback_, object state_);

        /// <summary>
        /// End the operation. This is the least safe version of this overload as it will not check if you are indeed
        /// attempting to end the same peice of code that was invoked.
        /// </summary>
        /// <returns></returns>
        TR EndExecuteOperation();

        /// <summary>
        /// End the operation and validate that this async state does belong to the same function that it was invoked
        /// for in the first instance
        /// </summary>
        /// <param name="operation_"></param>
        /// <returns>The results of the async operation</returns>
        TR EndExecuteOperation(AsyncAction<TR, TA> operation_);

        /// <summary>
        /// End the operation and validate that this async state does belong to the same function that it was invoked
        /// for in the first instance, however only the name of the function is verified.
        /// </summary>
        /// <param name="methodName_"></param>
        /// <returns>The results of the async operation</returns>
        TR EndExecuteOperation(string methodName_);

        /// <summary>
        /// Read only property of tThe async code to execute
        /// </summary>
        AsyncAction<TR, TA> Action { get; }

        /// <summary>
        /// Read only property providng the async callback to notify on upon completion
        /// </summary>
        AsyncCallback Callback { get; }

        /// <summary>
        /// The parameters to provide to the async action.
        /// </summary>
        TA Arguments { get;  }

        /// <summary>
        /// The results of the async operation.
        /// </summary>
        TR Results { get; }
    }
}
