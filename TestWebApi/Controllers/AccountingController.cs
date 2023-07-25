using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi
{
    [Route("/v1/account")]
    public class AccountingController : Controller
    {
        private readonly AccountingService _accountingService;

        public AccountingController(AccountingService accountingService) {

            this._accountingService = accountingService;
            if (this._accountingService == null)
                throw new ArgumentNullException(nameof(accountingService));
        }

        public async Task<IActionResult> GetAccountsAsync() {

            try
            {
                var res = await _accountingService.GetAccountsExceedingFunds();
                return base.Ok(res);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            

        }

    }
}
