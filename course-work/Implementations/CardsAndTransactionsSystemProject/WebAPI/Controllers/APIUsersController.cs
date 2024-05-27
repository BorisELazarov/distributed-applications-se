using DAL.Entities;
using DTL.DTO;
using DTL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APIUsersController : ControllerBase
    {
        UsersRepository _usersRepo = new UsersRepository();
        CardsRepository _cardsRepo = new CardsRepository();

        [HttpGet]
        public ActionResult<List<UserDTO>> GetAllUsers()
        {
            return Ok(_usersRepo.GetAll().ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(int id)
        {
            UserDTO user = _usersRepo.GetFirstOrDefault(x => x.Id == id);
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        [HttpGet("{id}")]
        public ActionResult<decimal> GetBalance(int id)
        {
            decimal balance = 0;
            foreach (CardDTO card in _cardsRepo.GetAll(x => x.UserId == id))
            {
                balance += card.Balance;
            }
            return Ok(balance);
        }

        [HttpGet("{username}, {password}")]
        public ActionResult<int> Login(string username, string password)
        {
            UserDTO user = _usersRepo.GetFirstOrDefault(x => x.Username == username
            && x.Password == password);
            if (user == null) return NotFound("Wrong credentials");
            return Ok(user.Id);
        }

        [HttpPost]
        public ActionResult<UserDTO> PostUser(UserDTO user)
        {
            _usersRepo.Save(user);
            user = _usersRepo.GetFirstOrDefault(x => x.Username == user.Username);
            return Ok(user);
        }

        [HttpPut]
        public ActionResult<UserDTO> PutUser(UserDTO user)
        {
            UserDTO dto = _usersRepo.GetFirstOrDefault(x => x.Id == user.Id);
            if ( user == null)
            {
                return NotFound();
            }
            dto.Username = user.Username;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            dto.BirthDate = user.BirthDate;
            dto.IsMale = user.IsMale;
            _usersRepo.Save(dto);
            return NoContent();
        }

        [HttpGet("{id}, {oldPassword}, {newPassword}")]
        public ActionResult<UserDTO> ChangePassword(int id, string oldPassword,
            string newPassword)
        {
            UserDTO user = _usersRepo.GetFirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Password!=oldPassword)
            {
                return BadRequest("Wrong password!");
            }
            user.Password = newPassword;
            _usersRepo.Save(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            if (_usersRepo.GetFirstOrDefault(x=>x.Id == id)==null)
                return NotFound();
            _usersRepo.Delete(id);
            return Ok();
        }
    }
}
