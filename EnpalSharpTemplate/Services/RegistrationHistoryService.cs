using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.Model;

namespace EnpalSharpTemplate.Services
{
    public class RegistrationHistoryService : IHistoryRegistration
    {
        private HistoryContext _context;
        public RegistrationHistoryService(HistoryContext historyContext)
        {
            _context = historyContext;
        }

        public async Task<bool> Create(PayloadModel payload)
        {
            bool result = false;
            var model = _context.HistoryData.Where(x => x.Key == payload.Key).FirstOrDefault();
            try
            {
                if (model != null)
                {
                    model.FirstName = payload.FirstName;
                    model.LastName = payload.LastName;
                    model.Address = payload.Address;
                    model.UpdatedDate = DateTime.UtcNow;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    payload.Id = Guid.NewGuid();
                    payload.CreatedDate = DateTime.UtcNow;
                    _context.HistoryData.Add(payload);
                    await _context.SaveChangesAsync();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }
    }
}
