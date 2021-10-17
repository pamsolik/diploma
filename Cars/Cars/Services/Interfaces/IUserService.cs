using System.Threading.Tasks;

namespace Cars.Services.Interfaces
{
    public interface IUserService
    {
        public Task<string> GetUserId(string userName);
    }
}