using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMediatR.Controllers
{
    public class List
    {
        public class Command : IRequest<IEnumerable<string>>
        { }

        public class Handler : IRequestHandler<Command, IEnumerable<string>>
        {
            public IEnumerable<string> Handle(Command message)
            {
                return new string[] { "value1", "value2" };
            }
        }
    }
}
