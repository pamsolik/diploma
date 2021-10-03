using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.View;

namespace Cars.Services.Interfaces
{
    public interface IUserService
    {
        public Task<string> GetUserId(string userName);
    }
}