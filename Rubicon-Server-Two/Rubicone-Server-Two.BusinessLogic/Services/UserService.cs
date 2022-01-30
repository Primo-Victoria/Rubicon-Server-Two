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

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<UserInformationBlo> AuthWithLogin(string login, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(l => l.Login == login && l.Password == password);

            if (user == null)
                throw new NotFoundException($"Пользователь c {login} не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<UserInformationBlo> AuthWithPhone(string numberPrefix, string number, string password)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(n => n.PhoneNumber == number && n.PhoneNumberPrefix == numberPrefix && n.Password == password);

            if (user == null)
                throw new NotFoundException($"Пользователь c {number} не найден");

            return await ConvertToUserInformationAsync(user);
        }

        public async Task<bool> DoesExist(string numberPrefix, string number)
        {
            bool resault = await _context.Users.AnyAsync(u => u.PhoneNumber == number && u.PhoneNumberPrefix == numberPrefix);

            return resault;
        }

        public async Task<UserInformationBlo> Get(int userId)
        {
           UserRto user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new NotFoundException("Пользователь не найден");

           UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(user);

            return userInfoBlo;
        }

        public async Task<UserInformationBlo> RegisterWithPhone(string numberPrefix, string number, string password)
        {
            bool resault = await _context.Users.AnyAsync(u => u.PhoneNumber == number && u.PhoneNumberPrefix == numberPrefix);

            if (resault == true) throw new BadRequestException("Такой номер уже есть");

            UserRto newUser = new UserRto()
            {
                Password = password,
                PhoneNumber = number,
                PhoneNumberPrefix = numberPrefix
            };
            
            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(newUser);
            return userInfoBlo;

        }

        public async Task<UserInformationBlo> Update(UserUpdateBlo userUpdateBlo)
        {
            UserRto user = await _context.Users.FirstOrDefaultAsync(y => y.PhoneNumber == userUpdateBlo.CurrentPhoneNumber && y.PhoneNumberPrefix == userUpdateBlo.CurrentPhoneNumberPrefix && y.Password == userUpdateBlo.CurrentPhoneNumberPassword);

            if (user == null) throw new NotFoundException("Такого пользователя нет");

            user.Password = userUpdateBlo.Password;
            user.FirstName = userUpdateBlo.FirstName;
            user.Email = userUpdateBlo.Email;
            user.Login = userUpdateBlo.Login;
            user.LastName = userUpdateBlo.LastName;
            user.Patronymic = userUpdateBlo.Patronymic;
            user.Birthday = userUpdateBlo.Birthday;
            user.IsBoy = userUpdateBlo.IsBoy;
            user.AvatarUrl = userUpdateBlo.AvatarUrl;
            user.PhoneNumber = userUpdateBlo.PhoneNumber;
            user.PhoneNumberPrefix = userUpdateBlo.PhoneNumberPrefix;

            UserInformationBlo userInfoBlo = await ConvertToUserInformationAsync(user);
            return userInfoBlo;
        } 
        private async Task<UserInformationBlo> ConvertToUserInformationAsync(UserRto userRto)
        {
            if (userRto == null) throw new ArgumentNullException(nameof(userRto));

            UserInformationBlo userInformationBlo = _mapper.Map<UserInformationBlo>(userRto);

            return userInformationBlo;
        }
    }
}
