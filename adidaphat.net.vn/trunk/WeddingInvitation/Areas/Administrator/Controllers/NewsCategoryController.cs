using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [SuperAdminAuthorize]
    public class NewsCategoryController : ControllerBase<INewsCategoryItemRepository, NewsCategoryItem>
    {
        public NewsCategoryController(IUnitOfWork unitOfWork, INewsCategoryItemRepository repository)
            : base(repository, unitOfWork)
        {
        }

        //
        // GET: /Administrator/NewsCategory/

        public ActionResult Index()
        {
            var listCategories = Repository.Search("");
            return View(listCategories);
        }

        public ActionResult Create()
        {
            var categories = new List<NewsCategoryItem>();
            foreach (var cat in Repository.GetAll().ToList())
            {
                categories.Add(new NewsCategoryItem
                {
                    CategoryName = GetCategoryBreadCrumb(cat),
                    NewsCategoryItemId = cat.NewsCategoryItemId
                });
            }
            ViewBag.AllCategories = categories;
            return View("Create", new NewsCategoryItem() { Active = true });
        }

        public ActionResult Edit(int id)
        {
            var categories = new List<NewsCategoryItem>();
            foreach (var cat in Repository.GetAll().ToList())
            {
                categories.Add(new NewsCategoryItem
                {
                    CategoryName = GetCategoryBreadCrumb(cat),
                    NewsCategoryItemId = cat.NewsCategoryItemId
                });
            }
            ViewBag.AllCategories = categories;

            var category = Repository.GetById(id);
            return View("Edit", category);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(NewsCategoryItem category)
        {
            if (category.NewsCategoryItemId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var categories = new List<NewsCategoryItem>();
                    foreach (var cat in Repository.GetAll().ToList())
                    {
                        categories.Add(new NewsCategoryItem
                        {
                            CategoryName = GetCategoryBreadCrumb(cat),
                            NewsCategoryItemId = cat.NewsCategoryItemId
                        });
                    }
                    ViewBag.AllCategories = categories;
                    return View("Create", category);
                }
                if (Repository.GetAll().Where(p => p.CategoryName == category.CategoryName).FirstOrDefault() != null)
                {
                    this.SetErrorNotification("Tên danh mục này đã tồn tại trong hệ thống.");
                    return RedirectToAction("index");
                }
                else
                {
                    if (category.ParentId != null)
                    {
                        category.Level = Repository.GetById(category.ParentId).Level + 1;
                    }
                    else
                    {
                        category.Level = 0;
                    }
                    using (UnitOfWork)
                    {
                        Repository.Insert(category);
                    }
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var categories = new List<NewsCategoryItem>();
                    foreach (var cat in Repository.GetAll().ToList())
                    {
                        categories.Add(new NewsCategoryItem
                        {
                            CategoryName = GetCategoryBreadCrumb(cat),
                            NewsCategoryItemId = cat.NewsCategoryItemId
                        });
                    }
                    ViewBag.AllCategories = categories;
                    return View("Edit", category);
                }
                var categoryEntity = Repository.GetById(category.NewsCategoryItemId);
                if (category.ParentId == null)
                {
                    categoryEntity.Level = 0;
                }
                else
                {
                    categoryEntity.Level = Repository.GetById(category.ParentId).Level + 1;
                }
                categoryEntity.CategoryName = category.CategoryName;
                categoryEntity.CategoryDescription = category.CategoryDescription;
                categoryEntity.Active = category.Active;
                categoryEntity.DisplayOrder = category.DisplayOrder;
                using (UnitOfWork)
                {
                    Repository.Update(categoryEntity);
                }
            }
            this.SetSuccessNotification("Danh mục đã được lưu thành công.");
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult TreeDrop(int item, int destinationitem, string position)
        {
            var categoryItem = Repository.GetById(item);
            var categoryDestinationItem = Repository.GetById(destinationitem);
            IList<NewsCategoryItem> nodes = null;
            using (UnitOfWork)
            {
                switch (position)
                {
                    case "over":
                        IList<NewsCategoryItem> children = null;
                        if (categoryDestinationItem == null)
                        {
                            children = Repository.GetAll().Where(p => p.Level == 0 || p.Parent == null).ToList();
                            categoryItem.Parent = null;
                            categoryItem.Level = 0;
                        }
                        else
                        {
                            children = Repository.GetChildren(categoryDestinationItem.NewsCategoryItemId).ToList();
                            categoryItem.ParentId = categoryDestinationItem.NewsCategoryItemId;
                            categoryItem.Level = categoryDestinationItem.Level + 1;
                        }
                        if (children.Count > 0)//children != null
                            categoryItem.DisplayOrder = children.Max(p => p.DisplayOrder) + 1;
                        break;
                    case "before":
                        if (categoryDestinationItem == null)
                        { }
                        else
                        {
                            categoryItem.ParentId = categoryDestinationItem.ParentId;

                            categoryItem.Level = categoryDestinationItem.Level;
                            categoryItem.DisplayOrder = categoryDestinationItem.DisplayOrder;
                            nodes = Repository.GetAll().Where(p => (p.ParentId == categoryItem.ParentId || (p.Level == 0 && categoryItem.Level == 0)) && p.DisplayOrder >= categoryDestinationItem.DisplayOrder).ToList();
                            if (nodes != null)
                            {
                                foreach (NewsCategoryItem node in nodes)
                                {
                                    if (node.NewsCategoryItemId != categoryItem.NewsCategoryItemId)
                                    {
                                        node.DisplayOrder += 1;
                                        Repository.Update(node);
                                    }
                                }
                            }
                        }
                        break;
                    case "after":
                        if (categoryDestinationItem == null)
                        { }
                        else
                        {
                            int desPosition = categoryDestinationItem.DisplayOrder;
                            categoryItem.ParentId = categoryDestinationItem.ParentId;

                            categoryItem.Level = categoryDestinationItem.Level;
                            nodes = Repository.GetAll().Where(p => (p.ParentId == categoryItem.ParentId || (p.Level == 0 && categoryItem.Level == 0)) && p.DisplayOrder <= categoryDestinationItem.DisplayOrder && p.DisplayOrder > categoryItem.DisplayOrder).ToList();
                            if (nodes != null)
                            {
                                foreach (NewsCategoryItem node in nodes)
                                {
                                    if (node.NewsCategoryItemId != categoryItem.NewsCategoryItemId)
                                    {
                                        node.DisplayOrder -= 1;
                                        Repository.Update(node);
                                    }
                                }
                            }
                            categoryItem.DisplayOrder = desPosition;
                        }
                        break;
                }
                Repository.Update(categoryItem);
            }
            return Json(new { success = true });
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

                category = Repository.GetById(category.ParentId);

            }
            return result;
        }

        private string GetListInActiveCategories(NewsCategoryItem category)
        {
            string listInActiveCategories = "";
            if (category != null)
            {
                if (category.Active == false || category.Level > 0)
                    listInActiveCategories += category.NewsCategoryItemId + ",";
                if (category.Children != null)
                {
                    foreach (var cat in category.Children)
                    {
                        listInActiveCategories += GetListInActiveCategories(cat);
                    }
                }
            }
            return listInActiveCategories;
        }

    }
}
