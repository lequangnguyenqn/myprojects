using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ShippingServiceController : ControllerBase<IShippingServiceRepository, ShippingService>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;

        public ShippingServiceController(IUnitOfWork unitOfWork, IShippingServiceRepository repository,
            IMyOfficeRepository myOfficeRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index(int? mode)
        {
            return View(mode);
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var model = Repository.Search(search);

            var gridModel = new GridModel<ShippingServiceModel>
            {
                Data = model.Select(x => new ShippingServiceModel
                {
                    ShippingServiceId= x.ShippingServiceId,
                    ShippingServiceName = x.ShippingServiceName,
                    PhoneNumber = x.PhoneNumber,
                    CoachStation = x.CoachStation,
                    StartAt = x.StartAt
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var model = new ShippingServiceModel { MyOffices = offices};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new ShippingServiceModel()
            {
                ShippingServiceId = entity.ShippingServiceId,
                PhoneNumber = entity.PhoneNumber,
                ShippingServiceName = entity.ShippingServiceName,
                Address = entity.Address,
                MyOfficeId = entity.MyOfficeId,
                MyOffices= offices,
                CoachStation = entity.CoachStation,
                StartAt = entity.StartAt
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ShippingServiceModel myOfficeModel)
        {
            if (myOfficeModel.ShippingServiceId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").ToList();
                    myOfficeModel.MyOffices = offices;
                    return View("Create", myOfficeModel);
                }
                var myOffice = new ShippingService()
                {
                    IsDeleted = false,
                    PhoneNumber = myOfficeModel.PhoneNumber,
                    ShippingServiceName = myOfficeModel.ShippingServiceName,
                    MyOfficeId = myOfficeModel.MyOfficeId,
                    StartAt = myOfficeModel.StartAt,
                    CoachStation = myOfficeModel.CoachStation
                };
                using (UnitOfWork)
                {
                    Repository.Insert(myOffice);
                }
                //Save success
                this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Chành xe"));
                return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").ToList();
                    myOfficeModel.MyOffices = offices;
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.ShippingServiceId);
                myOffice.PhoneNumber = myOfficeModel.PhoneNumber;
                myOffice.ShippingServiceName = myOfficeModel.ShippingServiceName;
                myOffice.MyOfficeId = myOfficeModel.MyOfficeId;
                myOffice.StartAt = myOfficeModel.StartAt;
                myOffice.CoachStation = myOfficeModel.CoachStation;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Chành xe"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    entity.IsDeleted = true;
                }
                this.SetSuccessNotification("Chành xe đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Chành xe này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
