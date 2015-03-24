using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IImportTrackRepository : IRepository<ImportTrack>
    {
        IQueryable<ImportTrack> Search(string text);
    }
}
