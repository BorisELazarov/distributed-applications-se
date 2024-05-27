using DAL.Entities;
using DTL.DTO;
using DTL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Services
{
    public class UsersServices : BaseServices<UserDTO, User>, IServices<UserDTO>
    {

        public IEnumerable<UserDTO> GetAll()
        {
            List<UserDTO> users = new List<UserDTO>();
            foreach (User user in _dao.GetAll())
            {
                UserDTO dto = new UserDTO();
                dto.Id = user.Id;
                dto.Username = user.Username;
                dto.Password = user.Password;
                dto.FirstName = user.FirstName;
                dto.LastName = user.LastName;
                //dto.Age = user.Age;
                //dto.DateOfRegistration = user.DateOfRegistration;
                dto.BirthDate = user.BirthDate;
                dto.IsMale = user.IsMale;
                users.Add(dto);
            }
            return users.AsQueryable();
        }

        public UserDTO GetById(int id)
        {
            User user=_dao.GetById(id);
            UserDTO dto = new UserDTO();
            dto.Id = user.Id;
            dto.Username = user.Username;
            dto.Password = user.Password;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            //dto.Age = user.Age;
            //dto.DateOfRegistration = user.DateOfRegistration;
            dto.BirthDate = user.BirthDate;
            dto.IsMale = user.IsMale;
            return dto;
        }

        public void Insert(UserDTO dto)
        {

            User user = new User();
            user.Username = dto.Username;
            user.Password = dto.Password;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            //user.Age = dto.Age;
            //user.DateOfRegistration = dto.DateOfRegistration;
            user.BirthDate = dto.BirthDate;
            user.IsMale = dto.IsMale;
            _dao.Insert(user);
        }

        public void Update(UserDTO dto)
        {
            User user = new User();
            user.Id = dto.Id;
            user.Username = dto.Username;
            user.Password = dto.Password;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            //user.Age = dto.Age;
            //user.DateOfRegistration = dto.DateOfRegistration;
            user.BirthDate = dto.BirthDate;
            user.IsMale = dto.IsMale;
            _dao.Update(user);
        }
    }
}
