using System;
using System.Web;
using System.Web.Mvc;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    public class UploadController : Controller
    {
        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public string OrderDetailFile(HttpPostedFileBase fileData)
        {
            var myUniqueFileName = string.Format(@"{0}-{1}", Guid.NewGuid(), fileData.FileName);
            //Save image
            var fileName = this.Server.MapPath("~/Uploads/" + System.IO.Path.GetFileName(myUniqueFileName));
            fileData.SaveAs(fileName);
            return (Url.Content("~/Uploads/") + myUniqueFileName);
        }

    }
}
