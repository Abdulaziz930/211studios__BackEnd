using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concret
{
    public class TeamMemberBannerManager : ITeamMemberBannerService
    {
        private readonly ITeamMemberBannerDal _teamMemberBannerDal;

        public TeamMemberBannerManager(ITeamMemberBannerDal teamMemberBannerDal)
        {
            _teamMemberBannerDal = teamMemberBannerDal;
        }

        public async Task<bool> AddAsync(TeamMemberBanner teamMemberBanner)
        {
            return await _teamMemberBannerDal.AddAsync(teamMemberBanner);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _teamMemberBannerDal.DeleteAsync(new TeamMemberBanner { Id = id });
        }

        public async Task<TeamMemberBanner> GetTeamMemberBannerAsync(int id)
        {
            return await _teamMemberBannerDal.GetAsync(x => x.Id == id);
        }

        public async Task<List<TeamMemberBanner>> GetTeamMembersAsync()
        {
            return await _teamMemberBannerDal.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(TeamMemberBanner teamMemberBanner)
        {
            return await _teamMemberBannerDal.UpdateAsync(teamMemberBanner);
        }
    }
}
