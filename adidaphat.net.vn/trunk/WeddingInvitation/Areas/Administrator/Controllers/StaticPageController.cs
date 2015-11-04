using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [SuperAdminAuthorize]
    public class StaticPageController : ControllerBase<IStaticPageRepository, StaticPage>
    {
        public StaticPageController(IUnitOfWork unitOfWork, IStaticPageRepository repository)
            : base(repository, unitOfWork)
        {
        }

        //
        // GET: /Administrator/StaticPage/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var user = Repository.Search(search);

            var gridModel = new GridModel<StaticPageModel>
            {
                Data = user.Select(x => new StaticPageModel
                                            {
                                                StaticPageId = x.StaticPageId,
                                                Description = x.Description
                                            })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public virtual ActionResult Edit(int id)
        {
            var entity = Repository.GetById(id);
            var model = new StaticPageModel()
            {
                Content = HttpUtility.HtmlDecode(entity.Content),
                Description = entity.Description,
                StaticPageId = entity.StaticPageId

            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(StaticPageModel staticPageModel)
        {
            if (staticPageModel.StaticPageId > 0)
            {
                if (!ModelState.IsValid)
                {
                    var entity = Repository.GetById(staticPageModel.StaticPageId);
                    var model = new StaticPageModel()
                    {
                        Content = HttpUtility.HtmlDecode(entity.Content),
                        Description = entity.Description,
                        StaticPageId = entity.StaticPageId

                    };
                    return View("Edit", model);
                }

                var staticPageEntity = Repository.GetById(staticPageModel.StaticPageId);
                staticPageEntity.Content = staticPageModel.Content;
                staticPageEntity.Description = staticPageModel.Description;
                using (UnitOfWork)
                {
                    Repository.Update(staticPageEntity);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Bài viết"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }
    }
}
