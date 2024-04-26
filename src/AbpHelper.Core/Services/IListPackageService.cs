using System.Threading.Tasks;
using AbpTools.AbpHelper.Core.Models;

namespace AbpTools.AbpHelper.Core.Services
{
    public interface IListPackageService
    {
        Task<GetInstalledPackagesOutput> GetInstalledPackagesAsync(string baseDirectory);
    }
}