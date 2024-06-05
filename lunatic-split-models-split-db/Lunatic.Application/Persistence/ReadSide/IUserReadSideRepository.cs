using Lunatic.Application.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunatic.Application.Persistence.ReadSide {
	public interface IUserReadSideRepository : IAsyncReadSideRepository<UserReadModel> {
	}
}
