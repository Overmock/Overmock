namespace Overmock.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RuntimeHandlerBase : IRuntimeHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runtimeContext"></param>
        protected RuntimeHandlerBase(RuntimeContext runtimeContext)
        {
            Context = runtimeContext;
        }

        /// <summary>
        /// 
        /// </summary>
        protected RuntimeContext Context { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RuntimeHandlerResult Handle(params object[] parameters)
        {
            return HandleCore(parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected abstract RuntimeHandlerResult HandleCore(params object[] parameters);
    }
}