using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EnpalSharpTemplate.Services
{
    public class SearchHistoryService: IHistorySearch
    {
        private HistoryContext _context;

        public SearchHistoryService(HistoryContext historyContext)
        {
            _context = historyContext;
        }

        public SearchHistoryService() { }

        public async Task<PayloadModel> GetSingleVersion(string key)
        {
            try
            {
                return await _context.HistoryData.Where(x => x.Key == key).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async  Task<List<PayloadViewModel>> GetAll()
        {
            var list = new List<PayloadViewModel>();
            try
            {
                list = await (from record in _context.HistoryData
                        select new PayloadViewModel
                        {
                            Id = record.Id,
                            Key = record.Key,
                            FirstName = record.FirstName,
                            LastName = record.LastName,
                            Address = record.Address,
                            CreatedDate = record.CreatedDate,
                            UpdatedDate = record.UpdatedDate
                        }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return list;
        }
        public bool CheckKeyExist(string key)
        {
            return _context.HistoryData.Any(x => x.Key == key);  
        }

    }
}
