using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModel_Canvas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BusinessModel_Canvas.Controllers;
using Microsoft.EntityFrameworkCore;
using BusinessModel_Canvas.Models;

namespace BusinessModel_Canvas.Pages
{
    
    public class IndexModel : PageModel
{



    //Canvas_Context _context = new Canvas_Context();
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;

    }

        public void OnGet()
        {






        }

    }
                    
 }

