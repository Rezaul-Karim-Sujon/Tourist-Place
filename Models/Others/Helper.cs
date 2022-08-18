using System;
using System.Collections.Generic;
using System.Linq;
using Tourist_Place.Models.Entity;

namespace Tourist_Place.Models.Others
{
    public static class Helper
    {
        public static string NoTilde(this string str)
        {
            return str.Replace("~", "");
        }

        internal static List<Place> SearchingAndSortingList(List<Place> places, string searchString,string sortOrder)
        {
            places = SearchingList(places, searchString);
            switch (sortOrder)
            {
                case "nameDesc":
                    places = places.
                        OrderByDescending(p => p.PlaceName).
                        ToList();
                    break;
                case "rating":
                    places = places.
                        OrderBy(p => p.Rating).
                        ToList();
                    break;
                case "ratingDesc":
                    places = places.
                        OrderByDescending(p => p.Rating).
                        ToList();
                    break;
                default:
                    places = places.
                        OrderBy(p => p.PlaceName).
                        ToList();
                    break;
            }
            return places;
        }

        internal static List<Place> SearchingList(List<Place> places, string searchString)
        {
            return String.IsNullOrEmpty(searchString) ? places
                : places.Where(p => p.PlaceName.ToUpper().Contains(searchString.ToUpper())).ToList();
        }
    }
}
