using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.ContentManagement;
using WeddingInvitation.Services;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using System.Data.Entity;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [SuperAdminAuthorize]
    public class NewsController : ControllerBase<INewsItemRepository, NewsItem>
    {
        private readonly INewsCategoryItemRepository _newsCategoryItemRepository;
        public NewsController(IUnitOfWork unitOfWork, INewsItemRepository repository,
                              INewsCategoryItemRepository newsCategoryItemRepository)
            : base(repository, unitOfWork)
        {
            _newsCategoryItemRepository = newsCategoryItemRepository;
        }

        //
        // GET: /Administrator/News/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var user = Repository.Search(search).Include(p => p.Category);

            var gridModel = new GridModel<NewsItemModel>
            {
                Data = user.Select(x => new NewsItemModel
                                            {
                                                NewsItemId = x.NewsItemId,
                                                Title = x.Title,
                                                CreateDate = x.CreatedDate,
                                                NewsCategoryItemName = x.Category.CategoryName
                                            })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var categories = new List<NewsCategoryItem>();
            foreach (var cat in _newsCategoryItemRepository.GetAll().ToList())
            {
                categories.Add(new NewsCategoryItem
                {
                    CategoryName = GetCategoryBreadCrumb(cat),
                    NewsCategoryItemId = cat.NewsCategoryItemId
                });
            }
            var model = new NewsItemModel
                            {
                Categories = categories
            };

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var categories = new List<NewsCategoryItem>();
            foreach (var cat in _newsCategoryItemRepository.GetAll().ToList())
            {
                categories.Add(new NewsCategoryItem
                {
                    CategoryName = GetCategoryBreadCrumb(cat),
                    NewsCategoryItemId = cat.NewsCategoryItemId
                });
            }
            var newsItem = Repository.GetById(id);
            var model = new NewsItemModel
                            {
                Categories = categories,
                Full = HttpUtility.HtmlDecode(newsItem.Full),
                NewsCategoryItemId = newsItem.NewsCategoryItemId,
                NewsItemId = newsItem.NewsItemId,
                Short = newsItem.Short,
                Title = newsItem.Title

            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(NewsItemModel newsItemModel)
        {
            if (newsItemModel.NewsItemId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var categories = new List<NewsCategoryItem>();
                    foreach (var cat in _newsCategoryItemRepository.GetAll().ToList())
                    {
                        categories.Add(new NewsCategoryItem
                        {
                            CategoryName = GetCategoryBreadCrumb(cat),
                            NewsCategoryItemId = cat.NewsCategoryItemId
                        });
                    }
                    newsItemModel.Categories = categories;
                    return View("Create", newsItemModel);
                }
                var newsItem = new NewsItem
                                   {
                    CreatedDate = DateTime.Now,
                    Full = newsItemModel.Full,
                    IsDeleted = false,
                    NewsCategoryItemId = newsItemModel.NewsCategoryItemId,
                    Published = true,
                    Short = newsItemModel.Short,
                    Title = newsItemModel.Title
                };
                using (UnitOfWork)
                {
                    Repository.Insert(newsItem);
                }
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    var categories = new List<NewsCategoryItem>();
                    foreach (var cat in _newsCategoryItemRepository.GetAll().ToList())
                    {
                        categories.Add(new NewsCategoryItem
                        {
                            CategoryName = GetCategoryBreadCrumb(cat),
                            NewsCategoryItemId = cat.NewsCategoryItemId
                        });
                    }
                    newsItemModel.Categories = categories;
                    return View("Edit", newsItemModel);
                }

                var newsItem = Repository.GetById(newsItemModel.NewsItemId);
                newsItem.Title = newsItemModel.Title;
                newsItem.Short = newsItemModel.Short;
                newsItem.NewsCategoryItemId = newsItemModel.NewsCategoryItemId;
                newsItem.Full = newsItemModel.Full;
                using (UnitOfWork)
                {
                    Repository.Update(newsItem);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Bài viết"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    Repository.Delete(id);
                }
                this.SetSuccessNotification("Bài viết đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Bài viết này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        private string GetCategoryBreadCrumb(NewsCategoryItem category)
        {
            string result = string.Empty;

            while (category != null)
            {
                if (String.IsNullOrEmpty(result))
                    result = category.CategoryName;
                else
                    result = category.CategoryName + " >> " + result;

                category = _newsCategoryItemRepository.GetById(category.ParentId);

            }
            return result;
        }
    }
}
