using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Backend_Goalify.Core.Entities.Enums;
using Backend_Goalify.Core.Models.Enums;
using Backend_Goalify.Core.Models;

namespace Backend_Goalify.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
    }
}