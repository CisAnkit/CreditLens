using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System.Reflection;
using System.Resources;
using System;
using FinancialAnalyst.DataManager;

namespace ACERIFT
{
    /// <summary>
    /// Summary description for BSandIS.
    /// </summary>
    public class BSandIS
    {
        private ResourceManager rm = null;
        private PRINTCOMMANDS Utility = null;
        private FORMATCOMMANDS FormatCommands = null;
        private ReportGenerator RG = null;
        private int ReportType;
        private int[] ColumnFormula;
        private int[] DetColumnFormula;
        private bool[] ColRound;
        //private int RptTypeVal;
        private int NumCols;
        private int startConst; //Start Statement Const to Remove
        private int endConst; //End Statement Const to Remove

        public BSandIS()
        {
            rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);
            Utility = new PRINTCOMMANDS();
            FormatCommands = new FORMATCOMMANDS();
            ReportType = 0;
            ColumnFormula = new int[1];
            DetColumnFormula = new int[1];
            ColRound = new bool[1];
            startConst = 0;
            endConst = 0;
        }

        private void initReports(ReportGenerator RGG, int RepType, string setCrossFoot)
        {

            RG = RGG;
            ReportType = RepType;
            switch (ReportType)
            {
                case FORMATCOMMANDS.ACTUAL:
                    NumCols = 1; break;
                case FORMATCOMMANDS.ACT_PERCENT:
                    NumCols = 2; break;
                case FORMATCOMMANDS.ACT_EXCHANGE:
                    NumCols = 2; break;
                case FORMATCOMMANDS.ACT_TREND:
                    NumCols = 2;
                    break;
                default:
                    NumCols = 1; break;
            }

            FormatCommands.LoadFormatDefaults(RG);

            RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");
            RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
            RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");

            Utility.LoadColumnHeadingDefaults(RG);
            //Remove the stmt constants added in the default.
            Utility.arrColHead.RemoveRange(startConst, endConst);

            if (ReportType == FORMATCOMMANDS.ACT_PERCENT)
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_2, "1");
                RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_2, "-");
                RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_2, "()");
                RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_2, "");
                RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_2, "True");
                RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, setCrossFoot);
            }
            else if ((ReportType == FORMATCOMMANDS.ACT_EXCHANGE))
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_2, "()");
                RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_2, "-");
                RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_COLUMN_2, "1.0");
                RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_2, "True");
                RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_2, "");
                RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, setCrossFoot);
                ColRound[0] = true;
            }
            else if ((ReportType == FORMATCOMMANDS.ACT_TREND))
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_2, "()");
                RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_2, "N/A");
                RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_2, "");
                RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_2, "1");
                RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, setCrossFoot);
                RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_2, "True");
            }
            else //Actual
                RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, setCrossFoot);

            if (ReportType == FORMATCOMMANDS.ACT_TREND)
            {
                //Remove the last two stmt constants added in the default.
                ///CPF 08/17/06 Log 1769:  Changed the RemoveRange(3,3) to (4,3) to accomodate accounting standard
                Utility.arrColHead.RemoveRange(4, 3);
                //Re-Add the last two stmt constants with the extra "Trend % Chg" labels in column 2.
                ///CPF 08/17/06 Log 1769:  Changed stmt constant indexes (1,2,3) to (2,3,4) for accomodating accounting standard
                StatementConstant sc1 = (StatementConstant)RG.Customer.Model.StatementConstants[2];
                StatementConstant sc2 = (StatementConstant)RG.Customer.Model.StatementConstants[3];
                StatementConstant sc3 = (StatementConstant)RG.Customer.Model.StatementConstants[4];
                ColumnHeader ch1 = new ColumnHeader(sc1.Label, sc1, rm.GetString("bsTrend"), "");
                ColumnHeader ch2 = new ColumnHeader(sc2.Label, sc2, rm.GetString("percent"), "");
                ColumnHeader ch3 = new ColumnHeader(sc3.Label, sc3, rm.GetString("chg"), "");
                Utility.arrColHead.Add(ch1);
                Utility.arrColHead.Add(ch2);
                Utility.arrColHead.Add(ch3);
            }



            if (ReportType == FORMATCOMMANDS.ACT_EXCHANGE)
            {
                ///CPF 08/17/06 Log 1770:  Add currency after all of the stmt constants.  Because Acct Standard was
                ///added, it pushed Stmt Type down a spot.  As a result, the following line removed it.
                //Utility.arrColHead.RemoveRange(6, 1);
                ColumnHeader ch1 = new ColumnHeader(rm.GetString("currency"), rm.GetString("target"), rm.GetString("original"), "");
                Utility.arrColHead.Add(ch1);
            }

            Utility.arrColHead.RemoveRange(1, 1);
            ColumnHeader chead1 = new ColumnHeader(rm.GetString("periods"), RG.GetPrintOrderCalc(RG.STMT_PERIODS()), "", "");
            Utility.arrColHead.Insert(1, chead1);



            ///This creates the standard page header for the report.
            Utility.CreatePageHeader(RG);
            ///This prints the statement constant rows
            Utility.PrintStmtConstRows(RG, NumCols);
            //Print Source and Target Currency
            Utility.PrintSourceTargetCurr(RG);

        }

        public void DETAILED_BALANCE_SHEET(ReportGenerator RGG, int RepType)
        {

            /// Set up the fomula for the second column
            ColumnFormula[0] = M.COLUMN_2_PERCENT_TOTAL_ASSETS;
            DetColumnFormula[0] = M.COLUMN_2_PERCENT_TOTAL_ASSETS_DC;
            ColRound[0] = false;

            initReports(RGG, RepType, "False");

            ///This is the outer most keep together for the entire report
            Utility.mT.AddStartRow(Utility.nRow + 1);

            //NC Assets
            Utility.mT.AddStartRow(Utility.nRow + 1);
            //20
            int GrsFixAsts = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_5")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_6")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_7")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_8")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_9")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_10")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_12")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_13"));
            //22
            int AccumDepr = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_14"));
            //24
            //lh added new types 19 -22, 4/20/06
            int Intangibles = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_15")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_16")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_18")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_19")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_20")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_21")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_22")) +
                                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_17"));
            //26
            int NCAssts = FormatCommands.DetailCount(RG, RG.DETAILCLASS(5));
            //Print Exchange Rate

            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "5");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
            ///CPF 02/01/06 Log 1603:  Before printing specific "End" and "Average" rates, check to see if we are using acct
            ///rates.  The only circumstances under which we print the End/Avg rates are:  1.  Using grid rates 2. Using Exch DB AND both end/avg are required.
            bool useAcctRate = OrgPropertyDataManager.GetRptsUseAcctRate();
            eDualConversion dualConversion = OrgPropertyDataManager.GetDualConversion();
            if (useAcctRate || (!useAcctRate && (dualConversion == eDualConversion.On)))
                Utility.PrintSummary(RG, rm.GetString("exchRatePE"), RG.CONV_RATE_BS(), ReportType, new int[] { M.COLUMN_2_EMPTY }, ColRound);
            else
                Utility.PrintSummary(RG, rm.GetString("exchRate"), RG.CONV_RATE_BS(), ReportType, new int[] { M.COLUMN_2_EMPTY }, ColRound);
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");

            if ((ReportType == FORMATCOMMANDS.ACT_EXCHANGE))
            {
                ColumnFormula[0] = M.COLUMN_2_BS_NO_EXCHANGE;
                DetColumnFormula[0] = M.COLUMN_2_BS_NO_EXCHANGE_DC;
                RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_2, "-");
            }
            else if ((ReportType == FORMATCOMMANDS.ACT_TREND))
            {
                ColumnFormula[0] = M.COLUMN_2_BS_TREND;
                DetColumnFormula[0] = M.COLUMN_2_BS_TREND_DC;
                RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_2, "");

            }
            else if (ReportType == FORMATCOMMANDS.ACT_PERCENT)
                RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_2, "N/A");


            Utility.Skip(RG, 1);

            /// Print the "NON-CURRENT ASSETS" header
            if (NCAssts > 0)
                Utility.PrintLabel(RG, rm.GetString("nonCurAstHdr"));

            if (GrsFixAsts < 1)
                GrsFixAsts = 0;
            else if (GrsFixAsts > 1)
                GrsFixAsts = 2;

            if (AccumDepr == 0)
                AccumDepr = 3;
            else
                AccumDepr = 6;

            int line86 = GrsFixAsts + AccumDepr;

            if ((line86 == 2) || (line86 >= 5))
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            printDetails(new int[] { 5, 6, 7, 8, 9, 10, 12, 13 });

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");

            //if ((line86 == 2) || (line86 == 5) || (line86 == 7))
            if ((line86 == 2) || (line86 == 5))
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("netFixAssets"), RG.GetCalc("NetFixAssts"), ReportType, ColumnFormula, ColRound);
            }

            if (line86 == 8)
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("grsFixAssets"), RG.GetCalc("GrossFixAssts"), ReportType, ColumnFormula, ColRound);
            }

            if (line86 >= 6)
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
                Utility.PrintDetail(RG, RG.GetDetailCalcs("T_14"), ReportType, DetColumnFormula, ColRound);

            }
            if ((line86 == 8) || (line86 == 7))
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("netFixAssets"), RG.GetCalc("NetFixAssts"), ReportType, ColumnFormula, ColRound);

            }
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

            //Utility.Skip(RG, 1);

            if (Intangibles > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            else
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");

            //lh added types 19-22 4/20/06
            printDetails(new int[] { 15, 16, 19, 20, 21, 22, 17, 18 });

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (Intangibles > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
                Utility.PrintSummary(RG, rm.GetString("netItangibles"), RG.GetCalc("NetIntang"), ReportType, ColumnFormula, ColRound);
            }

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            printDetails(new int[] { 27, 28, 30, 31, 32, 33, 34, 35, 36, 37, 39, 40, 41 });

            if (NCAssts > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");

            Utility.PrintSummary(RG, rm.GetString("totNCAssts"), RG.GetCalc("TotNCAssts"), ReportType, ColumnFormula, ColRound);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            Utility.Skip(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);
            //NC Assets End

            //Current Assets
            Utility.mT.AddStartRow(Utility.nRow + 1);
            Calc line50 = (RG.TYPE(66) - RG.TYPE(67)) * RG.CONV_RATE_BS();
            Calc line60 = (RG.CLASS(5) + RG.CLASS(10)) * RG.CONV_RATE_BS();
            //30
            //lh added type(63) 4/20/06
            int curAssts = FormatCommands.DetailCount(RG, RG.DETAILCLASS(10));
            int line31 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_50")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_52")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_54")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_55")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_56")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_63")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_57")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_58")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_59")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_60")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_61")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_64")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_65"));

            int line32 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_66")) +
                        FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_67"));
            if (curAssts > 0)
                Utility.PrintLabel(RG, rm.GetString("curAstHdr"));
            //lh added type 56 4/20/06
            printDetails(new int[] { 50, 52, 54, 55, 56, 63, 57, 58, 59, 60, 61, 64, 65 });

            if (line32 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            printDetails(new int[] { 66, 67 });
            if (line32 > 1)
                Utility.UnderlineColumn(RG, NumCols, 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            if (line32 > 1)
                Utility.PrintSummary(RG, rm.GetString("netTrdRecv"), RG.GetCalc("NetTrdRecv"), ReportType, ColumnFormula, ColRound);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            //lh added type(70) 4/20/06
            //lh added new type 71 6/14/06
            ///cpf flipflopped 71 and 69 to change print order 8/11/06
            printDetails(new int[] { 70, 68, 71, 69 });

            if (curAssts > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            Utility.PrintSummary(RG, rm.GetString("totCurrAssts"), RG.GetCalc("TotCurrAssts"), ReportType, ColumnFormula, ColRound);
            Utility.Skip(RG, 1);
            Utility.PrintSummary(RG, rm.GetString("totAssts"), RG.GetCalc("TotAssts"), ReportType, ColumnFormula, ColRound);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            //Utility.Skip(RG, 1);
            Utility.UnderlinePage(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);
            //Current Assts End

            //Equity and Reserves
            Utility.mT.AddStartRow(Utility.nRow + 1);

            int line121 = FormatCommands.DetailCount(RG, RG.DETAILCLASS(15));
            int line122 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_80")) +
                           FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_81")) +
                           FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_82")) +
                           FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_87")) +
                           FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_83"));
            int line123 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_84")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_85")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_86")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_89")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_90")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_91")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_93")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_94")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_95")) +
                            FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_96"));

            if (line121 > 0)
                Utility.PrintLabel(RG, rm.GetString("eqtyResrvs").ToUpper());

            if (line122 > 0)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            else
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");

            printDetails(new int[] { 80, 81, 82, 87, 83 });
            if (line122 > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);


            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            Utility.PrintSummary(RG, rm.GetString("prmntEqty"), RG.GetCalc("PrmntEquity"), ReportType, ColumnFormula, ColRound);
            //Utility.Skip(RG, 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");



            if (line123 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

            printDetails(new int[] { 84, 85, 86, 89, 90, 91, 93, 94, 95 });
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (line123 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
                Utility.PrintSummary(RG, rm.GetString("eqtyResrvs"), RG.GetCalc("EqtyAndRsrvs"), ReportType, ColumnFormula, ColRound);
            }

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            Utility.PrintDetail(RG, RG.GetDetailCalcs("T_96"), ReportType, DetColumnFormula, ColRound);

            if (line123 > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            Utility.PrintSummary(RG, rm.GetString("totEqtyRsrvs"), RG.GetCalc("TotEqAndRsrvs"), ReportType, ColumnFormula, ColRound);
            Utility.Skip(RG, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");


            Utility.mT.AddEndRow(Utility.nRow);
            //Equity and Reserver End

            //NC LIABS
            Utility.mT.AddStartRow(Utility.nRow + 1);
            int line141 = FormatCommands.DetailCount(RG, RG.DETAILCLASS(20));

            if (line141 > 0)
                Utility.PrintLabel(RG, rm.GetString("ncLiabs"));
            //lh added types 128, 129 4/20/06
            printDetails(new int[]{110, 111, 112, 113, 114, 115, 116, 117, 118, 129, 119,
                                         120, 121, 122, 123, 124, 125, 126, 128, 127});
            if (line141 > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            Utility.PrintSummary(RG, rm.GetString("totNCLiabs"), RG.GetCalc("TotNCLiabs"), ReportType, ColumnFormula, ColRound);
            Utility.Skip(RG, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            Utility.mT.AddEndRow(Utility.nRow);
            //NC LIABS END

            //Current LIABS
            Utility.mT.AddStartRow(Utility.nRow + 1);
            int line151 = FormatCommands.DetailCount(RG, RG.DETAILCLASS(25));

            if (line151 > 0)
                Utility.PrintLabel(RG, rm.GetString("currLiabs"));

            //lh added types 159, 161, 171 4/20/06
            printDetails(new int[]{140, 141, 142, 143, 144, 145, 147, 148, 149, 150, 151, 152,
                                      153, 159, 154, 155, 156, 157, 158, 160, 161, 165, 166, 167, 168, 169,
                                      171, 170});
            if (line151 > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            Utility.PrintSummary(RG, rm.GetString("totCurrLiabs"), RG.GetCalc("TotCurrLiab"), ReportType, ColumnFormula, ColRound);
            Utility.Skip(RG, 1);

            Utility.PrintSummary(RG, rm.GetString("totLiabs"), RG.GetCalc("TotLiabs"), ReportType, ColumnFormula, ColRound);
            Utility.Skip(RG, 1);
            Utility.PrintSummary(RG, rm.GetString("totEqtyRsrvLiabs"), RG.GetCalc("TotEqRsrvsLiab"), ReportType, ColumnFormula, ColRound);
            Utility.mT.AddEndRow(Utility.nRow);
            //Current LIABS END

            for (int i = 0; i < RG.Statements.Count; i++)
            {
                if (Math.Round(RG.GetCalc("TotAssts")[i], 2) != Math.Round(RG.GetCalc("TotEqRsrvsLiab")[i], 2))
                {
                    Utility.PrintLabel(RG, rm.GetString("msgAstToLiab"));
                    break;
                }
            }

            ///CPF 07/27/06 Log 1733:  Added supplemental data section.
            ///First see if anything has been spread
            int OffBsItems = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_503")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_504")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_506")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_508")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_509")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_510"));

            ///if something has been spread, then proceed.
            if (OffBsItems > 0)
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
                Utility.Skip(RG, 1);
                Utility.PrintLabel(RG, rm.GetString("bsSupData"));

                ///CPF 8/11/06 Log 1733:  added logic to non print column 2 if act & %
                if (ReportType == FORMATCOMMANDS.ACT_PERCENT)
                {
                    ColumnFormula[0] = M.COLUMN_2_EMPTY;
                    DetColumnFormula[0] = M.COLUMN_2_EMPTY;
                    RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "");
                    RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_2, "");
                }

                printDetails(new int[] { 503, 504, 506 });
                ///had to do these as print summaries because the strings where slighly different from the
                ///account names, so I couldn't use print detail.
                ///CPF 8/11/06 Log 1733:  added logic to non print column 2 for whole number items if exchange rpt.
                if ((ReportType == FORMATCOMMANDS.ACT_EXCHANGE))
                {
                    ColumnFormula[0] = M.COLUMN_2_EMPTY;
                    RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_2, "");
                }
                //amit: log# 1997
                RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");

                //ESW 05/18/17 - Replaced PrintSummary with PrintDetails to match the rest of the report
                Utility.PrintSummary(RG, rm.GetString("bsNumOutComShares"), RG.GetDetailCalcs("T_509").GetTotals(RG), ReportType, ColumnFormula, ColRound);
                Utility.PrintSummary(RG, rm.GetString("bsNumOutPreShares"), RG.GetDetailCalcs("T_510").GetTotals(RG), ReportType, ColumnFormula, ColRound);
                Utility.PrintSummary(RG, rm.GetString("bsNumEmployees"), RG.GetDetailCalcs("T_508").GetTotals(RG), ReportType, ColumnFormula, ColRound);
                //printDetails(new int[] { 509, 510, 508 });
            }
            ///CPF 02/01/06 Log 1603:  This warning is no longer necessary.
            ///ExchRateWarnMsg.printWarning(RG, Utility, rm);
            Utility.UnderlinePage(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.CloseReport(RG);

        }

        public void DETAILED_INCOME_STMT(ReportGenerator RGG, int RepType)
        {

            /// Set up the fomula for the second column
            ColumnFormula[0] = M.COLUMN_2_PERCENT_NET_SALES;
            DetColumnFormula[0] = M.COLUMN_2_PERCENT_NET_SALES_DC;
            ColRound[0] = false;

            initReports(RGG, RepType, "True");

            //lh removed type 205 4/20/06
            //cpf added type 205 back as it was decided not to remove goods and services 7/06/06.
            int line200 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_202")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_205")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_206")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_203")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_204"));

            int line201 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_200"));
            int line202 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_208"));

            int line205 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_209")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_210")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_211"));

            int line207 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_212")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_213")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_214")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_217")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_218")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_222")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_223")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_226")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_224")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_215")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_216")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_228")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_227"));

            int line208 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_219")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_220")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_221")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_225")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_208"));
            //lh added type 234 4/20/06
            int line210 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_235")) +
                         FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_233")) +
                         FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_232")) +
                         FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_234"));
            int line211 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_243")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_249")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_244")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_251")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_250"));
            int line213 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_245")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_246")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_247")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_248")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_252")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_253")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_254"));

            int line215 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_258")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_259")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_260"));

            int line216 = line208 + line210 + line211 + line213 + line215 +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_238")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_239"));

            int line217 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_268")) +
                FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_269"));

            //Minority Interest separated from Extraordinary
            int lineMinorityInt = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("T_263"));

            int line220 = FormatCommands.DetailCount(RG, RG.DETAILCLASS(35));
            int line221 = FormatCommands.DetailCount(RG, RG.DETAILCLASS(40));

            //Start: Report Keep together
            Utility.mT.AddStartRow(Utility.nRow + 1);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            //Print Exchange Rate
            RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "5");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
            ///CPF 02/01/06 Log 1603:  Before printing specific "End" and "Average" rates, check to see if we are using acct
            ///rates.  The only circumstances under which we print the End/Avg rates are:  1.  Using grid rates 2. Using Exch DB AND both end/avg are required.
            bool useAcctRate = OrgPropertyDataManager.GetRptsUseAcctRate();
            eDualConversion dualConversion = OrgPropertyDataManager.GetDualConversion();
            if (useAcctRate || (!useAcctRate && (dualConversion == eDualConversion.On)))
                Utility.PrintSummary(RG, rm.GetString("exchRateAvg"), RG.CONV_RATE_IS(), ReportType, new int[] { M.COLUMN_2_EMPTY }, ColRound);
            else
                Utility.PrintSummary(RG, rm.GetString("exchRate"), RG.CONV_RATE_IS(), ReportType, new int[] { M.COLUMN_2_EMPTY }, ColRound);
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "True");

            if ((ReportType == FORMATCOMMANDS.ACT_EXCHANGE))
            {
                ColumnFormula[0] = M.COLUMN_2_IS_NO_EXCHANGE;
                DetColumnFormula[0] = M.COLUMN_2_IS_NO_EXCHANGE_DC;
            }
            else if ((ReportType == FORMATCOMMANDS.ACT_TREND))
            {
                ColumnFormula[0] = M.COLUMN_2_IS_TREND;
                DetColumnFormula[0] = M.COLUMN_2_IS_TREND_DC;
            }
            else if ((ReportType == FORMATCOMMANDS.ACT_PERCENT))
                RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_2, "N/A");



            Utility.Skip(RG, 1);

            if (line201 == 0)
            {
                RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
                Utility.PrintSummary(RG, rm.GetString("salesRevs"), RG.GetCalc("SalesRevs"), ReportType, ColumnFormula, ColRound);
            }
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            Utility.PrintDetail(RG, RG.GetDetailCalcs("T_200"), ReportType, DetColumnFormula, ColRound);

            if (line200 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            //lh removed 205 4/20/06
            //cpf added 205 back as it was decided not to remove goods and services 7/06/06.
            printDetails(new int[] { 203, 204, 202, 205, 206 });

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            int line320 = 0;
            int line321 = 0;
            int line322 = 0;
            if (line200 > 1)
                line320 = 1;
            if (line202 > 0)
                line321 = 2;

            line322 = line320 + line321;
            if (line322 == 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("totCostSales").ToUpper(), RG.GetCalc("TotCostOfSales"), ReportType, ColumnFormula, ColRound);
            }
            else if (line322 == 3)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("totCshCostSales"), RG.GetCalc("TotCshCostOfSales"), ReportType, ColumnFormula, ColRound);

            }

            if ((line322 == 0) || (line322 == 1))
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("grssPrft").ToUpper(), RG.GetCalc("GrossPrft"), ReportType, ColumnFormula, ColRound);
            }
            else if ((line322 == 2) || (line322 == 3))
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("cshGrssPrft"), RG.GetCalc("cshGrossPrft"), ReportType, ColumnFormula, ColRound);

            }

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            printDetails(new int[] { 209, 210, 211 });

            //if (line205 > 0)
            //Utility.Skip(RG, 1);

            if (line207 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

            printDetails(new int[] { 212, 213, 214, 217, 218, 222, 223, 226, 224, 215, 216, 228, 227 });

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (line207 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("oprExpExclDepAmrt"), RG.GetCalc("OperExpExclDeprAmrt"), ReportType, ColumnFormula, ColRound);
            }
            Utility.UnderlineColumn(RG, NumCols, 1);

            Utility.PrintSummary(RG, rm.GetString("ebitda"), RG.GetCalc("EBITDA"), ReportType, ColumnFormula, ColRound);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            if (line208 > 0)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

            printDetails(new int[] { 208, 219, 220, 221, 225 });

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (line208 > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);
            Utility.PrintSummary(RG, rm.GetString("netOpPrftEBIT").ToUpper(), RG.GetCalc("EBIT"), ReportType, ColumnFormula, ColRound);

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            if (line210 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            //lh added type 234 4/20/06
            printDetails(new int[] { 235, 232, 234, 233 });

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (line210 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("netIntIncExp"), RG.GetCalc("NetIntIncExp"), ReportType, ColumnFormula, ColRound);
            }

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");


            printDetails(new int[] { 238, 239 });

            if (line211 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            printDetails(new int[] { 243, 244, 249, 251, 250 });

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            if (line211 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("netOthFinIncExp"), RG.GetCalc("NetOthFinIncExp"), ReportType, ColumnFormula, ColRound);
            }

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            if (line213 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            printDetails(new int[] { 245, 246, 247, 248, 254, 252, 253 });
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            if (line213 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("othIncExp"), RG.GetCalc("OthIncExp"), ReportType, ColumnFormula, ColRound);
            }

            if (line216 > 0)
                Utility.UnderlineColumn(RG, NumCols, 1);

            Utility.PrintSummary(RG, rm.GetString("prftLossB4tax"), RG.GetCalc("PrftLossB4Tax"), ReportType, ColumnFormula, ColRound);

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            if (line215 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            printDetails(new int[] { 258, 259, 260 });
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (line215 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("totIncTax"), RG.GetCalc("TotIncTax"), ReportType, ColumnFormula, ColRound);
            }
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            //lh moved type 263 into the Extraordinary section 4/20/06
            //RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            //Utility.PrintDetail(RG, RG.GetDetailCalcs("T_263"), ReportType, DetColumnFormula, ColRound);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            if (line217 > 0)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("prftLsB4ExOrdItem"), RG.GetCalc("PrftLossB4ExtOrdItems"), ReportType, ColumnFormula, ColRound);
            }
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            if (line217 > 1)
                RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

            printDetails(new int[] { 268, 269 });
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            if (lineMinorityInt > 0)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
            }
            Utility.PrintSummary(RG, rm.GetString("NetProfitLossAftTax"), RG.GetCalc("NetPrftLossAftTax"), ReportType, ColumnFormula, ColRound);
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            printDetails(new int[] { 263 });
            if (lineMinorityInt > 0)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
            }
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            Utility.PrintSummary(RG, rm.GetString("netPrftLoss"), RG.GetCalc("NetPrftLoss"), ReportType, ColumnFormula, ColRound);
            Utility.UnderlineColumn(RG, NumCols, 1);

            if (line202 > 0)
            {
                Utility.Skip(RG, 1);
                Utility.PrintSummary(RG, rm.GetString("totCostSales"), RG.GetCalc("TotCostOfSales"), ReportType, ColumnFormula, ColRound);
                Utility.PrintSummary(RG, rm.GetString("grssPrft"), RG.GetCalc("GrossPrft"), ReportType, ColumnFormula, ColRound);


            }
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            if (line220 > 0)
            {
                Utility.Skip(RG, 1);
                Utility.PrintLabel(RG, rm.GetString("othEqRsrvIncExp"));
            }
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            Utility.PrintDetail(RG, (RG.DETAILCLASS(35) * RG.CONV_RATE_IS()), ReportType, DetColumnFormula, ColRound);

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            if (line220 > 1)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("totOthEqRsrv"), RG.GetCalc("TotOthEqtyRsrvsIncExp"), ReportType, ColumnFormula, ColRound);
            }
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddStartRow(Utility.nRow + 1);

            if (line221 > 0)
            {
                Utility.Skip(RG, 1);
                Utility.PrintLabel(RG, rm.GetString("adjToRetPrft"));
            }
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
            Utility.PrintDetail(RG, (RG.DETAILCLASS(40) * RG.CONV_RATE_IS()), ReportType, DetColumnFormula, ColRound);

            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
            //amit:04/24/06 log #1585; print even if 0
            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            if (line221 > 0)
            {
                Utility.UnderlineColumn(RG, NumCols, 1);
                Utility.PrintSummary(RG, rm.GetString("totAdjRetPrftLoss"), RG.GetCalc("TotAdjRetPrftLoss"), ReportType, ColumnFormula, ColRound);
            }

            ///CPF 02/01/06 Log 1603:  This warning is no longer necessary.
            //ExchRateWarnMsg.printWarning(RG, Utility, rm);
            Utility.UnderlinePage(RG, 1);

            Utility.mT.AddEndRow(Utility.nRow);
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.CloseReport(RG);
        }

        private void printDetails(int[] ids)
        {

            string label = "";

            for (int i = 0; i < ids.Length; i++)
            {
                label = "T_" + ids[i].ToString();
                Utility.PrintDetail(RG, RG.GetDetailCalcs(label), ReportType, DetColumnFormula, ColRound);
            }

        }
    }
}
