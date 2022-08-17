using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tourist_Place.Models.Entity;

namespace Tourist_Place.Controllers
{
    public class PlaceController : Controller
    {
        List<Place> places = new List<Place>()
        {
            new Place { PlaceID = 1, PlaceName = "Mohasthan Ghor", Address = "Comilla", Rating = 3, Type = 4, Picture = "Mohasthan Ghor Picture"},
            new Place { PlaceID = 2, PlaceName = "Lalbagh Fort", Address = "Lalbagh", Rating = 2, Type = 4, Picture = "Lalbagh Fort Picture"},
            new Place { PlaceID = 3, PlaceName = "Everest", Address = "Tibet, Nepal", Rating = 5, Type = 2, Picture = "Everest Picture"}
        };
        // GET: PlaceController
        public ActionResult Index()
        {
            return View(places);
        }

        // GET: PlaceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlaceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlaceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlaceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlaceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
