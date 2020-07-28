using System.Collections;
using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System.Reflection;
using System.Resources;

namespace ACERIFT
{
	/// <summary>
	/// Summary description for CALCULATIONS.
	/// </summary>
	public class CALCULATIONS
	{

		// This constructor method will load all the values current and prior period for TYPES and FLOWS
		public CALCULATIONS(ReportGenerator RG)
		{
			//if multiple reports are being printed then execute this code once.  Just checking some random
			//types to make sure if this code has been executed at least once.
			if ((RG.GetDetailCalcs("T_253") != null) || (RG.GetDetailCalcs("T_66") != null)) return;

			Calc convRate = new Calc(1, RG.Statements.Count);
			string Tlabel = "T_{0}";
			string Flabel = "F_{0}";
			string TlabelLag = "TL_{0}";
			string FlabelLag = "FL_{0}";
			string label = "";
						 
			foreach (FinancialAnalyst.Class c in RG.Customer.Model.Classes)
			{
				convRate = classConvRate(c.Id, RG, false);
				foreach (int d in c.Types)
				{
					//These two types use BS rate eventhough the class as a whole uses BS rate.
					//define these seperately if needed.
					if ((d == 456) || (d == 458)) continue;
					label = string.Format(Tlabel, d);
					if (RG.GetDetailCalcs(label) == null)
						RG.AddCalc(label, RG.DETAILTYPE(d) * convRate);
				}
				foreach (int d in c.Flows)
				{
					if ((d == 456) || (d == 458)) continue;
					label = string.Format(Flabel, d);
					if (RG.GetDetailCalcs(label) == null)
						RG.AddCalc(label, RG.DETAILFLOW(d) * convRate);
				}
				convRate = classConvRate(c.Id, RG, true);
				foreach (int d in c.Types)
				{
					if ((d == 456) || (d == 458)) continue;
					label = string.Format(TlabelLag, d);
					if (RG.GetDetailCalcs(label) == null)
						RG.AddCalc(label, RG.DETAILTYPE(d,RG.LAG) * convRate);
				}
				foreach (int d in c.Flows)
				{
					if ((d == 456) || (d == 458)) continue;
					label = string.Format(FlabelLag, d);
					if (RG.GetDetailCalcs(label) == null)
						RG.AddCalc(label, RG.DETAILFLOW(d,RG.LAG) * convRate);
				}
			}
							
		}

		//If new classes are added this method needs to be modifed to get the 
		//correct converstion rate.
		private Calc classConvRate(int classID, ReportGenerator RG, bool isLag)
		{
			Calc c = new Calc(1, RG.Statements.Count);
			switch(classID)
			{
					//Balance Sheet Classes
				case 5: case 10: case 15: case 20: case 25: case 55: case 60:
					c = (isLag) ? RG.CONV_RATE_BS(RG.LAG): RG.CONV_RATE_BS();
					break;
				   //Income Statement Clasees
				case 30: case 35: case 40: case 75: case 80: case 85: case 90:
					c = (isLag) ? RG.CONV_RATE_IS(RG.LAG): RG.CONV_RATE_IS();
					break;
				default:
					break;
			}
			return c;
		}



		public void BS_Calcs(ReportGenerator RG)
		{
			
			/*
			foreach (FinancialAnalyst.Type t in RG.Customer.Model.Types)
			{
				if (t.Id > 171)
					break;
				string label = "T_" + t.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(t.Id) * RG.CONV_RATE_BS());
			}
			*/

			//35
			if (RG.GetCalc("GrossFixAssts") == null)
				RG.AddCalc("GrossFixAssts", RG.MACRO(M.GROSS_FIXED_ASSETS));
			//40
			if (RG.GetCalc("NetFixAssts") == null)
				RG.AddCalc("NetFixAssts", RG.MACRO(M.NET_FIXED_ASSETS));
			//45
			if (RG.GetCalc("NetIntang") == null)
				RG.AddCalc("NetIntang", RG.MACRO(M.NET_INTANGIBLES));
			if (RG.GetCalc("TotNCAssts") == null)
				RG.AddCalc("TotNCAssts", RG.MACRO(M.TOTAL_NON_CURRENT_ASSETS));
			//50
			if (RG.GetCalc("NetTrdRecv") == null)
				RG.AddCalc("NetTrdRecv", RG.MACRO(M.NET_TRADE_RECEIVABLES));
			//5
			if (RG.GetCalc("TotCurrAssts") == null)
				RG.AddCalc("TotCurrAssts", RG.MACRO(M.TOTAL_CURRENT_ASSETS));
			//60
			if (RG.GetCalc("TotAssts") == null)
				RG.AddCalc("TotAssts", RG.MACRO(M.TOTAL_ASSETS));
			//125
			if (RG.GetCalc("PrmntEquity") == null)
				RG.AddCalc("PrmntEquity", RG.MACRO(M.PERMANENT_EQUITY));

			//126
			if (RG.GetCalc("EqtyAndRsrvs") == null)
				RG.AddCalc("EqtyAndRsrvs", RG.MACRO(M.EQUITY_AND_RESERVES));

			//130
			if (RG.GetCalc("TotEqAndRsrvs") == null)
				RG.AddCalc("TotEqAndRsrvs", RG.MACRO(M.TOTAL_EQUITY_AND_RESERVES));
			if (RG.GetCalc("TotNCLiabs") == null)
				RG.AddCalc("TotNCLiabs", RG.MACRO(M.TOTAL_NON_CURRENT_LIABILITIES));
			if (RG.GetCalc("TotCurrLiab") == null)
				RG.AddCalc("TotCurrLiab", RG.MACRO(M.TOTAL_CURRENT_LIABILITIES));
			//155
			if (RG.GetCalc("TotLiabs") == null)
				RG.AddCalc("TotLiabs", RG.MACRO(M.TOTAL_LIABILITIES));
			//160
			if (RG.GetCalc("TotEqRsrvsLiab") == null)
				RG.AddCalc("TotEqRsrvsLiab", RG.MACRO(M.TOTAL_EQUITY_AND_RESERVES_AND_LIABS));

			
		}

		public void IS_Calcs(ReportGenerator RG)
		{
			
			/*
			foreach (FinancialAnalyst.Type t in RG.Customer.Model.Types)
			{
				if (t.Id < 200)
					continue;
				string label = "T_" + t.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(t.Id) * RG.CONV_RATE_IS());
			}
			*/
			//240
			if (RG.GetCalc("SalesRevs") == null)
				RG.AddCalc("SalesRevs", RG.MACRO(M.SALES_REVENUES));
			//250
			if (RG.GetCalc("TotCostOfSales") == null)
				RG.AddCalc("TotCostOfSales", RG.MACRO(M.TOTAL_COST_OF_SALES));
			if (RG.GetCalc("TotCshCostOfSales") == null)
				RG.AddCalc("TotCshCostOfSales", RG.MACRO(M.TOTAL_CASH_COST_OF_SALES));
			//255
			if (RG.GetCalc("GrossPrft") == null)
				RG.AddCalc("GrossPrft", RG.MACRO(M.GROSS_PROFIT));
			if (RG.GetCalc("cshGrossPrft") == null)
			    RG.AddCalc("cshGrossPrft", RG.MACRO(M.CASH_GROSS_PROFIT));
			//260
			if (RG.GetCalc("OperExpExclDeprAmrt") == null)
				RG.AddCalc("OperExpExclDeprAmrt", RG.MACRO(M.OPER_EXP_EXCL_DEPREC_AMORT));
			//265
			if (RG.GetCalc("EBITDA") == null)
				RG.AddCalc("EBITDA", RG.MACRO(M.EBITDA));
			//270
			if (RG.GetCalc("EBIT") == null)
				RG.AddCalc("EBIT", RG.MACRO(M.EBIT));
			//275
			if (RG.GetCalc("NetIntIncExp") == null)
				RG.AddCalc("NetIntIncExp", RG.MACRO(M.NET_INTEREST_INCOME_EXPENSE));
			//280
			if (RG.GetCalc("NetOthFinIncExp") == null)
				RG.AddCalc("NetOthFinIncExp", RG.MACRO(M.NET_OTHER_FINANCIAL_INC_EXP));
			//285
			if (RG.GetCalc("OthIncExp") == null)
				RG.AddCalc("OthIncExp", RG.MACRO(M.OTHER_INCOME_EXPENSE));
			//290
			if (RG.GetCalc("PrftLossB4Tax") == null)
				RG.AddCalc("PrftLossB4Tax", RG.MACRO(M.PROFIT_LOSS_BEFORE_TAX));
			//295
			if (RG.GetCalc("TotIncTax") == null)
				RG.AddCalc("TotIncTax", RG.MACRO(M.TOTAL_INCOME_TAX));
			//300
			if (RG.GetCalc("PrftLossB4ExtOrdItems") == null)
				RG.AddCalc("PrftLossB4ExtOrdItems", RG.MACRO(M.PROFIT_LOSS_BEF_EXTRAORDINARY_ITEMS));
			//310
			if (RG.GetCalc("NetPrftLoss") == null)
				RG.AddCalc("NetPrftLoss", RG.MACRO(M.NET_PROFIT));
			///New subtotal added by llh 5/25/06
			if (RG.GetCalc("NetPrftLossAftTax") == null)
				RG.AddCalc("NetPrftLossAftTax", RG.MACRO(M.NET_PROFIT_AFTER_TAX));
			//CLASS(35)* RG.CONV_RATE_IS()
			if (RG.GetCalc("TotOthEqtyRsrvsIncExp") == null)
				RG.AddCalc("TotOthEqtyRsrvsIncExp", RG.MACRO(M.TOTAL_OTHER_EQUITY_RESERVE_INC_EXP));
			
			if (RG.GetCalc("TotAdjRetPrftLoss") == null)
				RG.AddCalc("TotAdjRetPrftLoss", RG.MACRO(M.TOTAL_ADJ_RETAINED_PROFIT_LOSS));
			
		}
		public void ExecSummary_Calcs(ReportGenerator RG)
		{
			
			/*
			foreach (FinancialAnalyst.Type t in RG.Customer.Model.Types)
			{
				if (t.Id < 200)
				{
					string label = "T_" + t.Id;
					if (RG.GetDetailCalcs(label) == null)
						RG.AddCalc(label, RG.DETAILTYPE(t.Id) * RG.CONV_RATE_BS());
				}
				else
				{
					string label = "T_" + t.Id;
					if (RG.GetDetailCalcs(label) == null)
						RG.AddCalc(label, RG.DETAILTYPE(t.Id) * RG.CONV_RATE_IS());
				}
			}
			*/
				if (RG.GetCalc("Dividends") == null)
					RG.AddCalc("Dividends",RG.GetDetailCalcs("T_289").GetTotals(RG) + 
					RG.GetDetailCalcs("T_287").GetTotals(RG) + RG.GetDetailCalcs("T_288").GetTotals(RG));
				
			    if (RG.GetCalc("RetProfit") == null)
					RG.AddCalc("RetProfit", RG.GetDetailCalcs("T_89").GetTotals(RG)); 
		
				if (RG.GetCalc("OthEquity") == null)
					RG.AddCalc("OthEquity", RG.MACRO(M.EQUITY_AND_RESERVES)- RG.GetCalc("RetProfit") + RG.GetDetailCalcs("T_96").GetTotals(RG));
					
				
				if (RG.GetCalc("OthLTLiabs") == null)
					RG.AddCalc("OthLTLiabs", RG.MACRO(M.TOTAL_NON_CURRENT_LIABILITIES)-RG.MACRO(M.LONG_TERM_DEBT)-
						RG.GetDetailCalcs("T_114").GetTotals(RG));  

				if (RG.GetCalc("CPLTDSTLoans") == null)
					RG.AddCalc("CPLTDSTLoans", RG.MACRO(M.CURRENT_PORTION_OF_LTD)+ RG.GetDetailCalcs("T_144").GetTotals(RG) +
					RG.GetDetailCalcs("T_145").GetTotals(RG));
				
				if (RG.GetCalc("TradePayCP") == null)
					RG.AddCalc("TradePayCP", RG.GetDetailCalcs("T_165").GetTotals(RG));

				if (RG.GetCalc("OthCurrLiabs") == null)
					RG.AddCalc("OthCurrLiabs", RG.MACRO(M.TOTAL_CURRENT_LIABILITIES)-RG.GetCalc("TradePayCP") -
                    RG.GetCalc("CPLTDSTLoans"));

			if (RG.GetCalc("AllOthNCAstsBS") == null)
				RG.AddCalc("AllOthNCAstsBS", RG.MACRO(M.ALL_OTHER_NON_CUR_ASSETS_NONCONTR)); 

			if (RG.GetCalc("InventoryBS") == null)
				RG.AddCalc("InventoryBS", RG.MACRO(M.INVENTORIES)); 	

			
			if (RG.GetCalc("NetTrRecBS") == null)
				RG.AddCalc("NetTrRecBS", RG.MACRO(M.NET_TRADE_RECEIVABLES)); 
			
			if (RG.GetCalc("CashEqBS") == null)
				RG.AddCalc("CashEqBS", RG.MACRO(M.CASH_AND_EQUIVALENTS)); 

			if (RG.GetCalc("AllOtherCurAstsBS") == null)
				RG.AddCalc("AllOtherCurAstsBS", RG.MACRO(M.ALL_OTHER_CUR_ASSETS_NONCONTR)); 

			if (RG.GetCalc("LTDBS") == null)
				RG.AddCalc("LTDBS", RG.MACRO(M.LONG_TERM_DEBT) + RG.GetDetailCalcs("T_114").GetTotals(RG)); 
			

			
			
			

			}
		
