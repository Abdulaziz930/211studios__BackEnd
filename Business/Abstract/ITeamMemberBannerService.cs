using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITeamMemberBannerService
    {
        Task<TeamMemberBanner> GetTeamMemberBannerAsync(int id);

        Task<List<TeamMemberBanner>> GetTeamMembersAsync();

        Task<bool> AddAsync(TeamMemberBanner teamMemberBanner);

        Task<bool> UpdateAsync(TeamMemberBanner teamMemberBanner);

        Task<bool> DeleteAsync(int id);
    }
}
