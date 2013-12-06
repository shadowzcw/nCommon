using System.Reflection;

namespace nCommon.IOC.Castle
{
    public class MethodExecutionArgs
    {
        public MethodExecutionArgs(object instance, MethodInfo method, object[] arrguments)
        {
            Arguments = arrguments;
            Exception = null;
            FlowBehavior = FlowBehavior.Default;
            Instance = instance;
            Method = method;
            ReturnValue = null;
        }

        /// <summary>
        /// Gets the arguments with which the method has been invoked. 
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        /// Gets the exception currently flying. 
        /// </summary>
        public System.Exception Exception { get; set; }

        /// <summary>
        /// Determines the control flow of the target method once the advice is exited. 
        /// </summary>
        public FlowBehavior FlowBehavior { get; set; }

        /// <summary>
        /// Gets or sets the object instance on which the method is being executed. 
        /// </summary>
        public object Instance { get; private set; }

        /// <summary>
        /// Gets the method being executed. 
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Gets or sets the method return value.
        /// </summary>
        public object ReturnValue { get; set; }
    }

    /// <summary>
    /// Default:
    ///		Default flow behavior for the current method. For OnEntry or OnExit, 
    ///		the fault flow is Continue, for OnException it is RethrowException.
    ///	Continue:
    ///		Continue normally (in an OnException advice, does not rethrow the exception).
    /// RethrowException:
    ///		The current exception will be rethrown. Available only for OnException. 
    ///	Return:
    ///		Return immediately from the current method. Available only for OnEntry and OnException. 
    ///		Note that you may want to set the ReturnValue  property, otherwise you may get a NullReferenceException.
    /// </summary>
    public enum FlowBehavior : int
    {
        Default = 0,
        Continue = 1,
        RethrowException = 2,
        Return = 3
    }
}
