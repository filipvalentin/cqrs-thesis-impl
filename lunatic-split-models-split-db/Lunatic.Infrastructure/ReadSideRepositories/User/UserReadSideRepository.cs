using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunatic.Infrastructure.ReadSideRepositories.User
{
    public class UserReadSideRepository : BaseReadSideRepository<UserReadModel>, IUserReadSideRepository
    {
        public UserReadSideRepository(ILunaticReadContext context) : base(context)
        {
        }
    }
}
