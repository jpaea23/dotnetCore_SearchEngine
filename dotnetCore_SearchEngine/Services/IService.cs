using dotnetCore_SearchEngine.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCore_SearchEngine.Services
{
    public interface IService
    {
        Task<IActionResult> ProcessData(SearchData search); 
    }
}
