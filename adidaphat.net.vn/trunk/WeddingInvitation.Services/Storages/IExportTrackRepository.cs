using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IExportTrackRepository : IRepository<ExportTrack>
    {
        IQueryable<ExportTrack> Search(string text);
    }
}
