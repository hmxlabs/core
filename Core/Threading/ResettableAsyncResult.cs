using System;

namespace HmxLabs.Core.Threading
{
    /// <summary>
    /// In Progress. Not currently implemented
    /// </summary>
    public class ResettableAsyncResult : AsyncResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public ResettableAsyncResult(AsyncCallback callback_, object state_) : base(callback_, state_)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        public void Reset(AsyncCallback callback_, object state_)
        {
            
        }
    }
}
