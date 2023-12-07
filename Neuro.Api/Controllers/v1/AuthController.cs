// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Neuro.Api.Models;
// using Neuro.Application.Base.Service;
// using Neuro.Application.Helpers;
// using Neuro.Domain.Entities;
// using Neuro.Domain.UnitOfWork;
//
// namespace Neuro.Api.Controllers.v1;
//
// [ApiController]
// [Route("api/v1/[controller]")]
// public class AuthController : BaseController
// {
//     #region constructor
//
//     private readonly BaseBusinessService _baseService;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public AuthController(BaseBusinessService baseService, IUnitOfWork unitOfWork)
//     {
//         _baseService = baseService;
//         _unitOfWork = unitOfWork;
//     }
//
//     #endregion
//
//     [HttpGet("Register/{id}")]
//     public async Task<IActionResult> Register([FromBody] RegisterModel model)
//     {
//         try
//         {
//             var user = new User
//             {
//                 Username = model.Username ?? model.Email,
//                 Email = model.Email,
//                 Password = EncryptionHelper.Encrypt(model.Password)
//             };
//             await _unitOfWork.Repository<User>().InsertAsync(user);
//             var result = await _unitOfWork.SaveChangesAsync();
//             if (result > 0)
//             {
//                 return Ok(new {Username = user.Username, Email = user.Email,IsSuccess = true, Message = "Register Success"});
//             }
//
//             return BadRequest();
//
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
//     
//     [HttpPost("Login")]
//     public async Task<IActionResult> Login([FromBody] LoginModel model)
//     {
//         try
//         {
//             var user = await _unitOfWork.Repository<User>()
//                 .FindBy(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase))
//                 .FirstOrDefaultAsync();
//
//             if (user != null && EncryptionHelper.Decrypt(user.Password).Equals(model.Password))
//             {
//                 return Ok(new {IsSuccess = true, Message = "Login Success"});
//             }
//             return Unauthorized();
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
// }