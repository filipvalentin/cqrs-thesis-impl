﻿using Lunatic.Application.Models.ReadModels.Tasks;

namespace Lunatic.Application.Persistence.ReadSide.Task
{
    public interface IFlatTaskReadSideRepository : IAsyncReadSideRepository<FlatTaskReadModel>
    {
    }
}
