using System.Collections.Generic;
using System.Web.Mvc;

namespace WeddingInvitation.Infrastructure.Mvc
{
    public static class WebViewPageExtensions
    {
        public const string NOTIFICATION_KEY_FORMAT = "NotificationKey_{0}";

        /// <summary>
        /// Get Success Notification
        /// </summary>
        /// <param name="page"></param>
        /// <param name="persistForTheNextRequest"></param>
        /// <returns></returns>
        public static List<string> GetSuccessNotification(this WebViewPage page, bool persistForTheNextRequest = true)
        {
            return page.GetNotification(NotificationType.Success, persistForTheNextRequest);
        }

        /// <summary>
        /// Get Error Notification
        /// </summary>
        /// <param name="page"></param>
        /// <param name="persistForTheNextRequest"></param>
        /// <returns></returns>
        public static List<string> GetErrorNotification(this WebViewPage page, bool persistForTheNextRequest = true)
        {
            return page.GetNotification(NotificationType.Error, persistForTheNextRequest);
        }

        /// <summary>
        /// Get Notification
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <param name="persistForTheNextRequest"></param>
        /// <returns></returns>
        public static List<string> GetNotification(this WebViewPage page, NotificationType type, bool persistForTheNextRequest)
        {
            string dataKey = string.Format(NOTIFICATION_KEY_FORMAT, type);
            if (persistForTheNextRequest == true)
            {
                if (page.TempData[dataKey] == null)
                {
                    page.TempData[dataKey] = new List<string>();
                }
                return (List<string>)page.TempData[dataKey];
            }
            else
            {
                if (page.ViewData[dataKey] == null)
                {
                    page.ViewData[dataKey] = new List<string>();
                }
                return (List<string>)page.ViewData[dataKey];
            }
        }
    }
}
