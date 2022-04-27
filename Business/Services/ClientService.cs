using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClientService : BaseService<Client>, IClientService
    {
        public ClientService(IBaseRepository<Client> _baseRepository) : base(_baseRepository)
        {

        }
    }
}
