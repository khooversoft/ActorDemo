using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IFizzBuzzService
    {
        Task<string> GetFizzBuzz(int value);
    }
}