		public void CashFlowMgmt_Calcs(ReportGenerator RG)
		{
			if (RG.GetCalc("cfmOperExpenses") == null)
				RG.AddCalc("cfmOperExpenses", RG.GetCalc("OperExpExclDeprAmrt") / RG.GetCalc("SalesRevs") * 100);

			if (RG.GetCalc("cfmOthOpAsts") == null)
				RG.AddCalc("cfmOthOpAsts", RG.MACRO(M.CFM_OTHER_OPER_ASSETS));

			if (RG.GetCalc("cfmAccruals") == null)
				RG.AddCalc("cfmAccruals", RG.MACRO(M.CFM_ACCRUALS));

			///gross profit
			if (RG.GetCalc("cfmBegCashGrsPft") == null)
				RG.AddCalc("cfmBegCashGrsPft", RG.MACRO(M.CASH_GROSS_PROFIT, RG.LAG) * ((RG.STMT_PERIODS()/12)/(RG.STMT_PERIODS(RG.LAG)/12)));

			if (RG.GetCalc("cfmCashGrsPftGrwth") == null)
				RG.AddCalc("cfmCashGrsPftGrwth", RG.GetCalc("cfmBegCashGrsPft") * RG.GetCalc("salesRevGrwth")/100);

			if (RG.GetCalc("cfmCashGrsPftMgmt") == null)
				RG.AddCalc("cfmCashGrsPftMgmt", RG.GetCalc("cshGrossPrft") - RG.GetCalc("cfmCashGrsPftGrwth") - RG.GetCalc("cfmBegCashGrsPft"));

			///Oper Expense
			if (RG.GetCalc("cfmBegOpExp") == null)
				RG.AddCalc("cfmBegOpExp", RG.MACRO(M.OPER_EXP_EXCL_DEPREC_AMORT, RG.LAG) * ((RG.STMT_PERIODS()/12)/(RG.STMT_PERIODS(RG.LAG)/12)));

			if (RG.GetCalc("cfmOpExpGwth") == null)
				RG.AddCalc("cfmOpExpGwth", RG.GetCalc("cfmBegOpExp") * RG.GetCalc("salesRevGrwth")/100);

			if (RG.GetCalc("cfmOpExpMgmt") == null)
				RG.AddCalc("cfmOpExpMgmt", RG.GetCalc("OperExpExclDeprAmrt") - RG.GetCalc("cfmOpExpGwth") - RG.GetCalc("cfmBegOpExp"));

			///CIG - Net TRade Receivables
			if (RG.GetCalc("cfmCIGNetTrdRecv") == null)
				RG.AddCalc("cfmCIGNetTrdRecv", -1 * (RG.MACRO(M.NET_TRADE_RECEIVABLES, RG.LAG) * RG.GetCalc("salesRevGrwth")/100));

			///Cash Cost Of Sales Growth
			if (RG.GetCalc("cfmCashCostSalesGwth") == null)
				RG.AddCalc("cfmCashCostSalesGwth", (RG.MACRO(M.TOTAL_CASH_COST_OF_SALES) / (RG.MACRO(M.TOTAL_CASH_COST_OF_SALES, RG.LAG) * (RG.STMT_PERIODS()/12)) -1)* 100);

			Calc cogssub = new Calc(double.NaN, RG.Statements.Count);
			for (int i=0; i < RG.Statements.Count; i++)
			{
				if (RG.MACRO(M.TOTAL_CASH_COST_OF_SALES)[i] == 0)
					cogssub[i] = RG.GetCalc("salesRevGrwth")[i];	
				else
					cogssub[i] = RG.GetCalc("cfmCashCostSalesGwth")[i];	
			}
			///Cost of sales substitution logic
			if (RG.GetCalc("cfmCostSalesSub") == null)
				RG.AddCalc("cfmCostSalesSub", cogssub);

			///CIG - Inventories
			if (RG.GetCalc("cfmCIGInventories") == null)
				RG.AddCalc("cfmCIGInventories", -1 * (RG.GetDetailCalcs("TL_52").GetTotals(RG) * RG.GetCalc("cfmCostSalesSub")/100));

			///CIG - Trade Payables CP
			if (RG.GetCalc("cfmCIGTrdPayCP") == null)
				RG.AddCalc("cfmCIGTrdPayCP", RG.GetDetailCalcs("TL_165").GetTotals(RG) * RG.GetCalc("cfmCostSalesSub")/100);

			///CIG - Tot Growth Trading Accounts
			if (RG.GetCalc("cfmTotGwthTrdAccts") == null)
				RG.AddCalc("cfmTotGwthTrdAccts", RG.GetCalc("cfmCIGNetTrdRecv") + RG.GetCalc("cfmCIGInventories") + RG.GetCalc("cfmCIGTrdPayCP"));

			///CIG - Oth Op Assets
			if (RG.GetCalc("cfmCIGOthOpAssets") == null)
				RG.AddCalc("cfmCIGOthOpAssets", -1 * ((RG.MACRO(M.CFM_OTHER_OPER_ASSETS, RG.LAG)) * RG.GetCalc("salesRevGrwth")/100));
			///CIG - Accruals
			if (RG.GetCalc("cfmCIGAccruals") == null)
				RG.AddCalc("cfmCIGAccruals", (RG.MACRO(M.CFM_ACCRUALS, RG.LAG)) * RG.GetCalc("salesRevGrwth")/100);

			///CIG - Tot Growth Other Factors
			if (RG.GetCalc("cfmTotGwthOthFactors") == null)
				RG.AddCalc("cfmTotGwthOthFactors", RG.GetCalc("cfmCIGOthOpAssets") + RG.GetCalc("cfmCIGAccruals"));

			///Tot Cash Imp of Sales Growth
			if (RG.GetCalc("cfmTotCashImpSalesGwth") == null)
				RG.AddCalc("cfmTotCashImpSalesGwth", RG.GetCalc("cfmTotGwthTrdAccts") + RG.GetCalc("cfmTotGwthOthFactors"));
/////////////////////////////////////
			///CIM - Net TRade Receivables
			if (RG.GetCalc("cfmCIMNetTrdRecv") == null)
				RG.AddCalc("cfmCIMNetTrdRecv", RG.MACRO(M.NET_TRADE_RECEIVABLES, RG.LAG) - RG.MACRO(M.NET_TRADE_RECEIVABLES) - RG.GetCalc("cfmCIGNetTrdRecv"));
			///CIM - Inventories
			if (RG.GetCalc("cfmCIMInventories") == null)
				RG.AddCalc("cfmCIMInventories", RG.GetDetailCalcs("TL_52").GetTotals(RG) - RG.GetDetailCalcs("T_52").GetTotals(RG) - RG.GetCalc("cfmCIGInventories"));

			///CIM - Trade Payables CP
			if (RG.GetCalc("cfmCIMTrdPayCP") == null)
				RG.AddCalc("cfmCIMTrdPayCP", RG.GetDetailCalcs("T_165").GetTotals(RG)  - RG.GetDetailCalcs("TL_165").GetTotals(RG) - RG.GetCalc("cfmCIGTrdPayCP"));

			///CIM - Tot Growth Trading Accounts
			if (RG.GetCalc("cfmTotMgmtTrdAccts") == null)
				RG.AddCalc("cfmTotMgmtTrdAccts", RG.GetCalc("cfmCIMNetTrdRecv") + RG.GetCalc("cfmCIMInventories") + RG.GetCalc("cfmCIMTrdPayCP"));

			///CIM - Oth Op Assets
			if (RG.GetCalc("cfmCIMOthOpAssets") == null)
				RG.AddCalc("cfmCIMOthOpAssets", RG.MACRO(M.CFM_OTHER_OPER_ASSETS, RG.LAG) - RG.MACRO(M.CFM_OTHER_OPER_ASSETS) - RG.GetCalc("cfmCIGOthOpAssets"));
			///CIM - Accruals
			if (RG.GetCalc("cfmCIMAccruals") == null)
				RG.AddCalc("cfmCIMAccruals", RG.MACRO(M.CFM_ACCRUALS) - RG.MACRO(M.CFM_ACCRUALS, RG.LAG) - RG.GetCalc("cfmCIGAccruals"));

			///CIM - Tot Growth Other Factors
			if (RG.GetCalc("cfmTotMgmtOthFactors") == null)
				RG.AddCalc("cfmTotMgmtOthFactors", RG.GetCalc("cfmCIMOthOpAssets") + RG.GetCalc("cfmCIMAccruals"));

			///Tot Cash Imp of Sales Growth
			if (RG.GetCalc("cfmTotCashImpMgmt") == null)
				RG.AddCalc("cfmTotCashImpMgmt", RG.GetCalc("cfmTotMgmtTrdAccts") + RG.GetCalc("cfmTotMgmtOthFactors"));

			///Tot Trd Acct and Oth Fact Chg
			if (RG.GetCalc("cfmTotTrdActOthFactChg") == null)
				RG.AddCalc("cfmTotTrdActOthFactChg", RG.GetCalc("cfmTotCashImpSalesGwth") + RG.GetCalc("cfmTotCashImpMgmt"));

			///Cash After Operations
			if (RG.GetCalc("cfmCashAftOps") == null)
				RG.AddCalc("cfmCashAftOps", RG.GetCalc("cfmTotTrdActOthFactChg") + (RG.GetCalc("cshGrossPrft") + (-1 * RG.GetCalc("OperExpExclDeprAmrt"))));

		}
		public void UCACashFlow_Calcs(ReportGenerator RG)
		{
			
			/*
			foreach (FinancialAnalyst.Flow f in RG.Customer.Model.Flows)
			{
				if (f.Id > 400)
					break;
				string label = "F_" + f.Id;
				string lagLabel = "FL_" + f.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(f.Id));
				if (RG.GetDetailCalcs(lagLabel) == null)
					RG.AddCalc(lagLabel, RG.DETAILTYPE(f.Id, RG.LAG));
			}
			*/

				
			if (RG.GetCalc("ucaCshCollect") == null)
				RG.AddCalc("ucaCshCollect", RG.MACRO(M.UCA_CASH_COLL_FROM_SALES));

			if (RG.GetCalc("ucaCshPdSupplier") == null)
				RG.AddCalc("ucaCshPdSupplier", RG.MACRO(M.UCA_CASH_PAID_TO_SUPPLIERS));
			
			if (RG.GetCalc("ucaCshFrmTrdAct") == null)
				RG.AddCalc("ucaCshFrmTrdAct", RG.MACRO(M.UCA_CASH_FROM_TRADE_ACTIV));

			if (RG.GetCalc("ucaCshPdOpCosts") == null)
				RG.AddCalc("ucaCshPdOpCosts", RG.MACRO(M.UCA_CASH_PD_OPER_COSTS));
			
			if (RG.GetCalc("ucaCshAftOp") == null)
				RG.AddCalc("ucaCshAftOp", RG.MACRO(M.UCA_CASH_AFT_OPER));

			if (RG.GetCalc("ucaOthIncExpTaxPd") == null)
				RG.AddCalc("ucaOthIncExpTaxPd", RG.MACRO(M.UCA_OTH_INC_EXP_TAX_PD));

			if (RG.GetCalc("ucaNetCshAftOp") == null)
				RG.AddCalc("ucaNetCshAftOp", RG.MACRO(M.UCA_NET_CASH_AFT_OPER));
			
			if (RG.GetCalc("ucaCshPdDivsInt") == null)
				RG.AddCalc("ucaCshPdDivsInt", RG.MACRO(M.UCA_CSH_PD_DIVS_INT));

			if (RG.GetCalc("ucaNetCshInc") == null)
				RG.AddCalc("ucaNetCshInc", RG.MACRO(M.UCA_NET_CASH_INC));
			
			if (RG.GetCalc("ucaCPLTD") == null)
				RG.AddCalc("ucaCPLTD", RG.MACRO(M.UCA_CPLTD));
			
			if (RG.GetCalc("ucaCshAftDbtAmort") == null)
				RG.AddCalc("ucaCshAftDbtAmort", RG.MACRO(M.UCA_CASH_AFT_DEBT_AMORT));

			if (RG.GetCalc("ucaChgNFA") == null)
				RG.AddCalc("ucaChgNFA", RG.MACRO(M.UCA_CHG_NET_FIXED_ASSETS));

			if (RG.GetCalc("ucaChgIntang") == null)
				RG.AddCalc("ucaChgIntang", RG.MACRO(M.UCA_CHG_IN_INTANG));
			
			if (RG.GetCalc("ucaChgInInvest") == null)
				RG.AddCalc("ucaChgInInvest", RG.MACRO(M.UCA_CHG_IN_INVEST));

			if (RG.GetCalc("ucaCshPdPlantInvest") == null)
				RG.AddCalc("ucaCshPdPlantInvest", RG.MACRO(M.UCA_CASH_PD_PLANT_INVEST));
	
			if (RG.GetCalc("ucaExtraNonCash") == null)
				RG.AddCalc("ucaExtraNonCash", RG.MACRO(M.UCA_EXTRA_NON_CASH));

			if (RG.GetCalc("ucaFinSurp") == null)
				RG.AddCalc("ucaFinSurp", RG.MACRO(M.UCA_FIN_SURPLUS));

			if (RG.GetCalc("ucaChgInLTD") == null)
				RG.AddCalc("ucaChgInLTD", RG.MACRO(M.UCA_CHG_IN_LTD));

			if (RG.GetCalc("ucaTotExtFin") == null)
				RG.AddCalc("ucaTotExtFin", RG.MACRO(M.UCA_TOT_EXTERNAL_FINANCING));

			if (RG.GetCalc("ucaCashAftFin") == null)
				RG.AddCalc("ucaCashAftFin", RG.MACRO(M.UCA_CASH_AFTER_FINANCING));

			//lh added new flow 71 6/14/06
			if (RG.GetCalc("ucaCAFPlusCash") == null)
				RG.AddCalc("ucaCAFPlusCash", (RG.FLOW(69, RG.LAG)* RG.CONV_RATE_BS(RG.LAG)+
					RG.FLOW(71, RG.LAG)* RG.CONV_RATE_BS(RG.LAG))-
					(RG.MACRO(M.UCA_CASH_AFTER_FINANCING)+RG.FLOW(69,RG.LAG)*RG.CONV_RATE_BS(RG.LAG)+
					RG.FLOW(71,RG.LAG)*RG.CONV_RATE_BS(RG.LAG))-
					(RG.FLOW(69)+RG.FLOW(71))*RG.CONV_RATE_BS());

			//lh added new flow 71 6/14/06
			if (RG.GetCalc("ucaCashAdj") == null)
				RG.AddCalc("ucaCashAdj", (RG.MACRO(M.UCA_CASH_AFTER_FINANCING)+
					(RG.FLOW(69,RG.LAG)+RG.FLOW(71,RG.LAG))*
					RG.CONV_RATE_BS(RG.LAG))-(RG.FLOW(69)+RG.FLOW(71))*RG.CONV_RATE_BS());
			
			///cpf 08/14/06 Log 1755:  Split 69 and 71
			if (RG.GetCalc("ucaCashIBIH") == null)
				RG.AddCalc("ucaCashIBIH", (RG.FLOW(69, RG.LAG)* RG.CONV_RATE_BS(RG.LAG)));
			if (RG.GetCalc("ucaCashEquiv") == null)
				RG.AddCalc("ucaCashEquiv", (RG.FLOW(71,RG.LAG)* RG.CONV_RATE_BS(RG.LAG)));

			//lh added new flow 71 6/14/06
			if (RG.GetCalc("ucaEndCash") == null)
				RG.AddCalc("ucaEndCash", ((RG.FLOW(69)+RG.FLOW(71))*RG.CONV_RATE_BS()));

			if (RG.GetCalc("DTCPLTDBank") == null)
				RG.AddCalc("DTCPLTDBank", -1 * (RG.DETAILFLOW(140, RG.LAG) * RG.YEAR() * RG.CONV_RATE_BS(RG.LAG)) - 
					(RG.DETAILFLOW(140, RG.CPLTD) * RG.CONV_RATE_BS()));

			if (RG.GetCalc("DTCPLTDOther") == null)
				RG.AddCalc("DTCPLTDOther", -1 * (RG.DETAILFLOW(141, RG.LAG) * RG.YEAR() * RG.CONV_RATE_BS(RG.LAG)) - 
					(RG.DETAILFLOW(141, RG.CPLTD) * RG.CONV_RATE_BS()));

			if (RG.GetCalc("DTCPLTDConver") == null)
				RG.AddCalc("DTCPLTDConver", -1 * (RG.DETAILFLOW(142, RG.LAG) * RG.YEAR() * RG.CONV_RATE_BS(RG.LAG)) - 
					(RG.DETAILFLOW(142, RG.CPLTD) * RG.CONV_RATE_BS()));

			if (RG.GetCalc("DTCPLTDSub") == null)
				RG.AddCalc("DTCPLTDSub", -1 * (RG.DETAILFLOW(143, RG.LAG) * RG.YEAR() * RG.CONV_RATE_BS(RG.LAG)) - 
					(RG.DETAILFLOW(143, RG.CPLTD) * RG.CONV_RATE_BS()));

			if (RG.GetCalc("DTCPLTDFinLs") == null)
				RG.AddCalc("DTCPLTDFinLs", -1 * (RG.DETAILFLOW(147, RG.LAG) * RG.YEAR() * RG.CONV_RATE_BS(RG.LAG)) - 
					(RG.DETAILFLOW(147, RG.CPLTD) * RG.CONV_RATE_BS()));

					
		

			

			

			
		
		}


