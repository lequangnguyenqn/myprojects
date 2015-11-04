using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class StateProvinceController : ControllerBase<IStateProvinceRepository, StateProvince>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;

        public StateProvinceController(IUnitOfWork unitOfWork, IStateProvinceRepository repository,
                                       IMyOfficeRepository myOfficeRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
        }

        //
        // GET: /Administrator/StateProvince/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var model = Repository.Search(search).ToList();
            var listOffice = _myOfficeRepository.Search("").ToList();
            var gridModel = new GridModel<StateProvinceModel>
            {
                Data = model.Select(x =>
                {
                    var firstOrDefault = listOffice.FirstOrDefault(p => p.MyOfficeId == x.MyOfficeId);
                    return new StateProvinceModel
                    {
                        StateProvinceId = x.StateProvinceId,
                        StateProvinceCode = x.StateProvinceCode,
                        StateProvinceName = x.StateProvinceName,
                        RegionName = firstOrDefault != null ? firstOrDefault.OfficeName : string.Empty
                    };
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var listOffice = _myOfficeRepository.Search("").ToList();
            //var regions = _regionRepository.GetAll().ToList();
            var model = new StateProvinceModel()
            {
                //Regions = regions,
                Offices = listOffice
            };

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            //var regions = _regionRepository.GetAll().ToList();
            var entity = Repository.GetById(id);
            var listOffice = _myOfficeRepository.Search("").ToList();
            var model = new StateProvinceModel()
            {
                StateProvinceCode = entity.StateProvinceCode,
                StateProvinceId = entity.StateProvinceId,
                StateProvinceName = entity.StateProvinceName,
                //Regions = regions,
                //RegionId = entity.RegionId,
                Offices = listOffice,
                MyOfficeId = entity.MyOfficeId,
                IsMain = entity.IsMain
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(StateProvinceModel stateProvinceModel)
        {
            if (!ModelState.IsValid)
            {
                //var regions = _regionRepository.GetAll().ToList();
                //stateProvinceModel.Regions = regions;
                var listOffice = _myOfficeRepository.Search("").ToList();
                stateProvinceModel.Offices = listOffice;
                return View("Create", stateProvinceModel);
            }
            //Check existed
            var modelExisted = Repository.GetAll().FirstOrDefault(p => p.StateProvinceCode == stateProvinceModel.StateProvinceCode || p.StateProvinceName == stateProvinceModel.StateProvinceName);
            if ((modelExisted != null && modelExisted.StateProvinceId != stateProvinceModel.StateProvinceId) || (modelExisted != null && stateProvinceModel.StateProvinceId <= 0))
            {
                this.SetErrorNotification("Tỉnh thành này đã tồn tại trong hệ thống.");
                //var regions = _regionRepository.GetAll().ToList();
                //stateProvinceModel.Regions = regions;
                var listOffice = _myOfficeRepository.Search("").ToList();
                stateProvinceModel.Offices = listOffice;
                return View("Create", stateProvinceModel);
            }
            if (stateProvinceModel.StateProvinceId <= 0) //Create
            {
                var stateProvince = new StateProvince()
                {
                    IsDeleted = false,
                    StateProvinceCode = stateProvinceModel.StateProvinceCode,
                    StateProvinceName = stateProvinceModel.StateProvinceName,
                    MyOfficeId = stateProvinceModel.MyOfficeId
                    //RegionId = stateProvinceModel.RegionId
                };
                using (UnitOfWork)
                {
                    Repository.Insert(stateProvince);
                }
            }
            else //Edit
            {
                var stateProvince = Repository.GetById(stateProvinceModel.StateProvinceId);
                stateProvince.StateProvinceCode = stateProvinceModel.StateProvinceCode;
                stateProvince.StateProvinceName = stateProvinceModel.StateProvinceName;
                stateProvince.IsMain = stateProvinceModel.IsMain;
                stateProvince.MyOfficeId = stateProvinceModel.MyOfficeId;
                using (UnitOfWork)
                {
                    Repository.Update(stateProvince);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Tỉnh thành"));
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
                    entity.StateProvinceCode = entity.StateProvinceCode + "_Deleted";
                }
                this.SetSuccessNotification("Tỉnh thành đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Tỉnh thành này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
