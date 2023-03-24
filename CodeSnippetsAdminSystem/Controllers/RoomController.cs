using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models;
using Microsoft.AspNetCore.Mvc;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]

    // Thomas

    public class RoomController : Controller
    {
        private IRepositoryCrud<Department> departmentRepository;
        private IRepositoryCrud<Room> roomRepository;

        public RoomController(IRepositoryCrud<Department> departmentRepository, IRepositoryCrud<Room> roomRepository)
        {
            this.departmentRepository = departmentRepository;
            this.roomRepository = roomRepository;
        }

        public ActionResult RoomList()
        {
            ViewBag.SelectedDepartment = "Vælg afdeling";
            ViewBag.Department = departmentRepository.Collection;
            ViewBag.Room = roomRepository.Collection;
            return View();

        }

        [HttpPost]
        public ActionResult RoomListByDepartment(int departmentid)
        {


            Department department = departmentRepository.GetByID(departmentid);

            ViewBag.SelectedDepartment = department.DepartmentName;
            ViewBag.Department = departmentRepository.Collection;

            ViewBag.Room = roomRepository.Collection.Where(r => r.Department == department);
            return View("RoomList");
        }

        public ActionResult CreateRoom()
        {
            ViewBag.Department = departmentRepository.Collection;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRoom(Room room)
        {
            if (room.Department != null)
                ModelState.Remove("Department.DepartmentName");

            if (ModelState.IsValid)
            {
                room.Department = departmentRepository.GetByID(room.Department.DepartmentID);

                roomRepository.Create(room);

                return RedirectToAction("RoomList");
            }

            ViewBag.Department = departmentRepository.Collection;
            return View();
        }

        public ActionResult DeleteRoom(int RoomID)
        {
            roomRepository.Delete(RoomID);
            return RedirectToAction("RoomList");
        }
    }
}
