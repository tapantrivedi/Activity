using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Activity.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Activity.Controllers
{
    public class ActivityController : Controller
    {     
        private UserContext dbContext;
        public ActivityController(UserContext context)
        {
            dbContext = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<User> allUsers = dbContext.UsersTable.ToList();
            ViewBag.AllUsers = allUsers;
            return View();
        }

        //Registration process
        [HttpPost]
        [Route("/registration")]
        public IActionResult RegistrationProcess(User user)
        {
            List<User> allUsers=dbContext.UsersTable.ToList();
            ViewBag.AllUsers = allUsers;

            if (ModelState.IsValid)
            {
                if(dbContext.UsersTable.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use. use other email!!");
                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);


                dbContext.Add(user);
                dbContext.SaveChanges();

                //setting user's firstname in session
                HttpContext.Session.SetString("firstName", user.FirstName);
                HttpContext.Session.SetString("lastName", user.LastName);
                HttpContext.Session.SetInt32("UserId", user.UserId);

                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        
        //Login process
        [HttpPost]
        [Route("/login")]
        public IActionResult LoginProcess(LoginUser userSubmission)
        {
            List<User> allUsers = dbContext.UsersTable.ToList();
            ViewBag.AllUsers = allUsers;

            if(ModelState.IsValid)
            {
                var userInDb = dbContext.UsersTable.SingleOrDefault(u => u.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email!! Hmmmm Looks like you do not have account with us, Got to Register and then try again!!");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);

                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid password. Really can't even Remember your Password?!!");
                    return View("Index");
                }
                HttpContext.Session.SetString("firstName", userInDb.FirstName);
                HttpContext.Session.SetString("lastName", userInDb.LastName);
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);

                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        //logout process
        [HttpGet]
        [Route("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

            //Registration/Login success process
        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");

            int? UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");

            List<ActivitySchedule> activitywithguest = dbContext.ActivityTable.Include( w => w.guest).ToList();

            return View(activitywithguest);
        }
        //render newWedding page
        [HttpGet]
        [Route("/newActivityPage")]
        public IActionResult NewActivityPageMethod()
        {
            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");


            int? UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");

            return View("NewActivityPage");
        }
        
        
        [HttpPost]
        [Route("/createActivityProcessRoute")]
        public IActionResult CreateActivityProcessMethod(ActivitySchedule schedule)
        {

            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");

            

            if(ModelState.IsValid)
            {
                if(schedule.Date <= DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Date must be set to the future date!!");
                    return View("NewActivityPage");
                }
                schedule.UserId = (int)HttpContext.Session.GetInt32("UserId");

                dbContext.Add(schedule);
                dbContext.SaveChanges();
             
            
                return RedirectToAction("Dashboard");
            }
            return View("NewActivityPage");
        }
        [HttpGet]
        [Route("/activityDetailPage/{showId}")]
        public IActionResult ActivityDetail(int showId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            ActivitySchedule showOne = dbContext.ActivityTable.Include(w => w.guest).ThenInclude(g => g.User).SingleOrDefault(h =>h.ActivityId == showId);
            ViewBag.ShowOne = showOne;

            return View(showOne);
        }


            [HttpGet]
        [Route("/deleteWed/{wedId}")]
        public IActionResult DeleteWedding(int wedId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ActivitySchedule CurrentWedding = dbContext.ActivityTable.SingleOrDefault( d => d.ActivityId == wedId);
            dbContext.ActivityTable.Remove(CurrentWedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
                [HttpGet]
        [Route("//rsvp/{wId}")]
        public IActionResult RSVP(int wId)
        {
            //check user in session
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            //get the session id 
            User currentUser = dbContext.UsersTable.SingleOrDefault( c => c.UserId == HttpContext.Session.GetInt32("UserId"));

            ActivitySchedule currWed = dbContext.ActivityTable.Include( w => w.guest).ThenInclude(g => g.User).SingleOrDefault(r => r.ActivityId == wId);
            Guest thisguest = dbContext.Guest.Where(j => j.ActivityId == wId && j.UserId == currentUser.UserId).SingleOrDefault();
            
            if(thisguest != null)
            {
                currentUser.guest.Remove(thisguest);
            }
            else{
                Guest newGuest = new Guest
                {
                    UserId = currentUser.UserId,
                    ActivityId = currWed.ActivityId,
                };
                currWed.guest.Add(newGuest);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