		public void DtInputCF_Calcs(ReportGenerator RG)
		{
			/*
			foreach (FinancialAnalyst.Flow f in RG.Customer.Model.Flows)
			{
				if (f.Id <= 400)
					continue;
				string label = "F_" + f.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(f.Id) * RG.CONV_RATE_IS());
			}
			*/
				
			if (RG.GetCalc("cfFrmOprAct") == null)
				RG.AddCalc("cfFrmOprAct", RG.MACRO(M.ICF_CASH_FLOWS_FROM_OPER_ACTIVITIES));
			if (RG.GetCalc("cfFrmInvestAct") == null)
				RG.AddCalc("cfFrmInvestAct", RG.MACRO(M.ICF_CASH_FLOWS_FROM_INVEST_ACTIVITIES));
			if (RG.GetCalc("cfFrmFinAct") == null)
				RG.AddCalc("cfFrmFinAct", RG.MACRO(M.ICF_CASH_FLOWS_FROM_FINANCE_ACTIVITIES));
			//404
			if (RG.GetCalc("totMoveInCash") == null)
				RG.AddCalc("totMoveInCash", RG.MACRO(M.ICF_TOTAL_MOVEMENTS_IN_CASH));
			//1440
			if (RG.GetCalc("adjBegCshDueChgExchRate") == null)
				RG.AddCalc("adjBegCshDueChgExchRate", RG.FLOW(456) * (RG.CONV_RATE_BS() - RG.CONV_RATE_BS(RG.LAG)));
			//1441
			if (RG.GetCalc("adjTotMoveAvgRateEOP") == null)
				RG.AddCalc("adjTotMoveAvgRateEOP", RG.GetCalc("totMoveInCash")/ RG.CONV_RATE_IS() * (RG.CONV_RATE_BS() - RG.CONV_RATE_IS()));
			if (RG.GetCalc("ImpactOfChgInExchRates") == null)
				RG.AddCalc("ImpactOfChgInExchRates", RG.GetCalc("adjBegCshDueChgExchRate") + RG.GetCalc("adjTotMoveAvgRateEOP"));
			if (RG.GetCalc("begPerCashEquiv") == null)
				RG.AddCalc("begPerCashEquiv", RG.MACRO(M.ICF_BEG_OF_PERIOD_CASH));
			if (RG.GetCalc("unExplAdjToCash") == null)
				RG.AddCalc("unExplAdjToCash", RG.MACRO(M.ICF_END_OF_PERIOD_CASH) - RG.GetCalc("totMoveInCash")-
					RG.MACRO(M.ICF_BEG_OF_PERIOD_CASH) - RG.GetCalc("adjBegCshDueChgExchRate") - 
					RG.GetCalc("adjTotMoveAvgRateEOP"));
			if (RG.GetCalc("endPerCashEquiv") == null)
				RG.AddCalc("endPerCashEquiv", RG.MACRO(M.ICF_END_OF_PERIOD_CASH));

				
		
		}
		public void DtInputCFDir_Calcs(ReportGenerator RG)
		{
			/*
			foreach (FinancialAnalyst.Flow f in RG.Customer.Model.Flows)
			{
				if (f.Id <= 400)
					continue;
				string label = "F_" + f.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(f.Id) * RG.CONV_RATE_IS());
			}
			*/		
			if (RG.GetCalc("cfFrmOprActDir") == null)
				RG.AddCalc("cfFrmOprActDir", RG.MACRO(M.ICF_CF_FROM_OPER_ACTIV_DIRECT));
			
				
			if (RG.GetCalc("TotMoveCashDir") == null)
					RG.AddCalc("TotMoveCashDir", RG.MACRO(M.ICF_TOTAL_MOVEMENTS_IN_CASH_DIRECT));
			//1440
			if (RG.GetCalc("adjBegCshDueChgExchRate") == null)
				RG.AddCalc("adjBegCshDueChgExchRate", RG.FLOW(456) * (RG.CONV_RATE_BS() - RG.CONV_RATE_BS(RG.LAG)));

			if (RG.GetCalc("adjTotMoveAvgRateEOPDir") == null)
				RG.AddCalc("adjTotMoveAvgRateEOPDir", RG.GetCalc("TotMoveCashDir")/ RG.CONV_RATE_IS() * (RG.CONV_RATE_BS() - RG.CONV_RATE_IS()));
			
			if (RG.GetCalc("ImpactOfChgInExchRatesDir") == null)
				RG.AddCalc("ImpactOfChgInExchRatesDir", RG.GetCalc("adjBegCshDueChgExchRate") + RG.GetCalc("adjTotMoveAvgRateEOPDir"));

			if (RG.GetCalc("unExplAdjToCashDir") == null)
				RG.AddCalc("unExplAdjToCashDir", RG.MACRO(M.ICF_END_OF_PERIOD_CASH) - RG.GetCalc("TotMoveCashDir")-
					RG.MACRO(M.ICF_BEG_OF_PERIOD_CASH) - RG.GetCalc("adjBegCshDueChgExchRate") - 
					RG.GetCalc("adjTotMoveAvgRateEOPDir"));
							
				}

