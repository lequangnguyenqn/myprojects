using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace WeddingInvitation.Infrastructure.Unity
{
    public sealed class HttpContextLifetimeManager : LifetimeManager, IDisposable
    {
        public override object GetValue()
        {
            return HttpContext.Current.Items[this];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(this);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[this] = newValue;
        }

        public void Dispose()
        {
            RemoveValue();
        }

    }
}
