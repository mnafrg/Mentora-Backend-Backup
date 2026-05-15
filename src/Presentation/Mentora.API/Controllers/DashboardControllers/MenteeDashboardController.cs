using Mentora.Application.DTOs.Dashboards.AdminDashboard;
using Mentora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers.DashboardControllers.AdminDashboardController
{

    [ApiController]
    [Route("api/[controller]")]

    public class MenteeDashboardController : ControllerBase
    {
        private readonly IMenteeDashboardServices _menteeService;

        public MenteeDashboardController(IMenteeDashboardServices menteeService)
        {
            _menteeService = menteeService;


        }
    }
}