		public void DetDervCF_Calcs(ReportGenerator RG)
		{
			
			/*
			foreach (FinancialAnalyst.Flow f in RG.Customer.Model.Flows)
			{
				if (f.Id > 400)
					break;
				string label = "F_" + f.Id;
				string lagLabel = "FL_" + f.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(f.Id));
				if (RG.GetDetailCalcs(lagLabel) == null)
					RG.AddCalc(lagLabel, RG.DETAILTYPE(f.Id, RG.LAG));
			}
			*/
		
			//290
			if (RG.GetCalc("prftLossB4Tax") == null)
				RG.AddCalc("prftLossB4Tax", RG.MACRO(M.PROFIT_LOSS_BEFORE_TAX));

			//50
			if (RG.GetCalc("NetTrdRecv") == null)
				RG.AddCalc("NetTrdRecv", RG.MACRO(M.NET_TRADE_RECEIVABLES));

			//431
			if (RG.GetCalc("chgNetRecv") == null)
				RG.AddCalc("chgNetRecv", RG.MACRO(M.DCF_CHG_IN_NET_RECEIVABLES));
			//432
			if (RG.GetCalc("chgOthCurrOperAssts") == null)
				RG.AddCalc("chgOthCurrOperAssts", RG.MACRO(M.DCF_CHG_IN_OTHER_CURRENT_OPER_ASSETS));
			//433
			if (RG.GetCalc("chgOthNCOpAssts") == null)
				RG.AddCalc("chgOthNCOpAssts", RG.MACRO(M.DCF_CHG_IN_OTHER_NON_CURRENT_OPER_ASSETS));
			//434
			if (RG.GetCalc("chgOthCurOpLiab") == null)
				RG.AddCalc("chgOthCurOpLiab", RG.MACRO(M.DCF_CHG_IN_OTHER_CURRENT_OPER_LIAB));
			//435
			if (RG.GetCalc("chgOthNCOpLiab") == null)
				RG.AddCalc("chgOthNCOpLiab", RG.MACRO(M.DCF_CHG_IN_OTHER_NON_CURRENT_OPER_LIAB));
			//436
			if (RG.GetCalc("incTaxPaid") == null)
				RG.AddCalc("incTaxPaid", RG.MACRO(M.DCF_INCOME_TAXES_PAID));
			//438
			if (RG.GetCalc("totAdjs") == null)
				RG.AddCalc("totAdjs", RG.MACRO(M.DCF_TOTAL_ADJUSTMENTS));
			//439
			if (RG.GetCalc("cfFrmOpAct") == null)
				RG.AddCalc("cfFrmOpAct", RG.MACRO(M.DCF_CASH_FLOWS_FROM_OPER_ACTIVITIES));
			//440
			if (RG.GetCalc("chgNetFixAssts") == null)
				RG.AddCalc("chgNetFixAssts", RG.MACRO(M.DCF_CHG_IN_NET_FIXED_ASSETS));
			//441
			if (RG.GetCalc("chgNetIntg") == null)
				RG.AddCalc("chgNetIntg", RG.MACRO(M.DCF_CHG_IN_NET_INTANGIBLES));

			//442
			if (RG.GetCalc("cfFrmInvstAct") == null)
				RG.AddCalc("cfFrmInvstAct", RG.MACRO(M.DCF_CASH_FLOWS_FROM_INVEST_ACTIVITIES));
			//443
			if (RG.GetCalc("intPaid") == null)
				RG.AddCalc("intPaid", RG.MACRO(M.DCF_INTEREST_PAID));
			//444
			if (RG.GetCalc("divPaid") == null)
				RG.AddCalc("divPaid", RG.MACRO(M.DCF_DIVIDENDS_PAID));
			//445
			if (RG.GetCalc("DrvcfFrmFinAct") == null)
				RG.AddCalc("DrvcfFrmFinAct", RG.MACRO(M.DCF_CASH_FLOWS_FROM_FINANCE_ACTIVITIES));
			//450
			if (RG.GetCalc("othAct") == null)
				RG.AddCalc("othAct", RG.MACRO(M.DCF_OTHER_ACTIVITIES));
			//411
			if (RG.GetCalc("chgInExhRatePerEnd") == null)
                //DE73833 - Replaced acct 7100 with CONV_RATE_BS as it will successfully pick DB or grid rates
                RG.AddCalc("chgInExhRatePerEnd", RG.CONV_RATE_BS() - RG.CONV_RATE_BS(RG.LAG));
			
			//******** Need to test this, using this instead of DetailCount.
			int line413 = 0;
			Calc temp = RG.DETAILTYPE(291).GetTotals(RG) + RG.DETAILTYPE(291).GetTotals(RG);

			if (temp.NonZero) line413 = 1;
									
			int line412 = 0;
			if (RG.GetCalc("chgInExhRatePerEnd").NonZero)
				line412 = 1;
			
			if ((line413 + line412) != 0)
			{
				//416
				if (RG.GetCalc("begRetPrftPrevRep") == null)
					RG.AddCalc("begRetPrftPrevRep", RG.TYPE(89, RG.LAG) * RG.CONV_RATE_BS(RG.LAG));
			}
			else
			{
				if (RG.GetCalc("begRetPrftPrevRep") == null)
					RG.AddCalc("begRetPrftPrevRep", new Calc(0, RG.Statements.Count));
			}

			if (RG.GetCalc("chgInExhRatePerEnd").NonZero)
			{
				//419
				if (RG.GetCalc("adjChgInExchRate") == null)
					RG.AddCalc("adjChgInExchRate", (RG.CONV_RATE_BS() - RG.CONV_RATE_BS(RG.LAG))/RG.CONV_RATE_BS(RG.LAG)* RG.GetCalc("begRetPrftPrevRep"));
			}
			else
			{
				//419
				if (RG.GetCalc("adjChgInExchRate") == null)
					RG.AddCalc("adjChgInExchRate", new Calc(0, RG.Statements.Count));
			}

			//426
			if (RG.GetCalc("convAdjNetPrft") == null)
				RG.AddCalc("convAdjNetPrft", RG.MACRO(M.CONVERSION_ADJ_TO_NET_PROFIT));

			//427
			if (RG.GetCalc("unexpAdjRetPrfts") == null)
				RG.AddCalc("unexpAdjRetPrfts", RG.MACRO(M.UNEXPLAINED_ADJ_TO_RET_PROFITS));
			//447
			if (RG.GetCalc("dcfTotMoveInCash") == null)
				RG.AddCalc("dcfTotMoveInCash", RG.MACRO(M.DCF_TOTAL_MOVEMENTS_IN_CASH));
			
			//lh added new flow 71 6/14/06
			if (RG.GetCalc("dcfBegofPerCashEquiv") == null)
				RG.AddCalc("dcfBegofPerCashEquiv", ((RG.FLOW(69, RG.LAG)+RG.FLOW(71,RG.LAG))* RG.CONV_RATE_BS(RG.LAG)));

			//lh added new flow 71 6/14/06
			if (RG.GetCalc("dcfEndofPerCashEquiv") == null)
				RG.AddCalc("dcfEndofPerCashEquiv", RG.GetCalc("dcfTotMoveInCash") + 
				((RG.FLOW(69, RG.LAG)+RG.FLOW(71,RG.LAG))* RG.CONV_RATE_BS(RG.LAG)));
		}
		public void ReconCalcs(ReportGenerator RG)
		{
			DetDervCF_Calcs(RG);
			

			/*
			foreach (FinancialAnalyst.Type t in RG.Customer.Model.Types)
			{
				if (t.Id < 200)
					continue;
				string label = "T_" + t.Id;
				if (RG.GetDetailCalcs(label) == null)
					RG.AddCalc(label, RG.DETAILTYPE(t.Id) * RG.CONV_RATE_IS());
			}
			*/

			//420
			if (RG.GetCalc("begRetPrftRest") == null)
				RG.AddCalc("begRetPrftRest", RG.GetCalc("begRetPrftPrevRep") + RG.GetCalc("adjChgInExchRate") + 
					RG.GetDetailCalcs("T_291").GetTotals(RG) + RG.GetDetailCalcs("T_292").GetTotals(RG));

			Calc tempbegRetErn = new Calc(double.NaN, RG.Statements.Count);
			for (int i=0; i < RG.Statements.Count; i++)
			{
				if (RG.GetCalc("begRetPrftPrevRep")[i] == 0)
				{
					tempbegRetErn[i] = RG.TYPE(89, RG.LAG)[i]*RG.CONV_RATE_IS()[i];	
				}
			}
			//421
			if (RG.GetCalc("begRetEarn") == null)
				RG.AddCalc("begRetEarn", tempbegRetErn);

			//310
			if (RG.GetCalc("NetPrftLoss") == null)
				RG.AddCalc("NetPrftLoss", RG.MACRO(M.NET_PROFIT));

			if (RG.GetCalc("endRetPrfts") == null)
				RG.AddCalc("endRetPrfts", RG.TYPE(89) * RG.CONV_RATE_BS());

			//428 (130 LAG)
			if (RG.GetCalc("begEqAndRsrvs") == null)
				RG.AddCalc("begEqAndRsrvs", RG.MACRO(M.TOTAL_EQUITY_AND_RESERVES, RG.LAG));
			
			if (RG.GetCalc("cshStkDivd") == null)
				RG.AddCalc("cshStkDivd", ( -1 * RG.GetDetailCalcs("T_287").GetTotals(RG)) - RG.GetDetailCalcs("T_288").GetTotals(RG) -
					RG.GetDetailCalcs("T_289").GetTotals(RG) );

			int line413 = 0;
			Calc temp = RG.DETAILTYPE(291).GetTotals(RG) + RG.DETAILTYPE(291).GetTotals(RG);
			if (temp.NonZero) line413 = 1;
			
			//since RG.GetCalc("unexpAdjRetPrfts") is multiplied by -1 adding instead of -
			if (line413 > 0)
			{
				if (RG.GetCalc("adjToRetPrft") == null)
					RG.AddCalc("adjToRetPrft", ( -1 * RG.GetDetailCalcs("T_290").GetTotals(RG)) + 
						RG.GetDetailCalcs("T_291").GetTotals(RG) + RG.GetDetailCalcs("T_292").GetTotals(RG) + 
						RG.GetCalc("convAdjNetPrft") + 	RG.GetCalc("unexpAdjRetPrfts") + RG.GetCalc("adjChgInExchRate"));
			}
			else
			{
				if (RG.GetCalc("adjToRetPrft") == null)
					RG.AddCalc("adjToRetPrft", (-1 * RG.GetDetailCalcs("T_290").GetTotals(RG)) + RG.GetCalc("convAdjNetPrft") -
						RG.GetCalc("unexpAdjRetPrfts") + RG.GetCalc("adjChgInExchRate"));
			}

			if (RG.GetCalc("actEndEqtyRsrvs") == null)
				RG.AddCalc("actEndEqtyRsrvs", RG.MACRO(M.TOTAL_EQUITY_AND_RESERVES));

			if (RG.GetCalc("incDecEqtyRsrvs") == null)
				RG.AddCalc("incDecEqtyRsrvs", RG.MACRO(M.TOTAL_EQUITY_AND_RESERVES) - RG.GetCalc("begEqAndRsrvs"));
		}

		

