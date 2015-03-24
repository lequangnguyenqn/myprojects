using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.Mvc;

namespace WeddingInvitation.Infrastructure.Constant
{
    public class CountryList
    {
        /// <summary>
        /// method for generating a country list, say for populating
        /// a ComboBox, with country options. We return the
        /// values in a Generic List<T>
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCountryList()
        {
            //create a new Generic list to hold the country names returned
            List<string> cultureList = new List<string>();

            //create an array of CultureInfo to hold all the cultures found, these include the users local cluture, and all the
            //cultures installed with the .Net Framework
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

            //loop through all the cultures found
            foreach (CultureInfo culture in cultures)
            {
                //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                //to the RegionInfo contructor to gain access to the information for that culture
                RegionInfo region = new RegionInfo(culture.LCID);

                //make sure out generic list doesnt already
                //contain this country
                if (!(cultureList.Contains(region.EnglishName)))
                    //not there so add the EnglishName (http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.englishname.aspx)
                    //value to our generic list
                    cultureList.Add(region.EnglishName);
            }
            return cultureList;
        }

        /// <summary>
        /// method for generating a country list, say for populating
        /// a ComboBox, with country options. We return the
        /// values in a Generic List<T>
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetCountryList(string selectedCountry)
        {
            //create a new Generic list to hold the country names returned
            List<SelectListItem> cultureList = new List<SelectListItem>();

            //create an array of CultureInfo to hold all the cultures found, these include the users local cluture, and all the
            //cultures installed with the .Net Framework
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            SelectListItem uk = new SelectListItem();
            SelectListItem us = new SelectListItem();
            //loop through all the cultures found
            foreach (CultureInfo culture in cultures)
            {
                //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                //to the RegionInfo contructor to gain access to the information for that culture
                RegionInfo region = new RegionInfo(culture.LCID);

                //make sure out generic list doesnt already
                //contain this country
                if (cultureList.Where(p => p.Text == region.EnglishName).FirstOrDefault() == null)
                    //not there so add the EnglishName (http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.englishname.aspx)
                    //value to our generic list
                    if (region.TwoLetterISORegionName.ToUpper() == "US")
                    {
                        us.Text = region.EnglishName;
                        us.Value = region.TwoLetterISORegionName;
                        us.Selected = selectedCountry == region.TwoLetterISORegionName;
                    }
                    else if (region.TwoLetterISORegionName.ToUpper() == "GB")
                    {
                        uk.Text = region.EnglishName;
                        uk.Value = region.TwoLetterISORegionName;
                        uk.Selected = selectedCountry == region.TwoLetterISORegionName;
                    }
                    else
                    {
                        cultureList.Add(new SelectListItem
                        {
                            Text = region.EnglishName,
                            Value = region.TwoLetterISORegionName,
                            Selected = selectedCountry == region.TwoLetterISORegionName
                        });
                    }
            }
            cultureList = cultureList.OrderBy(p => p.Text).ToList();
            cultureList.Insert(0,us);
            cultureList.Insert(0, uk);
            cultureList.Insert(0, new SelectListItem
            {
                Text = "Choose Country",
                Value = "",
                Selected = selectedCountry == "default"
            });
            return cultureList;
        }
    }
}
