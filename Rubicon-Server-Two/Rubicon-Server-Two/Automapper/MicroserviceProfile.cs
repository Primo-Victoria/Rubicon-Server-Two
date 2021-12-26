using AutoMapper;
using Rubicon_Server_Two.Core.Models;
using Rubicone_Server_Two.BusinessLogic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubicon_Server_Two.Automapper
{
    public class MicroserviceProfile : Profile
    {
        public MicroserviceProfile()
        {
            CreateMap<UserInformationBlo, UserInformationDto>();
            CreateMap<UserUpdateBlo, UserUpdateDto>();
        }
    }
}
