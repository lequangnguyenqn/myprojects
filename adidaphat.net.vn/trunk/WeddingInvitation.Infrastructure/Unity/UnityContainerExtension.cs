using Microsoft.Practices.Unity;

namespace WeddingInvitation.Infrastructure.Unity
{
    public static class UnityContainerExtension
    {
        public static IUnityContainer RegisterTypeForHttpContext<T, TX>(this IUnityContainer container) where TX : T
        {
            return container.RegisterType<T, TX>(new HttpContextLifetimeManager());
        }
    }
}
