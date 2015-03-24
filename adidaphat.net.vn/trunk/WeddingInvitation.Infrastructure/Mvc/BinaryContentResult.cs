// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryContentResult.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net pkalkie@gmail.com
// </copyright>
// <summary>
//   An ActionResult used to send binary data to the browser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace WeddingInvitation.Infrastructure.Mvc
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// An ActionResult used to send binary data to the browser.
    /// </summary>
    public class BinaryContentResult : ActionResult
    {
        private readonly string _contentType;
        private readonly byte[] _contentBytes;
        private readonly string _fileName;

        public BinaryContentResult(byte[] contentBytes, string contentType, string fileName)
        {
            _contentBytes = contentBytes;
            _contentType = contentType;
            _fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = _contentType;
            response.AppendHeader("Content-Disposition", "attachment;filename=" + _fileName + ".pdf"); 

            using (var stream = new MemoryStream(_contentBytes))
            {
                stream.WriteTo(response.OutputStream);
                stream.Flush();
            }
        }
    }
}