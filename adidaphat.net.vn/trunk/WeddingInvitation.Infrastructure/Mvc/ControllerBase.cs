using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using WeddingInvitation.Services;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Infrastructure.Mvc
{
    public class ControllerBase<TRepository, TEntity> : Controller
        where TRepository : IRepository<TEntity>
        where TEntity : class
    {
        private const int HorizontalMargin = 40;
        private const int VerticalMargin = 40;
        protected readonly TRepository Repository;
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="unitOfWork"></param>
        public ControllerBase(TRepository repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated &&
                filterContext.HttpContext.Session != null &&
                filterContext.HttpContext.Session.IsNewSession)
            {
                var sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];
                if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    try
                    {
                        FormsAuthentication.SignOut();
                        Session.Clear();
                        Session.Abandon();
                    }
                    catch
                    {
                        
                    }
                    if (filterContext.HttpContext.Request.Url != null)
                    {
                        //Redirect to login page
                        var loginUrl = Url.Action("LogOn", "Account", new { area = "", ReturnUrl = filterContext.HttpContext.Request.RawUrl });
                        filterContext.HttpContext.Response.Redirect(loginUrl);
                        filterContext.Result = new HttpUnauthorizedResult();
                        return;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Set Success Notification
        /// </summary>
        /// <param name="message"></param>
        /// <param name="persistForTheNextRequest"></param>
        protected virtual void SetSuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            SetNotification(NotificationType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Set Error Notification
        /// </summary>
        /// <param name="message"></param>
        /// <param name="persistForTheNextRequest"></param>
        protected virtual void SetErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            SetNotification(NotificationType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Set Notification
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="persistForTheNextRequest"></param>
        protected virtual void SetNotification(NotificationType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format(WebViewPageExtensions.NOTIFICATION_KEY_FORMAT, type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                {
                    TempData[dataKey] = new List<string>();
                }
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                {
                    ViewData[dataKey] = new List<string>();
                }
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected ActionResult AccessDeniedView()
        {
            return new HttpUnauthorizedResult();
            //return RedirectToAction("AccessDenied","", new { pageUrl = this.Request.RawUrl });
        }

        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        private byte[] RenderToPdf(string htmlText, string pageTitle)
        {
            byte[] renderedBuffer;

            using (var outputMemoryStream = new MemoryStream())
            {
                using (var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                {
                    var pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                    pdfDocument.Open();
                    using (var htmlViewReader = new StringReader(htmlText))
                    {
                        using (var htmlWorker = new HTMLWorker(pdfDocument))
                        {
                            htmlWorker.Parse(htmlViewReader);
                        }
                    }
                }

                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }

        protected ActionResult PdfResult(string fileName, string viewName, object model)
        {
            // Render the view html to a string.
            var htmlText = RenderPartialViewToString(viewName, model);
            // Let the html be rendered into a PDF document through iTextSharp.
            var buffer = RenderToPdf(htmlText, "");

            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(buffer, "application/pdf", fileName);
        }

        protected ActionResult PdfResult(MemoryStream pdfData, string fileName)
        {
            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(pdfData.GetBuffer(), "application/pdf", fileName);
        }
    }
}
