using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace dotnetCore_SearchEngine.Models
{
    public class SearchData
    {
        [Required(ErrorMessage = "Search engine can not be empty.")]
        [RegularExpression(@"^(?:http[s]?[:](\/)\1)?(?:[w]{3}\.([\w\d]+)\.)(?:[a-z]{2,3})(?:(?:\1)|(?:\.[a-z]{2}))?$",ErrorMessage = "Unrecognised url. Please provide a valid url.")]
        public string SearchEngine { get; set; }
        [Required(ErrorMessage = "Please provide keywords.")]
        [StringLength(int.MaxValue,MinimumLength = 1,ErrorMessage = "Didn't meet the minimum number of characters")]
        public string SearchKeywords { get; set; }
        [Required(ErrorMessage = "Please provide a website to search for.")]
        public string SearchTargetUrl { get; set; }
    }
}
