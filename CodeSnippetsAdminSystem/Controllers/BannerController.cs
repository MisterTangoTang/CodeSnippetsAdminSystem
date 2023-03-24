using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models;
using Microsoft.AspNetCore.Mvc;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]

    // Thomas

    public class BannerController : Controller
    {
        private IBannerRepository bannerRepository;
        private IRepositoryCrud<Department> departmentRepository;

        public BannerController(IBannerRepository bannerRepository, IRepositoryCrud<Department> departmentRepository)
        {
            this.bannerRepository = bannerRepository;
            this.departmentRepository = departmentRepository;
        }

        public ActionResult InformationBanner()
        {
            ViewBag.Departments = departmentRepository.Collection;
            ViewBag.Banners = bannerRepository.Collection;
            ViewBag.BannersHerning = bannerRepository.Collection.Where(r => r.Department.DepartmentName == "Herning");
            ViewBag.BannersHolstebro = bannerRepository.Collection.Where(r => r.Department.DepartmentName == "Holstebro");

            return View();
        }

        [HttpPost]
        public ActionResult InformationBanner(Banner banner)
        {
            ModelState.Remove("Department.DepartmentName");
            if (ModelState.IsValid)
            {
                banner.Department = departmentRepository.GetByID(banner.Department.DepartmentID);

                bannerRepository.Create(banner);
            }
            ViewBag.Departments = departmentRepository.Collection;
            return RedirectToAction("InformationBanner");
        }

        [HttpPost]
        public ActionResult DeleteBanner(int bannerID)
        {
            bannerRepository.Delete(bannerID);
            return RedirectToAction("InformationBanner");
        }

        [HttpPost]
        public ActionResult ChangeActiveBanner(int bannerID)
        {
            bannerRepository.ChangeIsActive(bannerID);
            return RedirectToAction("InformationBanner");
        }
    }
}
