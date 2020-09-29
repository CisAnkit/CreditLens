using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;

namespace ACERIFT
{
	/// <summary>
	/// Summary description for BS_ACTUAL.
	/// </summary>
	public class BS_ACTUAL: FinancialAnalyst.IReport
	{
		public void Execute(ReportGenerator RG)
		{
			CALCULATIONS calc = new CALCULATIONS(RG);
			calc.BS_Calcs(RG);
			BSandIS utility = new BSandIS();
			utility.DETAILED_BALANCE_SHEET(RG, FORMATCOMMANDS.ACTUAL);
		}
	}
}