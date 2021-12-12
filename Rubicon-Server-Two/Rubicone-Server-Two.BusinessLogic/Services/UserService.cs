using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rubicone_Server_Two.BusinessLogic.Core.Interfaces;
using Rubicone_Server_Two.BusinessLogic.Core.Models;
using Rubicone_Server_Two.DataAccess.Core.Interfaces.DbContext;
using Rubicone_Server_Two.DataAccess.Core.Models;
using Share.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubicone_Server_Two.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRubicContext _context;

        public UserService(IMapper mapper, IRubicContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserInformationBlo> AuthWithEmail(string email, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);

            if (user == null)
                throw new NotFoundException($"Пользователь с почтой {email} не найден");

            return await ConvertToUserInformation(user);
        }

        public Task<UserInformationBlo> AuthWithLogin(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserInformationBlo> AuthWithPhone(string numberPrefix, string number, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesExist(string numberPrefix, string number)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInformationBlo> Get(int userId)
        {
           UserRto user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new NotFoundException("Пользователь не найден");

           UserInformationBlo userInfoBlo = await ConvertToUserInformation(user);

            return userInfoBlo;
        }

        public Task<UserInformationBlo> RegisterWithPhone(string numberPrefix, string number, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserInformationBlo> Update(string numberPrefix, string number, string password, UserUpdateBlo userUpdateBlo)
        {
            throw new NotImplementedException();
        }
        private async Task<UserInformationBlo> ConvertToUserInformation(UserRto userRto)
        {
            if (userRto == null) throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);

            return userInformationBlo;
        }
    }
}
