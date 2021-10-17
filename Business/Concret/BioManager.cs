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
    public class BioManager : IBioService
    {
        private readonly IBioDal _bioDal;
        public BioManager(IBioDal bioDal)
        {
            _bioDal = bioDal;
        }

        public async Task<bool> AddAsync(Bio bio)
        {
            return await _bioDal.AddAsync(bio);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _bioDal.DeleteAsync(new Bio { Id = id });
        }

        public async Task<Bio> GetBioAsync(int id)
        {
            return await _bioDal.GetAsync(x => x.Id == id);
        }

        public async Task<List<Bio>> GetBiosAsync()
        {
            return await _bioDal.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Bio bio)
        {
            return await _bioDal.UpdateAsync(bio);
        }
    }
}
