using AuthServiceAPI.Data;
using AuthServiceAPI.Repositories.Interfaces;

namespace AuthServiceAPI.Repositories
{
    public class AuthRepositoryDB : IAuthRepository
    {
        private readonly AuthDbContext _context;

        public AuthRepositoryDB(AuthDbContext context)
        {
            _context = context;
        }


    }


}
