using Microsoft.EntityFrameworkCore;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Resolvers
{
    public class UsersResolver
    {
        private readonly IDbContextFactory<NZWalksDbContext> _dbContextFactory;

        public UsersResolver(IDbContextFactory<NZWalksDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // Get All Role
        public async Task<List<Role>> GetAllRolesAsync()
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Roles.ToListAsync();
            }
        }

        // Get Role Name By identityId
        public async Task<string> GetRoleNameByIdentityIdAsync(string identityId)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.IdentityId == identityId);
                return user.Role.Name;
            }
        }

        // Get Role Name By Id
        public async Task<string> GetRoleNameByIdAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
                return user.Role.Name;
            }
        }

        // Get Role By Id
        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        // Get Me
        public async Task<User> GetMe(string identityId)
        {
            return await GetByIdentityIdAsync(identityId);
        }

        // Get All Users
        public async Task<List<User>> GetAllAsync()
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Users.ToListAsync();
            }
        }

        // Get User By Id
        public async Task<User> GetByIdAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        // Get User By IdentityId
        public async Task<User> GetByIdentityIdAsync(string identityId)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.IdentityId == identityId);
            }
        }

        // Get User By Email
        public async Task<User> GetByEmailAsync(string email)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            }
        }

        // Create User
        public async Task<User> CreateAsync(User user)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return user;
            }
        }

        // Update User
        public async Task<User> UpdateAsync(string idIdentityId, User user)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingUser = await GetByIdentityIdAsync(idIdentityId);

                if (existingUser == null)
                {
                    return null;
                }

                existingUser.Username = user.Username;
                existingUser.Fullname = user.Fullname;
                existingUser.Dob = user.Dob;
                existingUser.Address = user.Address;
                existingUser.Phone = user.Phone;

                await context.SaveChangesAsync();
                return existingUser;
            }
        }

        // Update Role
        public async Task<User> UpdateRoleAsync(Guid id, Guid roleId)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (existingUser == null)
                {
                    return null;
                }

                var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

                if (role == null)
                {
                    return null;
                }

                existingUser.RoleId = roleId;

                await context.SaveChangesAsync();
                return existingUser;
            }
        }

        // Delete User
        public async Task<User> DeleteAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (existingUser == null)
                {
                    return null;
                }

                context.Users.Remove(existingUser);
                await context.SaveChangesAsync();
                return existingUser;
            }
        }
    }
}