		public void RatioCalcs(ReportGenerator RG)
		{
            //Liquidity
			if (RG.GetCalc("workingCap") == null)
				RG.AddCalc("workingCap", RG.MACRO(M.WORKING_CAPITAL));
			if (RG.GetCalc("currRatio") == null)
				RG.AddCalc("currRatio", RG.MACRO(M.CURRENT_RATIO));
			if (RG.GetCalc("quickRatio") == null)
				RG.AddCalc("quickRatio", RG.MACRO(M.QUICK_RATIO));
			if (RG.GetCalc("slsRevToWC") == null)
				RG.AddCalc("slsRevToWC", RG.MACRO(M.SALES_REVENUES_TO_WORKING_CAPITAL));
            //aveys 2020-5-27, add ACERIFT custom macros
            if (RG.GetCalc("cashRatio") == null)
                RG.AddCalc("cashRatio", RG.MACRO(M.CASH_RATIO));
            //Leverage
			if (RG.GetCalc("TNW") == null)
				RG.AddCalc("TNW", RG.MACRO(M.TANGIBLE_NET_WORTH));
			if (RG.GetCalc("effTNW") == null)
				RG.AddCalc("effTNW", RG.MACRO(M.EFF_TANGIBLE_NET_WORTH));
			if (RG.GetCalc("dbtToNW") == null)
				RG.AddCalc("dbtToNW", RG.MACRO(M.DEBT_TO_NET_WORTH));
			if (RG.GetCalc("dbtToTNW") == null)
				RG.AddCalc("dbtToTNW", RG.MACRO(M.DEBT_TO_TANGIBLE_NET_WORTH));
			if (RG.GetCalc("dbttLessSubDbtEffTNW") == null)
				RG.AddCalc("dbttLessSubDbtEffTNW", RG.MACRO(M.DEBT_LESS_SUB_DEBT_TO_EFF_TNW));
			if (RG.GetCalc("brwFundToTotLiab") == null)
				RG.AddCalc("brwFundToTotLiab", RG.MACRO(M.BORROWED_FUNDS_TO_TOTAL_LIAB));
			if (RG.GetCalc("brwFundtoEffTNW") == null)
				RG.AddCalc("brwFundtoEffTNW", RG.MACRO(M.BORROWED_FUNDS_TO_EFF_TNW));
			if (RG.GetCalc("brwFundToEBITDA") == null)
				RG.AddCalc("brwFundToEBITDA", RG.MACRO(M.BORROWED_FUNDS_TO_EBITDA));
			if (RG.GetCalc("TotLiabToTotAssts") == null)
				RG.AddCalc("TotLiabToTotAssts", RG.MACRO(M.TOTAL_LIABILITIES) / RG.MACRO(M.TOTAL_ASSETS));
			if (RG.GetCalc("offBSLevrg") == null)
				RG.AddCalc("offBSLevrg", RG.MACRO(M.OFF_BALANCE_SHEET_LEVERAGE));
            //aveys 2020-5-27, add ACERIFT custom macros
            if (RG.GetCalc("dbtToAssts") == null)
                RG.AddCalc("dbtToAssts", RG.MACRO(M.DEBT_TO_ASSETS));
            if (RG.GetCalc("STDbtToTotDbt") == null)
                RG.AddCalc("STDbtToTotDbt", RG.MACRO(M.ST_DEBT_TO_TOTAL_DEBT));
            if (RG.GetCalc("STDbtToWC") == null)
                RG.AddCalc("STDbtToWC", RG.MACRO(M.ST_DEBT_TO_WC));
            if (RG.GetCalc("netDbtToEBIT") == null)
                RG.AddCalc("netDbtToEBIT", RG.MACRO(M.NET_DEBT_TO_EBIT));
            //Coverage
            if (RG.GetCalc("intCovrg") == null)
				RG.AddCalc("intCovrg", RG.MACRO(M.INTEREST_COVERAGE));
			if (RG.GetCalc("earnCovrg") == null)
				RG.AddCalc("earnCovrg", RG.MACRO(M.EARNINGS_COVERAGE));
			//for report print check
			if (RG.GetCalc("icfCFOpAct") == null)
				RG.AddCalc("icfCFOpAct", RG.MACRO(M.ICF_CASH_FLOWS_FROM_OPER_ACTIVITIES));
			if (RG.GetCalc("dcfCFOpAct") == null)
				RG.AddCalc("dcfCFOpAct", RG.MACRO(M.ICF_CF_FROM_OPER_ACTIV_DIRECT));
			if (RG.GetCalc("cfCovrg") == null)
				RG.AddCalc("cfCovrg", RG.MACRO(M.CASH_FLOW_COVERAGE));
            //Profitability
			if (RG.GetCalc("grssPrftMargin") == null)
				RG.AddCalc("grssPrftMargin", RG.MACRO(M.GROSS_PROFIT_MARGIN));
			if (RG.GetCalc("cshGrssPrftMargin") == null)
				RG.AddCalc("cshGrssPrftMargin", RG.MACRO(M.CASH_GROSS_PROFIT_MARGIN));
			if (RG.GetCalc("EBTIDAMargin") == null)
				RG.AddCalc("EBTIDAMargin", RG.MACRO(M.EBITDA_MARGIN));
			if (RG.GetCalc("netOpMargin") == null)
				RG.AddCalc("netOpMargin", RG.MACRO(M.NET_OPERATING_PROFIT_MARGIN));
			if (RG.GetCalc("PrftB4TaxMargin") == null)
				RG.AddCalc("PrftB4TaxMargin", RG.MACRO(M.PROFIT_BEFORE_TAX_MARGIN));
			if (RG.GetCalc("NetPrftMargin") == null)
				RG.AddCalc("NetPrftMargin", RG.MACRO(M.NET_PROFIT_MARGIN));
			if (RG.GetCalc("divdPayRate") == null)
				RG.AddCalc("divdPayRate", RG.MACRO(M.DIVIDEND_PAYOUT_RATE));
			if (RG.GetCalc("effTaxRate") == null)
				RG.AddCalc("effTaxRate", RG.MACRO(M.EFFECTIVE_TAX_RATE));
			if (RG.GetCalc("PBTToTotAssts") == null)
				RG.AddCalc("PBTToTotAssts", RG.MACRO(M.PBT_TO_TOTAL_ASSETS));
			if (RG.GetCalc("PBTToTNW") == null)
				RG.AddCalc("PBTToTNW", RG.MACRO(M.PBT_TO_TANGIBLE_NET_WORTH));
			if (RG.GetCalc("PBTToTotEqtyResrvs") == null)
				RG.AddCalc("PBTToTotEqtyResrvs", RG.MACRO(M.PBT_TO_TOTAL_EQUITY_AND_RESERVES));
			if (RG.GetCalc("ROA") == null)
				RG.AddCalc("ROA", RG.MACRO(M.RETURN_ON_ASSETS));
			if (RG.GetCalc("ROTNW") == null)
				RG.AddCalc("ROTNW", RG.MACRO(M.RETURN_ON_TANGIBLE_NET_WORTH));
			if (RG.GetCalc("ROTotEqtyResrvs") == null)
				RG.AddCalc("ROTotEqtyResrvs", RG.MACRO(M.RETURN_ON_TOTAL_EQUITY_AND_RESERVES));
			//Activity
			if (RG.GetCalc("netTrdRecvDays") == null)
				RG.AddCalc("netTrdRecvDays", RG.MACRO(M.NET_TRADE_RECEIVABLE_DAYS));
			if (RG.GetCalc("invDays") == null)
				RG.AddCalc("invDays", RG.MACRO(M.INVENTORY_DAYS));
			if (RG.GetCalc("invDaysExclCOS") == null)
				RG.AddCalc("invDaysExclCOS", RG.MACRO(M.INVENTORY_DAYS_EXCL_COS_DEPR));
			if (RG.GetCalc("trdPayDays") == null)
				RG.AddCalc("trdPayDays", RG.MACRO(M.TRADE_PAYABLE_DAYS));
			if (RG.GetCalc("trdPayDaysExclCOS") == null)
				RG.AddCalc("trdPayDaysExclCOS", RG.MACRO(M.TRADE_PAYABLE_DAYS_EXCL_COS_DEPR));
			if (RG.GetCalc("salesToTotAssts") == null)
				RG.AddCalc("salesToTotAssts", RG.MACRO(M.SALES_REVENUES_TO_TOTAL_ASSETS));
			if (RG.GetCalc("salesToTNW") == null)
				RG.AddCalc("salesToTNW", RG.MACRO(M.SALES_REVENUES_TO_TANGIBLE_NET_WORTH));
			if (RG.GetCalc("salesToNFA") == null)
				RG.AddCalc("salesToNFA", RG.MACRO(M.SALES_REVENUES_TO_NET_FIXED_ASSETS));
            //aveys 2020-5-27, add ACERIFT custom macros
            if (RG.GetCalc("cashConvCyc") == null)
                RG.AddCalc("cashConvCyc", RG.MACRO(M.CASH_CONV_CYCLE));
            if (RG.GetCalc("invToWC") == null)
                RG.AddCalc("invToWC", RG.MACRO(M.INVENTORY_TO_WC));
            //Growth
            if (RG.GetCalc("totAsstsGrwth") == null)
				RG.AddCalc("totAsstsGrwth", RG.MACRO(M.TOTAL_ASSET_GROWTH));
			if (RG.GetCalc("salesRevGrwth") == null)
				RG.AddCalc("salesRevGrwth", RG.MACRO(M.SALES_REVENUES_GROWTH));
			
			if (RG.GetCalc("EBITDAGrwth") == null)
				RG.AddCalc("EBITDAGrwth", RG.MACRO(M.EBITDA_GROWTH));
			if (RG.GetCalc("netOpPrftGrw") == null)
				RG.AddCalc("netOpPrftGrw", RG.MACRO(M.NET_OPERATING_PROFIT_GROWTH));
			if (RG.GetCalc("netPrftGrwth") == null)
				RG.AddCalc("netPrftGrwth", RG.MACRO(M.NET_PROFIT_GROWTH));
			if (RG.GetCalc("sustainGrowth") == null)
				RG.AddCalc("sustainGrowth", RG.MACRO(M.SUSTAINABLE_GROWTH_RATE));
						
			if (RG.GetCalc("UCACFCoverageCY") == null)
				RG.AddCalc("UCACFCoverageCY", RG.MACRO(M.UCA_CF_COVERAGE_CY));
				
			if (RG.GetCalc("UCACFCoveragePY") == null)
				RG.AddCalc("UCACFCoveragePY", RG.MACRO(M.UCA_CF_COVERAGE_PY));

            //hongzhou 2010-5-10 Add C&I Scorecard ratios
            if (RG.GetCalc("DebtToBookCapital") == null)
                RG.AddCalc("DebtToBookCapital", RG.MACRO(M.DEPT_TO_BOOK_CAP));
            if (RG.GetCalc("CashFromOperationsToDebt") == null)
                RG.AddCalc("CashFromOperationsToDebt", RG.MACRO(M.CASH_FROM_OPS_TO_DEPT));
            if (RG.GetCalc("CashToNetSales") == null)
                RG.AddCalc("CashToNetSales", RG.MACRO(M.CASH_TO_SALES_REV));
            if (RG.GetCalc("Log10NetSales") == null)
                RG.AddCalc("Log10NetSales", RG.MACRO(M.LOG_10_SALES_REV));
		}


