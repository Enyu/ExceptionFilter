using Ninject;

namespace DPE.ExceptionFilter.Service.Utilities
{
    public static class IocContainer
    {
        public static IKernel Initialize()
        {
            const string assemblyName = "DPE.ExceptionFilter.Service";
            var kernel = new StandardKernel();
            kernel.Load(assemblyName);
            kernel.Bind<ExceptionFilter>().To<ExceptionFilter>();

            return kernel;
        }
    }
}