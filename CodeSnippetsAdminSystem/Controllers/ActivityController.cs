using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace AdministrationSystem.Eamv.Controllers
{
    [Authorize]

    // Thomas

    public class ActivityController : Controller
    {
        private IRepositoryCrud<Department> departmentRepository;
        private IRepositoryCrud<Room> roomRepository;
        private IActiveRepository activityRepository;
        private IFeedbackRepository feedbackRepository;
        private IUserRepository userRepository;
        private IBannerRepository bannerRepository;

        public ActivityController(IRepositoryCrud<Department> departmentRepository, IRepositoryCrud<Room> roomRepository, IFeedbackRepository feedbackRepository, IUserRepository userRepository, IActiveRepository activityRepository, IBannerRepository bannerRepository)
        {
            this.departmentRepository = departmentRepository;
            this.roomRepository = roomRepository;
            this.feedbackRepository = feedbackRepository;
            this.userRepository = userRepository;
            this.activityRepository = activityRepository;
            this.bannerRepository = bannerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        // Thomas

        public ActionResult LoadRooms(int departmentId)
        {
            return Json(roomRepository.Collection.Where(r => r.Department.DepartmentID ==
            departmentId).Select(s => new
            {
                Id = s.RoomID,
                Name = s.RoomName
            }));
        }


        [HttpPost]
        public ActionResult SubmitFeedback(string feedbacktext, string returnUrl)
        {
            Feedback feedback = new Feedback();
            feedback.FeedbackDisc = "Sendt fra page: " + returnUrl + " === FeedbackTekst: " + feedbacktext;

            if (ModelState.IsValid)
            {
                feedbackRepository.Create(feedback);
                return Redirect(returnUrl);
            }

            return View("Index");
        }


        public ActionResult CreateActivity()
        {
            foreach (var item in roomRepository.Collection.Where(r => r.RoomName == "Ingen"))
            {
                ViewBag.noRooms = item.RoomID;
            }

            LoadRooms(departmentRepository.Collection.FirstOrDefault().DepartmentID);
            ViewBag.Departments = departmentRepository.Collection;
            return View();
        }

        //Hjælpmetode til at returnere en liste af rooms ud fra en string af roomID'er
        public List<ActivityRoom> GetRoomsFromIDList(string roomids)
        {
            List<ActivityRoom> rooms = new List<ActivityRoom>();

            string[] roomidlist = roomids.Split(",");

            foreach (string RoomID in roomidlist)
            {
                ActivityRoom room = new ActivityRoom();
                room.room = roomRepository.GetByID(int.Parse(RoomID));
                rooms.Add(room);
            }
            return rooms;
        }

        [HttpPost]
        public ActionResult CreateActivity(Activity activity, string moredates, string roomids, string returnURL)
        {
            List<Activity> activities = new List<Activity>();

            /* Checks if Endtime is before Starttime - already validates on clientside, this is an extra catch in case of something goes wrong
            with the clientside validation */
            if (activity.StartTime != null && activity.EndTime != null)
                if (TimeOnly.Parse(activity.StartTime) > TimeOnly.Parse(activity.EndTime))
                {
                    ViewBag.Departments = departmentRepository.Collection;

                    // Must return whole object if it comes from the Edit-page, in this case activityid is != 0
                    return activity.Activityid == 0 ? View() : View(activity);
                }

            if (activity.date != default(DateTime) && roomids != null && activity.Department != null)
            {
                ModelState.Remove("moredates");

                activity.Rooms = GetRoomsFromIDList(roomids);

                activity.Department = departmentRepository.GetByID(activity.Department.DepartmentID);

                activities.Add(activity);
            }
            else if (moredates != null && roomids != null && activity.Department != null)
            {
                ModelState.Remove("date");

                activity.Rooms = GetRoomsFromIDList(roomids);

                activity.Department = departmentRepository.GetByID(activity.Department.DepartmentID);

                string[] dates = moredates.Split(",");

                foreach (string d in dates)
                {
                    Activity a = new Activity();
                    a = Activity.Copy(activity);
                    a.date = DateTime.Parse(d.Trim());
                    activities.Add(a);
                }
            }

            // Removes modelstate check on the following properties as they are not relavent.
            if (activity.Rooms != null)
            {
                ModelState.Remove("Rooms");
                ModelState.Remove("Room.Department");
                ModelState.Remove("Room.RoomName");
            } else
            {
                ViewBag.Departments = departmentRepository.Collection;
                // Must return whole object if it comes from the Edit-page
                return activity.Activityid == 0 ? View() : View(activity);
            }

            // Not relavent for modelstate to check these.
            ModelState.Remove("Department.DepartmentName");
            ModelState.Remove("returnURL");

            if (ModelState.IsValid)
            {
                if (activity.Activityid == 0)
                    foreach (Activity a in activities)
                    {
                        activityRepository.Create(a);
                    }
                else // If activity ID != 0
                {
                    activityRepository.Update(activity);
                    if (returnURL != null)
                    {
                        return RedirectToAction("SearchActivity", new RouteValueDictionary(
                            new
                            {
                                controller = "Activity",
                                action = "SearchActivity",
                                returnURL = returnURL
                            }));

                    }
                    else
                        return RedirectToAction("SearchActivity");
                }

            return RedirectToAction("SearchActivity", "Activity");

            }
            else // Safe catch, ModelState should never be invalid, as validation happens on clientside.
            {
                ViewBag.Departments = departmentRepository.Collection;
                if (activity.date != null)
                    ViewBag.ReturnDate = activity.date.ToString("yyyy-MM-dd");

                // Must return whole object if it comes from the Edit-page
                if (activity.Activityid == 0)
                    return View();
                else
                    return View(activity);
            }
        }

        public ActionResult DeleteActivity(int ActivityID, string returnURL)
        {
            if (returnURL == null)
                ModelState.Remove("returnURL");

            if (ModelState.IsValid)
            {
                activityRepository.Delete(ActivityID);
                return RedirectToAction("SearchActivity", new RouteValueDictionary(
                    new
                    {
                        controller = "Activity",
                        action = "SearchActivity",
                        returnURL = returnURL
                    }));
            }

            return RedirectToAction("SearchActivity");
        }

        public ActionResult EditActivity(int ActivityID, string returnURL)
        {

            if (ActivityID == 0)
                return RedirectToAction("SearchActivity");

            foreach (var item in activityRepository.Collection.Include(a => a.Rooms).ThenInclude(r => r.room).Where(a => a.Activityid == ActivityID))
            {
                foreach (var room in item.Rooms)
                {
                    var last = item.Rooms.Last();

                    ViewBag.SelectedRooms += (room.room.RoomID) + "";
                    if (last == item.Rooms.Last())
                        ViewBag.SelectedRooms += ",";
                }
            }

            foreach (var item in roomRepository.Collection.Where(r => r.RoomName == "Ingen"))
            {
                ViewBag.noRooms = item.RoomID;
            }

            ModelState.Remove("returnURL");

            ViewBag.ReturnUrl = returnURL;

            if (ModelState.IsValid)
            {
                Activity activity = activityRepository.Collection // finds the correct activity in the database.
                    .Include(a => a.Rooms).ThenInclude(a => a.room)
                    .Include(a => a.Department)
                    .FirstOrDefault(a => a.Activityid == ActivityID);

                LoadRooms(activity.Department.DepartmentID);
                ViewBag.Departments = departmentRepository.Collection;
                return View("CreateActivity", activity);

            }
            return RedirectToAction("SearchActivity");
        }

        public ActionResult CancelActivity(int ActivityID, string status, string returnURL)
        {
            if (returnURL == null)
                ModelState.Remove("returnURL");

            if (ModelState.IsValid)
            {
                if (ActivityID != null && status != null)
                    if (status.Equals("true") || status.Equals("false"))
                    {
                        activityRepository.ChangeActivityStatus(ActivityID, bool.Parse(status));

                        if (returnURL != null)
                            if (returnURL.Split('/')[0].Equals("Preview"))
                                    return RedirectToAction("PreviewScreenPopUp", new RouteValueDictionary(
                                    new
                                    {
                                        controller = "Preview",
                                        action = "PreviewScreenPopUp",
                                        DepartmentID = returnURL.Split('&')[0].Split('=')[1],
                                        SelectedDate = returnURL.Split('&')[1].Split('=')[1]
                                    }));

                        return RedirectToAction("SearchActivity", new RouteValueDictionary(
                            new
                            {
                                controller = "Activity",
                                action = "SearchActivity",
                                returnURL = returnURL
                            }));
                    }
            }

            ViewBag.ErrorOnBool = "Der opstod en fejl, kontakt udvikleren hvis fejlen fortsætter";
            ViewBag.Departments = departmentRepository.Collection;
            return View("SearchActivity");
        }

        public ActionResult SearchActivity(string returnURL)
        {
            ViewBag.Departments = departmentRepository.Collection;

            Console.WriteLine("ReturnURL: " + returnURL);

            if (returnURL != null)
            {
                returnURL.Replace("#", " ");

                string[] returnValues = returnURL.Split("&");

                int DepartmentID = int.Parse(returnValues[0].Split("=")[1]);
                string SelectedDate = "";
                string ByWhom = "";
                string Title = "";

                if (returnValues.Length == 2 && returnValues[1].Split("=")[0].Equals("SelectedDate"))
                {
                    SelectedDate = returnValues[1].Split("=")[1];
                }
                else if (returnValues.Length == 2 && returnValues[1].Split("=")[0].Equals("ByWhom"))
                {
                    ByWhom = returnValues[1].Split("=")[1];
                }
                else if (returnValues.Length == 2 && returnValues[1].Split("=")[0].Equals("Title"))
                {
                    Title = returnValues[1].Split("=")[1];
                }
                else if (returnValues.Length == 3 && returnValues[1].Split("=")[0].Equals("SelectedDate") && returnValues[2].Split("=")[0].Equals("Title")) 
                {
                    SelectedDate = returnValues[1].Split("=")[1];
                    Title = returnValues[2].Split("=")[1];
                } 
                else if (returnValues.Length == 3 && returnValues[1].Split("=")[0].Equals("ByWhom") && returnValues[2].Split("=")[0].Equals("Title"))
                {
                    ByWhom = returnValues[1].Split("=")[1];
                    Title = returnValues[2].Split("=")[1];
                } 
                else if (returnValues.Length == 3 && returnValues[1].Split("=")[0].Equals("SelectedDate") && returnValues[2].Split("=")[0].Equals("ByWhom"))
                {
                    SelectedDate = returnValues[1].Split("=")[1];
                    ByWhom = returnValues[2].Split("=")[1];
                }
                else if (returnValues.Length == 4)
                {
                    SelectedDate = returnValues[1].Split("=")[1];
                    ByWhom = returnValues[2].Split("=")[1];
                    Title = returnValues[3].Split("=")[1];
                }


                ViewBag.DepartmentID = DepartmentID;
                try // Avoids error in case the user makes department input directly in the URL
                {
                    ViewBag.SelectedDepartment = departmentRepository.GetByID(DepartmentID).DepartmentName;
                }
                catch (Exception)
                {
                    ViewBag.ErrorOnDepartment = "Den indtastede afdeling findes ikke!";
                }

                ViewBag.SelectedDate = SelectedDate;
                ViewBag.ByWhom = ByWhom;
                ViewBag.Title = Title;
                ViewBag.returnURL = returnURL;

                return View(activityRepository.Collection
                    .Include(a => a.Department)
                    .Include(a => a.Rooms).ThenInclude(r => r.room)
                    .Where(a => a.Department.DepartmentID == DepartmentID &&
                    a.ByWhom.Contains(ByWhom == null ? "" : ByWhom) &&
                    a.Title.Contains(Title == null ? "" : Title) &&
                    (SelectedDate != "" ? a.date == DateTime.Parse(SelectedDate) : a.date >= DateTime.Today)));
            }

            // Page-load without returnURL information
            ViewBag.SelectedDepartment = "Vælg afdeling";
            return View(activityRepository.Collection
                .Include(a => a.Department)
                .Include(a => a.Rooms).ThenInclude(r => r.room)
                .Where(a => a.date >= DateTime.Today));
        }

        [HttpPost]
        public ActionResult SearchActivity(int DepartmentID, string SelectedDate, string ByWhom, string Title)
        {
            ViewBag.returnURL = "DepartmentID="+DepartmentID + (SelectedDate == null ? "" : "&SelectedDate="+SelectedDate) + (ByWhom == null ? "" : "&ByWhom="+ByWhom) + (Title == null ? "" : "&Title="+Title);

            ViewBag.Departments = departmentRepository.Collection;
            ViewBag.DepartmentID = DepartmentID;
            ViewBag.SelectedDepartment = DepartmentID == 0 ? "" : departmentRepository.GetByID(DepartmentID).DepartmentName;
            ViewBag.SelectedDate = SelectedDate;
            ViewBag.ByWhom = ByWhom;
            ViewBag.Title = Title;

            return View(activityRepository.Collection
                .Include(a => a.Department)
                .Include(a => a.Rooms).ThenInclude(r => r.room)
                .Where(a => a.Department.DepartmentID == DepartmentID && 
                a.ByWhom.Contains(ByWhom == null ? "" : ByWhom) && 
                a.Title.Contains(Title == null ? "" : Title) && 
                (SelectedDate != null ? a.date == DateTime.Parse(SelectedDate) : a.date >= DateTime.Today)));
        }
    }

}
