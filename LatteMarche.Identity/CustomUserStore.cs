using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Auth.Services;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Utenti.Services;
using LatteMarche.Core.Models;
using LatteMarche.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCode.Data;
using WeCode.Data.Interfaces;
using WeCode.Identity;
using System.Linq;

namespace LatteMarche.Identity
{


    /// <summary>
    /// http://www.jamessturtevant.com/posts/ASPNET-Identity2.0-Custom-Database/
    /// http://forums.asp.net/t/2014278.aspx?How+use+custom+IPasswordHasher+
    /// </summary>
    public class CustomUserStore : IUserStore<CustomUser>, IUserPasswordStore<CustomUser>
    {
        private LatteMarcheDbContext database;
        private IUnitOfWork uow;
        private IRepository<Utente, int> utentiRepository;
        private IRepository<TipoProfilo, int> profiliRepository;
        private IAutorizzazioniService authorizationsService;

        public CustomUserStore()
        {
            this.database = new LatteMarcheDbContext();
            this.uow = new UnitOfWork(this.database);
            this.utentiRepository = this.uow.Get<Utente, int>();
            this.profiliRepository = this.uow.Get<TipoProfilo, int>();

            this.authorizationsService = new AutorizzazioniService(new UnitOfWork(this.database));
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
            var utente = this.utentiRepository.GetById(Convert.ToInt32(userId));
            CustomUser user = ConvertToCustomUser(utente);

            if (user != null)
                user.Permissions = this.authorizationsService.GetPermissions(user.UserName);

            return await Task.FromResult<CustomUser>(user);
        }

        public async Task<CustomUser> FindByNameAsync(string username)
        {
            var utente = this.utentiRepository.DbSet.FirstOrDefault(u => u.Username == username);
            CustomUser user = ConvertToCustomUser(utente);

            if (user != null)
                user.Permissions = this.authorizationsService.GetPermissions(username);

            return await Task.FromResult<CustomUser>(user);
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash)
        {
            this.SetPasswordHash(user.UserName, passwordHash);
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(CustomUser user)
        {
            var utente = this.utentiRepository.DbSet.FirstOrDefault(u => u.Username == user.UserName);
            return Task.FromResult<string>(utente.Password);
        }

        public Task<bool> HasPasswordAsync(CustomUser user)
        {
            throw new NotImplementedException();
        }

        private CustomUser ConvertToCustomUser(Utente utente)
        {
            var profilo = this.profiliRepository.GetById(utente.IdProfilo);

            CustomUser user = null;
            if (utente != null)
            {
                user = new CustomUser()
                {
                    Id = utente.Id.ToString(),
                    Email = String.Empty,
                    PasswordHash = utente.Password,
                    Password = utente.Password,
                    UserName = utente.Username,
                    Roles = new List<string>() { profilo.Descrizione.Trim() }
                };
            }

            return user;
        }

        /// <summary>
        /// Impostazione della password hashata
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        private void SetPasswordHash(string username, string passwordHash)
        {
            Utente utente = this.utentiRepository.Query.FirstOrDefault(u => u.Username == username);
            if (utente != null)
            {
                utente.Password = passwordHash;
                this.uow.SaveChanges();
            }
        }

    }
}