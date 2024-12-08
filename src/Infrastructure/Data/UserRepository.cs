using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data
{
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public User? Authenticate(string identifier, string password)
        {
            User? user = _context.Set<User>().FirstOrDefault(u=>u.Email == identifier && u.Password == password);
            return user;
        }

    }
}
