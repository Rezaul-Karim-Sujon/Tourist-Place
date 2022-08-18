using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Tourist_Place.Models.Entity;
using Tourist_Place.Models.Others;
using Tourist_Place.Models.View_Model;

namespace Tourist_Place.Controllers
{
    public class PlaceController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        public PlaceController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public static List<Place> places = new List<Place>()
        {
            new Place { PlaceName = "Mohasthan Ghor", Address = "Comilla", Rating = 3, Type = 4, Picture = "~/img/Mahasthan Ghor.jpg"},
            new Place { PlaceName = "Lalbagh Fort", Address = "Lalbagh", Rating = 2, Type = 4, Picture = "~/img/LalbaghFort.jpg"},
            new Place { PlaceName = "Everest", Address = "Tibet, Nepal", Rating = 5, Type = 2, Picture = "~/img/Everest.jpg"}
        };
        public static List<PlaceType> placeTypes = new List<PlaceType>()
        {
            new PlaceType { PlaceTypeID = 1, PlaceTypeName = "Beach" },
            new PlaceType { PlaceTypeID = 2, PlaceTypeName = "Hills" },
            new PlaceType { PlaceTypeID = 3, PlaceTypeName = "Fountain" },
            new PlaceType { PlaceTypeID = 4, PlaceTypeName = "Landmark" }
        };
        // GET: PlaceController
        public ActionResult Index()
        {
            return View(places);
        }
        // GET: PlaceController/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null || id >= places.Count)
            {
                return NotFound();
            }
            var place = places[(int)id];
            var index = placeTypes.FindIndex(s => s.PlaceTypeID == place.Type);
            ViewBag.PlaceType = placeTypes[index].PlaceTypeName;
            ViewData["PlaceID"] = id;
            return View(place);
        }

        // GET: PlaceController/Create
        public ActionResult Create()
        {
            PopulatePlaceTypesDropDownList();
            return View();
        }

        // POST: PlaceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]VMPlaces filter)
        {
            if(ModelState.IsValid)
            {
                var PicturePath = UploadFileControl.FileName(filter.Picture,
                                                            string.Concat(_webHostEnvironment.WebRootPath,
                                                            Location.ImagePath.NoTilde()));
                var place = new Place
                {
                    PlaceName = filter.PlaceName,
                    Address = filter.Address,
                    Rating = filter.Rating,
                    Type = filter.Type,
                    Picture = Location.ImagePath+PicturePath
                };
                places.Add(place);
                return RedirectToAction(nameof(Index));
            }
            PopulatePlaceTypesDropDownList(filter.Type);
            return View(filter);
        }

        // GET: PlaceController/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null || id >= places.Count)
            {
                return NotFound();
            }
            var place = places[(int)id];
            PopulatePlaceTypesDropDownList(place.Type);
            ViewData["PlaceID"] = id;
            return View(place);
        }

        // POST: PlaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [FromForm] VMPlaces filter)
        {
            if(id == null || id >= places.Count)
            {
                return NotFound();
            }
            try
            {
                var index = (int)id;
                if (ModelState.IsValid)
                {
                    var PicturePath = UploadFileControl.FileName(filter.Picture,
                                                            string.Concat(_webHostEnvironment.WebRootPath,
                                                            Location.ImagePath.NoTilde()));
                    places[index].PlaceName = filter.PlaceName;
                    places[index].Address = filter.Address;
                    places[index].Type = filter.Type;
                    places[index].Rating = filter.Rating;
                    if(filter.Picture != null ) places[index].Picture = Location.ImagePath + PicturePath;
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                PopulatePlaceTypesDropDownList(filter.Type);
                return View();
            }
        }

        // GET: PlaceController/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id==null || id >= places.Count)
            {
                return NotFound();
            }
            var place = places[(int)id];
            var index = placeTypes.FindIndex(s => s.PlaceTypeID == place.Type);
            ViewBag.PlaceType = placeTypes[index].PlaceTypeName;
            ViewData["PlaceID"] = id;
            return View(place);
        }

        // POST: PlaceController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if(id == null || id >= places.Count)
            {
                return NotFound();
            }
            try
            {
                places.RemoveAt((int)id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var place = places[(int)id];
                var index = placeTypes.FindIndex(s => s.PlaceTypeID == place.Type);
                ViewBag.PlaceType = placeTypes[index].PlaceTypeName;
                ViewData["PlaceID"] = id;
                return View(place);
            }
        }

        private void PopulatePlaceTypesDropDownList(object selectedType = null)
        {
            ViewBag.PlaceTypes = new SelectList(placeTypes,
                                                "PlaceTypeID",
                                                "PlaceTypeName",
                                                selectedType);
        }
    }
}

