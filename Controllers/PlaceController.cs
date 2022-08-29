using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private static List<PlaceType> placeTypes = new List<PlaceType>()
        {
            new PlaceType { PlaceTypeID = 1, PlaceTypeName = "Beach" },
            new PlaceType { PlaceTypeID = 2, PlaceTypeName = "Hills" },
            new PlaceType { PlaceTypeID = 3, PlaceTypeName = "Fountain" },
            new PlaceType { PlaceTypeID = 4, PlaceTypeName = "Landmark" }
        };

        // GET: PlaceController
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nameDesc" 
                                                                       : "";
            ViewData["RatingSortParm"] = sortOrder == "rating" ? "ratingDesc" 
                                                               : "rating";
            ViewData["CurrentFilter"] = searchString;
            var selectedPlaces = Helper.SearchingAndSortingList(places, searchString, sortOrder);
            return View(selectedPlaces);
        }
        // GET: PlaceController/Details/5
        public ActionResult Details(string placeName)
        {
            if (string.IsNullOrEmpty(placeName) || !PlaceNameExists(places, placeName))
            {
                return NotFound();
            }
            var place = places.FirstOrDefault(p => p.PlaceName == placeName);
            var index = placeTypes.FindIndex(s => s.PlaceTypeID == place.Type);
            ViewBag.PlaceType = placeTypes[index].PlaceTypeName;
            ViewData["PlaceName"] = placeName;
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
            if (PlaceNameExists(places, filter.PlaceName))
            {
                ModelState.AddModelError("PlaceName", "The place name already exists");
            }
            if (ModelState.IsValid)
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
            return View();
        }


        // GET: PlaceController/Edit/5
        public ActionResult Edit(string placeName)
        {
            if (string.IsNullOrEmpty(placeName) || !PlaceNameExists(places, placeName))
            {
                return NotFound();
            }
            var place = places.FirstOrDefault(p => p.PlaceName == placeName);
            PopulatePlaceTypesDropDownList(place.Type);
            ViewData["PrevPlaceName"] = placeName;
            //ViewBag.Place = placeName;
            return View(place);
        }

        // POST: PlaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string prevPlaceName, [FromForm] VMPlaces filter)
        {
            if (string.IsNullOrEmpty(prevPlaceName) || !PlaceNameExists(places, prevPlaceName))
            {
                return NotFound();
            }
           
                var index = places.FindIndex(p => p.PlaceName == prevPlaceName);
                if (PlaceNameExists(places, filter.PlaceName) && prevPlaceName != filter.PlaceName)
                {
                    ModelState.AddModelError("PlaceName", "The place name already exists");
                }
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
                    return RedirectToAction(nameof(Index));
                }
                PopulatePlaceTypesDropDownList(filter.Type);
                ViewData["PrevPlaceName"] = prevPlaceName;
                return View();
            
        }

        // GET: PlaceController/Delete/5
        public ActionResult Delete(string placeName)
        {
            if (string.IsNullOrEmpty(placeName) || !PlaceNameExists(places, placeName))
            {
                return NotFound();
            }
            var place = places.FirstOrDefault(p => p.PlaceName == placeName);
            var index = placeTypes.FindIndex(s => s.PlaceTypeID == place.Type);
            ViewBag.PlaceType = placeTypes[index].PlaceTypeName;
            ViewData["PlaceName"] = placeName;
            return View(place);
        }

        // POST: PlaceController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string placeName)
        {
            if (string.IsNullOrEmpty(placeName) || !PlaceNameExists(places, placeName))
            {
                return NotFound();
            }

            try
            {
                var id = places.FindIndex(p => p.PlaceName == placeName);
                places.RemoveAt((int)id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var place = places.FirstOrDefault(p => p.PlaceName == placeName);
                var index = placeTypes.FindIndex(s => s.PlaceTypeID == place.Type);
                ViewBag.PlaceType = placeTypes[index].PlaceTypeName;
                ViewData["PlaceName"] = placeName;
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

        private bool PlaceNameExists(List<Place> places, string placeName)
        {
           return places.Any(s => s.PlaceName.ToLower() == placeName.ToLower());
        }
    }
}

