using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestWebApi
{
    public class AccountingService
    {

        private readonly HttpClient _httpClient;
        private String URL = "https://mockbin.org/bin/20acd654-c45a-4cea-bf6c-ad320a3dc303";
        private (List<Department> data, DateTime expires)? _departmentsCache;
        private int CacheExpireTimeIndays = 10;


        public AccountingService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Department>> GetAccountsExceedingFunds() {
            var responseR = new List<Department>();
            if (!_departmentsCache.HasValue || DateTime.UtcNow > _departmentsCache.Value.expires) {

                await GetAccountsAsync();
            }

            foreach (var item in _departmentsCache.Value.data)
            {
                if (item.FundsUsed >= item.FundsAvailable)
                {
                    responseR.Add(item);
                }

            }

            return responseR;
        }

        public async Task GetAccountsAsync()
        {
            var responseR = new List<Department>();

            var dataResponse = await _httpClient.GetAsync(URL);
            var data = await dataResponse.Content.ReadAsStringAsync();
            if (dataResponse.IsSuccessStatusCode)
            {
                var resData = JsonConvert.DeserializeObject<AccountingResponseModel>(data);
                foreach (var item in resData.Data)
                {
                    var department = new Department();
                    department.FiscalYear = item[9];
                    if (int.TryParse(item[10], out var id))
                    {
                        department.Id = id;
                    }
                    department.Name = item[11];
                    department.Remarks = item[14];
                    if (double.TryParse(item[12], out var fundAvailable))
                    {
                        department.FundsAvailable = fundAvailable;
                    }


                    if (double.TryParse(item[13], out var fundExpense))
                    {
                        department.FundsUsed = fundExpense;
                    }

                    responseR.Add(department);
                   
                }

            }
            else
            {
                throw new ArgumentException(data);
            }

            _departmentsCache = (responseR, DateTime.UtcNow.AddDays(CacheExpireTimeIndays));
        }
    }
}