		public void CredCompCalcs(ReportGenerator RG)
		{
			//for report print check
			if (RG.GetCalc("icfCFOpAct") == null)
				RG.AddCalc("icfCFOpAct", RG.MACRO(M.ICF_CASH_FLOWS_FROM_OPER_ACTIVITIES));
            //amit: 05/07/07
            if (RG.GetCalc("dcfCFOpAct") == null)
                RG.AddCalc("dcfCFOpAct", RG.MACRO(M.ICF_CF_FROM_OPER_ACTIV_DIRECT));

			Calc tempCalc = new Calc(0, RG.Statements.Count);
			
			
			foreach (Account a in RG.Context.Customer.Accounts)
			{
				bool reverse = false;
				Calc convRate = new Calc(1, RG.Statements.Count);
				
				if (a.ClassID == 60)
				{
					switch (a.Id)
					{
						case 7500: tempCalc = RG.MACRO(M.WORKING_CAPITAL); convRate = RG.CONV_RATE_BS(); break;
						case 7520: tempCalc = RG.MACRO(M.CURRENT_RATIO); break;
						case 7540: tempCalc = RG.MACRO(M.QUICK_RATIO); break;
						case 7560: tempCalc = RG.MACRO(M.DEBT_TO_TANGIBLE_NET_WORTH); reverse = true; break;
						case 7580: tempCalc = RG.MACRO(M.INTEREST_COVERAGE); break;
						case 7590: tempCalc = RG.MACRO(M.EARNINGS_COVERAGE); break;
						case 7600: tempCalc = RG.MACRO(M.CASH_FLOW_COVERAGE); break;
						case 7620: tempCalc = (RG.MACRO(M.GROSS_FIXED_ASSETS) - RG.MACRO(M.GROSS_FIXED_ASSETS, RG.LAG)); convRate = RG.CONV_RATE_BS(); reverse = true; break; 
						case 7640: tempCalc = RG.MACRO(M.OFF_BALANCE_SHEET_LEVERAGE); reverse = true; break;
						case 7660: tempCalc = RG.MACRO(M.NET_PROFIT_MARGIN); break;
						case 7680: tempCalc = RG.MACRO(M.RETURN_ON_ASSETS);  break;
						case 7700: tempCalc = RG.MACRO(M.RETURN_ON_TOTAL_EQUITY_AND_RESERVES);  break;
						case 7720: tempCalc = RG.MACRO(M.NET_TRADE_RECEIVABLE_DAYS); reverse = true; break;
						case 7740: tempCalc = RG.MACRO(M.INVENTORY_DAYS); reverse = true; break;
						case 7760: tempCalc = RG.MACRO(M.TRADE_PAYABLE_DAYS); reverse = true; break;
						case 7780: tempCalc = RG.MACRO(M.SALES_REVENUES_GROWTH); reverse = true; break; 
						case 7800: tempCalc = RG.MACRO(M.TANGIBLE_NET_WORTH);  convRate = RG.CONV_RATE_BS(); break;
						case 7820: tempCalc = RG.MACRO(M.SUBORDINATED_DEBT); convRate = RG.CONV_RATE_BS(); reverse = true; break;
						case 7840: tempCalc = RG.MACRO(M.CASH_DIVIDENDS); convRate = RG.CONV_RATE_IS(); reverse = true; break;
						//lh added new flow 71 6/14/06
						case 7860: tempCalc = (RG.TYPE(69)+RG.TYPE(71)) * RG.CONV_RATE_BS();  convRate = RG.CONV_RATE_BS(); break; 
						default: tempCalc = null;break;

					}
					this.calCulateCompliance(RG, a.Id, tempCalc, reverse, convRate);
						
				}
			}
   
		}

		private void calCulateCompliance(ReportGenerator RG, int acctID, Calc Value,  bool reverseVar, Calc convRate)
		{
			if (Value == null) return;
			RG.AddCalc(acctID+"act", new Calc());
			RG.AddCalc(acctID+"cov", new Calc());
			RG.AddCalc(acctID+"var", new Calc());

			for (int i = 0; i < RG.ACCOUNT(acctID).Count; i++) 
				if (RG.ACCOUNT(acctID)[i] != 0)
				{
					RG.GetCalc(acctID+"act").Add(Value[i]);
					RG.GetCalc(acctID+"cov").Add(RG.ACCOUNT(acctID)[i] * convRate[i]);
					if(reverseVar)
					{
						if ((acctID == 7560) || (acctID == 7640)) 
						{
							if ( (RG.ACCOUNT(acctID)[i] > 0) && (Value[i] < 0))
								RG.GetCalc(acctID+"var").Add(RG.GetCalc(acctID+"act")[i] - RG.GetCalc(acctID+"cov")[i]);
							else
								RG.GetCalc(acctID+"var").Add(RG.GetCalc(acctID+"cov")[i] - RG.GetCalc(acctID+"act")[i]);
						}
						else
						{
							RG.GetCalc(acctID+"var").Add(RG.GetCalc(acctID+"cov")[i] - RG.GetCalc(acctID+"act")[i]);
						}
					}
					else
						RG.GetCalc(acctID+"var").Add(RG.GetCalc(acctID+"act")[i] - RG.GetCalc(acctID+"cov")[i]);
				}
				else
				{
					RG.GetCalc(acctID+"act").Add(0);
					RG.GetCalc(acctID+"cov").Add(0);
					RG.GetCalc(acctID+"var").Add(0);
				}
		}
		
