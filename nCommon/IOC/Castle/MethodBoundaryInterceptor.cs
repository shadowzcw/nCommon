using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace nCommon.IOC.Castle
{
    public class MethodBoundaryInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            List<MethodBoundaryAttribute> attributes = GetAttributes(methodInfo);

            if (attributes.Count == 0)
            {
                invocation.Proceed();
            }
            else
            {
                MethodExecutionArgs args = new MethodExecutionArgs(
                    invocation.InvocationTarget, invocation.Method, invocation.Arguments);

                InvokeEntry(attributes, args);

                try
                {
                    if (args.FlowBehavior == FlowBehavior.Return)
                        invocation.ReturnValue = args.ReturnValue;
                    else
                        invocation.Proceed();

                    args.ReturnValue = invocation.ReturnValue;

                    InvokeSuccess(attributes, args);
                }
                catch (System.Exception err)
                {
                    args.Exception = err;

                    InvokeException(attributes, args);

                    if (args.FlowBehavior == FlowBehavior.Return)
                    {
                        invocation.ReturnValue = args.ReturnValue;
                        return;
                    }

                    if (args.FlowBehavior != FlowBehavior.Continue)
                        throw err;
                }
                finally
                {
                    InvokeExit(attributes, args);
                }
            }
        }

        private List<MethodBoundaryAttribute> GetAttributes(MethodInfo methodInfo)
        {
            List<MethodBoundaryAttribute> attributes = new List<MethodBoundaryAttribute>();

            //Method level attributes
            MethodBoundaryAttribute[] methodAttributes = (MethodBoundaryAttribute[])methodInfo.GetCustomAttributes(typeof(MethodBoundaryAttribute), false);

            //Class level attributes
            MethodBoundaryAttribute[] classAttributes = (MethodBoundaryAttribute[])methodInfo.ReflectedType.GetCustomAttributes(typeof(MethodBoundaryAttribute), false);

            //add method level
            attributes.AddRange(methodAttributes);

            //add class attribute if not exist in method
            foreach (MethodBoundaryAttribute classAttr in classAttributes)
            {
                bool exist = false;
                foreach (MethodBoundaryAttribute methodAttr in methodAttributes)
                {
                    if (classAttr.TypeId == methodAttr.TypeId)
                        exist = true;
                }
                if (!exist)
                    attributes.Add(classAttr);
            }

            return attributes;
        }

        private void InvokeEntry(List<MethodBoundaryAttribute> attributes, MethodExecutionArgs args)
        {
            foreach (MethodBoundaryAttribute attr in attributes)
            {
                attr.OnEntry(args);
            }
        }

        private void InvokeException(List<MethodBoundaryAttribute> attributes, MethodExecutionArgs args)
        {
            foreach (MethodBoundaryAttribute attr in attributes)
            {
                attr.OnException(args);
            }
        }

        private void InvokeExit(List<MethodBoundaryAttribute> attributes, MethodExecutionArgs args)
        {
            foreach (MethodBoundaryAttribute attr in attributes)
            {
                attr.OnExit(args);
            }
        }

        private void InvokeSuccess(List<MethodBoundaryAttribute> attributes, MethodExecutionArgs args)
        {
            foreach (MethodBoundaryAttribute attr in attributes)
            {
                attr.OnSuccess(args);
            }
        }
    }
}
