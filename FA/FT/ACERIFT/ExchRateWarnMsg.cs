using System.Collections;
using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System.IO;
using System.Text;
using System.Reflection;
using System.Resources;
using System;


namespace ACERIFT
{
	/// <summary>
	/// Summary description for ExchRateWarnMsg.
	/// </summary>
	public class ExchRateWarnMsg
	{
		public ExchRateWarnMsg()
		{
		}

		public static void printWarning(ReportGenerator RG, PRINTCOMMANDS Utility, ResourceManager rm)
		{
			ArrayList rateEnd = new ArrayList();
			ArrayList rateAvg = new ArrayList();
					
			for (int i=0; i< RG.Statements.Count; i++)
			{
				if (RG.ACCOUNT(7100)[i] != 0)
				{
					rateEnd.Add(true.ToString());
				}
				else
				{
					rateEnd.Add(false.ToString());
				}
			}
			for (int i=0; i< RG.Statements.Count; i++)
			{
				if (RG.ACCOUNT(7110)[i] != 0)
				{
					rateAvg.Add(true.ToString());
				}
				else
				{
					rateAvg.Add(false.ToString());
				}
			}
			
			bool printBSWarning = false;
			bool printISWarning = false;
			for (int i=0; i < RG.Statements.Count; i++)
			{
				if (rateEnd[i].ToString() != rateAvg[i].ToString())
				{
					if (rateEnd[i].ToString() == "True")
						printISWarning = true;
					else
						printBSWarning = true;
				}
			}

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (printISWarning)
			{
				Utility.PrintLabel(RG, rm.GetString("msgExchEnd"));
				
			}
			if (printBSWarning)
			{
				Utility.PrintLabel(RG, rm.GetString("msgExchAvg"));
				
			}
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (printBSWarning || printISWarning)
				Utility.PrintLabel(RG, rm.GetString("msgIncorrect"));

			/*
			if (printISWarning) bsVal = 1; //Msg ExchEND
			if (printBSWarning) isVal = 2; //MSG EXchgAVg
			*/
			
			//return (bsVal + isVal);
			
		}

		public static ArrayList ReconcileDate(ReportGenerator RG, ResourceManager rm)
		{

			ArrayList ReconDate = new ArrayList();
			foreach (Statement s in RG.Statements)
			{
				//Check to see if Reconcile id =-1.  If so, the the stmt reconciles to NONE.
				if (s.GetReconcileID(RG.Context) != -1)
				{
					foreach (Statement rs in RG.Statements)
					{
						if (rs.Id == s.GetReconcileID(RG.Context))
							ReconDate.Add(rs.Date);
					}
				}
				else
					///The stmt reconciled to none.
					ReconDate.Add(rm.GetString("none"));
			}

			return ReconDate;

		}
	}
}
