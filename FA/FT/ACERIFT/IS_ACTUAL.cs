using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;

namespace ACERIFT
{
	/// <summary>
	/// Summary description for IS_ACTUAL.
	/// </summary>
	public class IS_ACTUAL: FinancialAnalyst.IReport
	{
		public void Execute(ReportGenerator RG)
		{
			CALCULATIONS calc = new CALCULATIONS(RG);
			calc.IS_Calcs(RG);
			BSandIS utility = new BSandIS();
			utility.DETAILED_INCOME_STMT(RG, FORMATCOMMANDS.ACTUAL);
		}
	}
}
