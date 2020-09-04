using System;
using System.Collections.Generic;
using System.Linq;

namespace UnrealEstate.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnrealEstateDbContext _context;

        public UserRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void RemoveUser(int id)
        {
            var userFromDb = _context.Users.Find(id);

            _ = userFromDb ?? throw new ArgumentOutOfRangeException(paramName: "user id"
                , message: $"Not found user with id: {id}");

            _context.Users.Remove(userFromDb);

            _context.SaveChanges();
        }

        public void UpdateUser(User updatedUser)
        {
            var userFromDb = _context.Users.Find(updatedUser.Id);

            _ = userFromDb ?? throw new ArgumentOutOfRangeException(paramName: "user id"
                ,message: $"Not found user with id: {updatedUser.Id}");

            _context.Users.Update(updatedUser);
            _context.SaveChanges();
        }
    }
}
