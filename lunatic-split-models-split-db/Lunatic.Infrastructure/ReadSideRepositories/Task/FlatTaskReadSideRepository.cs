﻿using Lunatic.Application.Contracts;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;

namespace Lunatic.Infrastructure.ReadSideRepositories.Task {
	public class FlatTaskReadSideRepository(ILunaticReadContext context) 
		: BaseReadSideRepository<FlatTaskReadModel>(context), IFlatTaskReadSideRepository {
	}
}
