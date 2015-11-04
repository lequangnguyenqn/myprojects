using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services;
using WeddingInvitation.Services.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Controllers
{
    public class NewsController : ControllerBase<INewsItemRepository, NewsItem>
    {
        public NewsController(IUnitOfWork unitOfWork, INewsItemRepository repository)
            : base(repository, unitOfWork)
        {
        }
        //
        // GET: /News/

        public ActionResult Index(int id)
        {
            var newsItem = Repository.GetById(id);
            newsItem.Full = HttpUtility.HtmlDecode(newsItem.Full);
            return View(newsItem);
        }

        public ActionResult Category(int id)
        {
            var newsItems = Repository.GetAll().Where(p => p.NewsCategoryItemId == id).OrderByDescending(p => p.NewsItemId).Take(25).ToList();
            return View(newsItems);
        }

    }
}
