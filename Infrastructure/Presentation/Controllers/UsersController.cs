//using ServiceAbstraction;
//using Shared.DTOs.User;
//namespace Presentation.Controllers
//{
//    [Authorize(Roles = "User")]
//    public class UsersController(IUserService userService)
//        : ApiController
//    {
//        [AllowAnonymous]
//        [HttpGet("users/by-username/{username}")]
//        public async Task<ActionResult<UserByUsernameResult>> GetUserByUsername(string username)
//        {
//            var result = await userService.GetUserByUsernameAsync(username, GetIdFromToken());
//            return Ok(result);
//        }

//        [HttpGet("users/by-name/{name}")]
//        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersByName(string name, int pageSize, int pageIndex)
//        {
//            var users = await userService.GetUsersByNameAsync(name, pageSize, pageIndex, GetIdFromToken());
//            return Ok(users);
//        }

//        [HttpGet("my-profile")]
//        public async Task<IActionResult> GetMyProfileAsync()
//        {
//            var profile = await userService.GetMyProfileAsync(GetIdFromToken());
//            return Ok(profile);
//        }
//        [HttpPut]

//        public async Task<IActionResult> UpdateProfileAsync([FromForm] UpdateUserDto dto)
//        {
//            var result = await userService.UpdateMyProfileAsync(GetIdFromToken(), dto);
//            return Ok(result);
//        }
//    }
//}