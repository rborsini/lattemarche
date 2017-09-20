using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Services;
using LatteMarche.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Identity
{


    /// <summary>
    /// http://www.jamessturtevant.com/posts/ASPNET-Identity2.0-Custom-Database/
    /// http://forums.asp.net/t/2014278.aspx?How+use+custom+IPasswordHasher+
    /// </summary>
    public class CustomUserStore : IUserStore<CustomUser>, IUserPasswordStore<CustomUser>
    {
        private LatteMarcheDbContext database;
        private IUtentiService service;

        public CustomUserStore()
        {
            this.database = new LatteMarcheDbContext();
            this.service = new UtentiService(new UnitOfWork(this.database));
        }

        public void Dispose()
        {
            this.database.Dispose();
        }

        public Task CreateAsync(CustomUser user)
        {
            return Task.FromResult(0);
        }

        public Task UpdateAsync(CustomUser user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CustomUser user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<CustomUser> FindByIdAsync(string userId)
        {
            UtenteDto userDto = service.Details(Convert.ToInt32(userId));
            CustomUser user = ConvertToCustomUser(userDto);

            return await Task.FromResult<CustomUser>(user);
        }

        public async Task<CustomUser> FindByNameAsync(string userName)
        {
            UtenteDto userDto = this.service.GetByUsername(userName);
            CustomUser user = ConvertToCustomUser(userDto);

            return await Task.FromResult<CustomUser>(user);
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash)
        {
            //service.SetPasswordHash(user.UserName, passwordHash);
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(CustomUser user)
        {
            UtenteDto userDto = service.GetByUsername(user.UserName);
            return Task.FromResult<string>(userDto.Password);
        }

        public Task<bool> HasPasswordAsync(CustomUser user)
        {
            throw new NotImplementedException();
        }

        private CustomUser ConvertToCustomUser(UtenteDto userDto)
        {
            CustomUser user = null;
            if (userDto != null)
            {
                user = new CustomUser()
                {
                    Id = userDto.Id.ToString(),
                    Email = String.Empty,
                    PasswordHash = userDto.Password,
                    Password = userDto.Password,
                    UserName = userDto.Username
                };
            }

            return user;
        }

    }
}