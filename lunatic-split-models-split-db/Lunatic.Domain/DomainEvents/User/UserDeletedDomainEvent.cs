using Lunatic.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunatic.Domain.DomainEvents.User {
	public record UserDeletedDomainEvent(Guid Id) : IDomainEvent;
}