		private void calCulatePeerArray(ReportGenerator RG, string Array_Name, Calc MacroName1, Calc MacroName2, int PBaseId_Calc, int intBase_IND_Calc1, int intBase_IND_Calc2)
		{
			int intBase_IND = 0;
			if (RG.IND(33) == 100)
				intBase_IND = intBase_IND_Calc1;
			else
				intBase_IND = intBase_IND_Calc2;
			if (intBase_IND != 0)
			{
				RG.AddCalc("Temp", MacroName1 / MacroName2 * 100);
				RG.AddCalc(Array_Name, new Calc());
				RG.GetCalc(Array_Name).Add(RG.GetCalc("Temp")[PBaseId_Calc]);
				RG.GetCalc(Array_Name).Add(RG.IND(intBase_IND));
				RG.GetCalc(Array_Name).Add(RG.GetCalc("Temp")[PBaseId_Calc] - RG.IND(intBase_IND));
			}
			else
				RG.AddCalc(Array_Name, new Calc(double.NaN, 3));
		}
		private void calCulatePeerArray(ReportGenerator RG, string Array_Name, Calc MacroName1, Calc MacroName2, int PBaseId_Calc, int intBase_IND_Calc1)
		{
			RG.AddCalc("Temp", MacroName1 / MacroName2 * 100);
			RG.AddCalc(Array_Name, new Calc());
			RG.GetCalc(Array_Name).Add(RG.GetCalc("Temp")[PBaseId_Calc]);
			RG.GetCalc(Array_Name).Add(RG.IND(intBase_IND_Calc1));
			RG.GetCalc(Array_Name).Add(RG.GetCalc("Temp")[PBaseId_Calc] - RG.IND(intBase_IND_Calc1));
		}
		private void calCulateRatio(ReportGenerator RG, string Array_Name, Calc MacroName, int PBaseId_Calc, int IND1, int IND2, int IND3, int IND4, int IND5, int IND6, string Sign)
		{
			//in this part calc will have the value only if calculation has the same sign as it has to have
			//otherwise it will have double.NaN that will not be printed. If sign undefined, cal will be calculated.
			//Customer value for the second and third row equals to 0
			RG.AddCalc("Temp", MacroName);
			RG.AddCalc(Array_Name + "_Low", new Calc());
			RG.AddCalc(Array_Name + "_Med", new Calc());
			RG.AddCalc(Array_Name + "_Upp", new Calc());
			if (((Sign == "positive") && (RG.GetCalc("Temp")[PBaseId_Calc] < 0)) || ((Sign == "negative") && (RG.GetCalc("Temp")[PBaseId_Calc] > 0)))
				RG.GetCalc(Array_Name + "_Upp").Add(double.NaN);
			else 
				RG.GetCalc(Array_Name + "_Upp").Add(RG.GetCalc("Temp")[PBaseId_Calc]);
			RG.GetCalc(Array_Name + "_Med").Add(0);
			RG.GetCalc(Array_Name + "_Low").Add(0);
			if (RG.IND(33) == 100)
			{
				RG.GetCalc(Array_Name + "_Upp").Add(RG.IND(IND1));
				if (((Sign == "positive") && (RG.IND(IND1) < 0)) || ((Sign == "negative") && (RG.IND(IND1) > 0)))
					RG.GetCalc(Array_Name + "_Upp").Add(double.NaN);
				else 
					///CPF 7/26/05 Log 1296:  When calculating variance, use Array_Name+Upp instead of "Temp".  
					RG.GetCalc(Array_Name + "_Upp").Add(RG.GetCalc(Array_Name + "_Upp")[0] - RG.IND(IND1));

				RG.GetCalc(Array_Name + "_Med").Add(RG.IND(IND3));
				if (((Sign == "positive") && (RG.IND(IND3) < 0)) || ((Sign == "negative") && (RG.IND(IND3) > 0)))
					RG.GetCalc(Array_Name + "_Med").Add(double.NaN);
				else 
					///CPF 7/26/05 Log 1296:  When calculating variance, use Array_Name+Upp instead of "Temp".  
					RG.GetCalc(Array_Name + "_Med").Add(RG.GetCalc(Array_Name + "_Upp")[0] - RG.IND(IND3));

				RG.GetCalc(Array_Name + "_Low").Add(RG.IND(IND5));
				if (((Sign == "positive") && (RG.IND(IND5) < 0)) || ((Sign == "negative") && (RG.IND(IND5) > 0)))
					RG.GetCalc(Array_Name + "_Low").Add(double.NaN);
				else 
					///CPF 7/26/05 Log 1296:  When calculating variance, use Array_Name+Upp instead of "Temp".  
					RG.GetCalc(Array_Name + "_Low").Add(RG.GetCalc(Array_Name + "_Upp")[0] - RG.IND(IND5));
			}
			else
			{
				RG.GetCalc(Array_Name + "_Upp").Add(RG.IND(IND2));
				if (((Sign == "positive") && (RG.IND(IND2) < 0)) || ((Sign == "negative") && (RG.IND(IND2) > 0)))
					RG.GetCalc(Array_Name + "_Upp").Add(double.NaN);
				else 
					///CPF 7/26/05 Log 1296:  When calculating variance, use Array_Name+Upp instead of "Temp".  
					RG.GetCalc(Array_Name + "_Upp").Add(RG.GetCalc(Array_Name + "_Upp")[0] - RG.IND(IND2));

				RG.GetCalc(Array_Name + "_Med").Add(RG.IND(IND4));
				if (((Sign == "positive") && (RG.IND(IND4) < 0)) || ((Sign == "negative") && (RG.IND(IND4) > 0)))
					RG.GetCalc(Array_Name + "_Med").Add(double.NaN);
				else 
					///CPF 7/26/05 Log 1296:  When calculating variance, use Array_Name+Upp instead of "Temp".  
					RG.GetCalc(Array_Name + "_Med").Add(RG.GetCalc(Array_Name + "_Upp")[0] - RG.IND(IND4));

				RG.GetCalc(Array_Name + "_Low").Add(RG.IND(IND6));
				if (((Sign == "positive") && (RG.IND(IND6) < 0)) || ((Sign == "negative") && (RG.IND(IND6) > 0)))
					RG.GetCalc(Array_Name + "_Low").Add(double.NaN);
				else 
					///CPF 7/26/05 Log 1296:  When calculating variance, use Array_Name+Upp instead of "Temp".  
					RG.GetCalc(Array_Name + "_Low").Add(RG.GetCalc(Array_Name + "_Upp")[0] - RG.IND(IND6));
			}
		}
		private void calCulateRatio(ReportGenerator RG, string Array_Name, Calc MacroName, int PBaseId_Calc, int IND1, int IND2, int IND3, bool IFNOT)
		{
			RG.AddCalc("Temp", MacroName);
			RG.AddCalc(Array_Name + "_Low", new Calc());
			RG.AddCalc(Array_Name + "_Med", new Calc());
			RG.AddCalc(Array_Name + "_Upp", new Calc());
			if (((RG.IND(33)!= 100) && IFNOT == false) || ((RG.IND(33)== 100) && IFNOT == true))
			{
				RG.GetCalc(Array_Name + "_Upp").Add(RG.GetCalc("Temp")[PBaseId_Calc]);
				RG.GetCalc(Array_Name + "_Med").Add(0);
				RG.GetCalc(Array_Name + "_Low").Add(0);
				RG.GetCalc(Array_Name + "_Upp").Add(RG.IND(IND1));
				RG.GetCalc(Array_Name + "_Upp").Add(RG.GetCalc("Temp")[PBaseId_Calc] - RG.IND(IND1));
				RG.GetCalc(Array_Name + "_Med").Add(RG.IND(IND2));
				RG.GetCalc(Array_Name + "_Med").Add(RG.GetCalc("Temp")[PBaseId_Calc] - RG.IND(IND2));
				RG.GetCalc(Array_Name + "_Low").Add(RG.IND(IND3));
				RG.GetCalc(Array_Name + "_Low").Add(RG.GetCalc("Temp")[PBaseId_Calc] - RG.IND(IND3));
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					RG.GetCalc(Array_Name + "_Upp").Add(0);
					RG.GetCalc(Array_Name + "_Med").Add(0);
					RG.GetCalc(Array_Name + "_Low").Add(0);
				}
			}
		}
		public void Peer_Comp_Ind_Val(ReportGenerator RG, int PBaseId_Calc)
		{
			//Net Fixed Assets
			if (RG.GetCalc("NetFixAsts") == null)
				calCulatePeerArray(RG, "NetFixAsts", RG.MACRO(M.NET_FIXED_ASSETS), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 15, 13);

			//Net Intangibles
			if (RG.GetCalc("NetInt") == null)
				calCulatePeerArray(RG, "NetInt", RG.MACRO(M.NET_INTANGIBLES), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 17, 14);
			
			//All Other Non-Current Assets
			if (RG.GetCalc("AllOtherNonCurAsts") == null)
				calCulatePeerArray(RG, "AllOtherNonCurAsts", RG.MACRO(M.ALL_OTHER_NON_CUR_ASSETS_NONCONTR), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 0, 15);

			//Total peer values
			if (RG.GetCalc("TotNonCurAsts") == null)
				RG.AddCalc("TotNonCurAsts", RG.GetCalc("NetFixAsts") + RG.GetCalc("NetInt") + RG.GetCalc("AllOtherNonCurAsts"));

			//Cash & Equivalents
			if (RG.GetCalc("CashEq") == null)
				calCulatePeerArray(RG, "CashEq", RG.MACRO(M.CASH_AND_EQUIVALENTS), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 8);

			//Net Trade Receivables
			if (RG.GetCalc("NetTrRec") == null)
				calCulatePeerArray(RG, "NetTrRec", RG.MACRO(M.NET_TRADE_RECEIVABLES), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 9);
			//Inventories
			if (RG.GetCalc("Invent") == null)
				calCulatePeerArray(RG, "Invent", RG.MACRO(M.INVENTORIES), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 11, 10);
			//All Other Current Assets
			if (RG.GetCalc("AllOtherCurAsts") == null)
				calCulatePeerArray(RG, "AllOtherCurAsts", RG.MACRO(M.ALL_OTHER_CUR_ASSETS_NONCONTR), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 11);
			//Total Current Assets
			if (RG.GetCalc("TotCurAsts") == null)
				calCulatePeerArray(RG, "TotCurAsts", RG.MACRO(M.TOTAL_CURRENT_ASSETS), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 14, 12);
			//Total Equity & Reserves
			if (RG.GetCalc("TotEqRes") == null)
				calCulatePeerArray(RG, "TotEqRes", RG.MACRO(M.TOTAL_EQUITY_AND_RESERVES), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 31, 26);
			//Long Term Debt
			if (RG.GetCalc("LTD") == null)
				calCulatePeerArray(RG, "LTD", RG.MACRO(M.LONG_TERM_DEBT_PEER), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 28, 23);
			//Deferred Tax Liability
			if (RG.GetCalc("DefTaxLiab") == null)
				calCulatePeerArray(RG, "DefTaxLiab", RG.GetDetailCalcs("T_125").GetTotals(RG), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 29, 24);
			//All Other Non-Current Liabilities
			if (RG.GetCalc("AllOthNonCurLiab") == null)
				calCulatePeerArray(RG, "AllOthNonCurLiab", RG.MACRO(M.ALL_OTHER_NON_CUR_LIAB), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 30, 25);
			//Total Non-Current Liabs
			if (RG.GetCalc("TotNonCurLiab") == null)
				RG.AddCalc("TotNonCurLiab", RG.GetCalc("LTD") + RG.GetCalc("DefTaxLiab") + RG.GetCalc("AllOthNonCurLiab"));
			//Overdrafts, Short Term Loans
			if (RG.GetCalc("Overdrft") == null)
				calCulatePeerArray(RG, "Overdrft", RG.MACRO(M.ST_NOTES_PAY), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 20, 17);
			//Current Portion of LTD
			if (RG.GetCalc("	") == null)
				calCulatePeerArray(RG, "CPLTD", RG.MACRO(M.CURRENT_PORTION_OF_LTD), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 25, 18);
			//Trade Payables(CP)
			if (RG.GetCalc("Trade_Pay") == null)
				calCulatePeerArray(RG, "Trade_Pay", RG.MACRO(M.TRADE_PAYABLES), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 21, 19);
			//Current Tax Payable
			if (RG.GetCalc("CurTaxPay") == null)
				calCulatePeerArray(RG, "CurTaxPay", RG.MACRO(M.CURRENT_TAX_PAYABLE), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 24, 20);
			//All Other Current Liabilities
			if (RG.GetCalc("AllOthCurLiab") == null)
				calCulatePeerArray(RG, "AllOthCurLiab", RG.MACRO(M.ALL_OTHER_CUR_LIAB), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 21);
			//Total Current Liabilities'
			if (RG.GetCalc("TotCurLiab") == null)
				calCulatePeerArray(RG, "TotCurLiab", RG.MACRO(M.TOTAL_CURRENT_LIABILITIES), RG.MACRO(M.TOTAL_ASSETS), PBaseId_Calc, 27, 22);
			//Gross Profit(%)
			if (RG.GetCalc("GrProf") == null)
				calCulatePeerArray(RG, "GrProf", RG.MACRO(M.GROSS_PROFIT), RG.MACRO(M.SALES_REVENUES), PBaseId_Calc, 34, 29);
			//Other Oper Inc & Exp + D&A(%)
			if (RG.GetCalc("OthOperIncExp") == null)
				calCulatePeerArray(RG, "OthOperIncExp", RG.MACRO(M.OTHER_INC_EXP_PLUS_D_A), RG.MACRO(M.SALES_REVENUES), PBaseId_Calc, 35, 30);
			//EBIT(%)
			if (RG.GetCalc("EBIT_To_TA") == null)
				calCulatePeerArray(RG, "EBIT_To_TA", RG.MACRO(M.EBIT), RG.MACRO(M.SALES_REVENUES), PBaseId_Calc, 36, 31);
			//All Other Expenses(net)(%)
			if (RG.GetCalc("OthExp") == null)
				calCulatePeerArray(RG, "OthExp", RG.MACRO(M.ALL_OTHER_EXPENSES_NET), RG.MACRO(M.SALES_REVENUES), PBaseId_Calc, 37, 32);
			//Profit(Loss) Before Tax(%)
			if (RG.GetCalc("ProfLossB4Tax") == null)
				calCulatePeerArray(RG, "ProfLossB4Tax", RG.MACRO(M.PROFIT_LOSS_BEFORE_TAX), RG.MACRO(M.SALES_REVENUES), PBaseId_Calc, 38, 33);
			//Current Ratio
			if (RG.GetCalc("CURRENT_RATIO_Upp") == null)
				calCulateRatio(RG, "CURRENT_RATIO", RG.MACRO(M.CURRENT_RATIO), PBaseId_Calc, 40, 34, 41, 35, 42, 36, "");
			//Quick Ratio
			if (RG.GetCalc("QUICK_RATIO_Upp") == null)
				calCulateRatio(RG, "QUICK_RATIO", RG.MACRO(M.QUICK_RATIO), PBaseId_Calc, 37, 38, 39, false);
			//Net Trade Receivable Days
			if (RG.GetCalc("NetTrRecDays_Upp") == null)
				calCulateRatio(RG, "NetTrRecDays", RG.MACRO(M.NET_TRADE_RECEIVABLE_DAYS), PBaseId_Calc, 79, 82, 80, 83, 81, 84, "");
			//Inventory Days
			if (RG.GetCalc("InvDays_Upp") == null)
				calCulateRatio(RG, "InvDays", RG.MACRO(M.INVENTORY_DAYS), PBaseId_Calc, 85, 86, 87, false);
			//Trade Payable Days
			if (RG.GetCalc("TrPayDays_Upp") == null)
				calCulateRatio(RG, "TrPayDays", RG.MACRO(M.TRADE_PAYABLE_DAYS), PBaseId_Calc, 82, 88, 83, 89, 84, 90, "");
			//Sales to Working Capital
			if (RG.GetCalc("SalesToWorkCap_Upp") == null)
				calCulateRatio(RG, "SalesToWorkCap", RG.MACRO(M.SALES_REVENUES_TO_WORKING_CAPITAL), PBaseId_Calc, 52, 49, 53, 50, 54, 51, "positive");
			//Interest Coverage
			if (RG.GetCalc("IntCov_Upp") == null)
				calCulateRatio(RG, "IntCov", RG.MACRO(M.INTEREST_COVERAGE), PBaseId_Calc, 55, 52, 56, 53, 57, 54, "positive");
			//(Net P&L + D&A - Dvds)/CPLTD
			if (RG.GetCalc("civNetPLtoCPLTD_Upp") == null)
				calCulateRatio(RG, "civNetPLtoCPLTD", RG.MACRO(M.NET_PROFIT_LOSS_PLUS_DEPR_AMORT_LESS_DIVS_TO_CPLTD), PBaseId_Calc, 58, 55, 59, 56, 60, 57, "positive");
			//Net Fixed Asset / TNW
			if (RG.GetCalc("NetFixAstsToTNW_Upp") == null)
				calCulateRatio(RG, "NetFixAstsToTNW", RG.MACRO(M.NET_FIXED_ASSETS_TO_TANGIBLE_NET_WORTH), PBaseId_Calc, 61, 58, 62, 59, 63, 60, "positive");
			//% PBT / TNW
			// lh removed the "*100" 6/12/06
			if (RG.GetCalc("PBTtoTNW_Upp") == null)
				calCulateRatio(RG, "PBTtoTNW", RG.MACRO(M.PBT_TO_TANGIBLE_NET_WORTH), PBaseId_Calc, 67, 64, 68, 65, 69, 66, "positive");
			//% PBT / Total Assets
			// lh removed the "*100" 6/12/06
			if (RG.GetCalc("PBTtoTotAsts_Upp") == null)
				calCulateRatio(RG, "PBTtoTotAsts", RG.MACRO(M.PBT_TO_TOTAL_ASSETS), PBaseId_Calc, 70, 67, 71, 68, 72, 69, "positive");
			//Sales to Net Fixed Assets
			if (RG.GetCalc("SalesToNetFAsts_Upp") == null)
				calCulateRatio(RG, "SalesToNetFAsts", RG.MACRO(M.SALES_REVENUES_TO_NET_FIXED_ASSETS), PBaseId_Calc, 70, 71, 72, false);
			//Sales to Total Assets
			if (RG.GetCalc("SalesToTotAsts_Upp") == null)
				calCulateRatio(RG, "SalesToTotAsts", RG.MACRO(M.SALES_REVENUES_TO_TOTAL_ASSETS), PBaseId_Calc, 73, 74, 75, false);
			//Depr & Amort / Sales(%)
			if (RG.GetCalc("DeprAmrtToSales_Upp") == null)
				calCulateRatio(RG, "DeprAmrtToSales", RG.MACRO(M.DEPREC_AMORT_TO_SALES), PBaseId_Calc, 76, 77, 78, false);
			//Officer Comp / Sales(%)
			if (RG.GetCalc("OfCompToSales_Upp") == null)
				calCulateRatio(RG, "OfCompToSales", RG.MACRO(M.OFFICERS_COMP_TO_SALES), PBaseId_Calc, 76, 79, 77, 80, 78, 81, "");
			//RiskCalc Data
			if (RG.GetCalc("RC1Yr_Low") == null)
				creAteRC(RG, "RC1Yr", 114, 115, 116);
			if (RG.GetCalc("RC5Yr_Low") == null)
				creAteRC(RG, "RC5Yr", 117, 118, 119);
			
		}
		private void creAteRC(ReportGenerator RG, string Array_Name, int IND1, int IND2, int IND3)
		{
			RG.AddCalc(Array_Name + "_Low", new Calc());
			RG.AddCalc(Array_Name + "_Med", new Calc());
			RG.AddCalc(Array_Name + "_Upp", new Calc());

			RG.GetCalc(Array_Name + "_Upp").Add(double.NaN);
			RG.GetCalc(Array_Name + "_Upp").Add(RG.IND(IND1));
			RG.GetCalc(Array_Name + "_Upp").Add(double.NaN);

			RG.GetCalc(Array_Name + "_Med").Add(double.NaN);
			RG.GetCalc(Array_Name + "_Med").Add(RG.IND(IND2));
			RG.GetCalc(Array_Name + "_Med").Add(double.NaN);

			RG.GetCalc(Array_Name + "_Low").Add(double.NaN);
			RG.GetCalc(Array_Name + "_Low").Add(RG.IND(IND3));
			RG.GetCalc(Array_Name + "_Low").Add(double.NaN);
		}
	}
}