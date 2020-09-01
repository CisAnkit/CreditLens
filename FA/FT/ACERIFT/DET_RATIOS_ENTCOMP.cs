using System;
using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;

namespace ACERIFT
{
	/// <summary>
	/// Summary description for DET_RATIOS.
	/// </summary>
	public class DET_RATIOS_ENTCOMP: FinancialAnalyst.IReport
	{
		public void Execute(ReportGenerator RG)
		{
			bool isPeer = false;
			RATIOS.ratios_entcomp_report(RG, isPeer);
			
		}
	}
}
