using System;

namespace FinancialAnalyst.Addin.CHB
{
    [Serializable]
    public class HideAccounts : MFAPlugIn {
        public HideAccounts()
        {
        }

        public override void Start() {
            this.ListenCustomerLoadCompleteEvent();
        }

        public override void OnCustomerLoadComplete(object sender, CustomerEventArgs e) {
            if (PlugInManager.IsModelMappedForPlugIn(e.Customer.Model.Id, this.Id)) {
                var customer = e.Customer;

                // Hide all accounts under class: 'Income Expenses'
                foreach (Account acct in customer.Accounts.AccountsWithClass(C.IncomeExpenses)) {
                    acct.Hidden = true;
                }
            }
        }
    }
}