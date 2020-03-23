/**********************************************************************************************************************
 * Copyright Moody's Analytics. All Rights Reserved.
 *
 * This software is the confidential and proprietary information of
 * Moody's Analytics. ("Confidential Information"). You shall not
 * disclose such Confidential Information and shall use it only in
 * accordance with the terms of the license agreement you entered
 * into with Moody's Analytics.
 *********************************************************************************************************************/

using MKMV.RiskAnalyst.Ratings.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moodys.CreditLens.RiskGrading.CustomBlockLibrary {
    internal class CustomArithmetic: PersistFunction {
        public const string OPERATOR = "+,-,*,/,%";

        public override string GenerateOutputs(RatingCustomer theCust) {
            double aResult = double.NaN;

            Numeric aFirstInput = this.GetInput(theCust, "FirstInput") as Numeric;
            IOTypeList aInputs = this.GetInput(theCust, "SubsequentInputs") as IOTypeList;
            Numeric aDefaultFirst = this.GetProperty("DefaultFirstInput") as Numeric;
            Numeric aDefaultSecond = this.GetProperty("DefaultSecondInput") as Numeric;
            Text aOperator = this.GetProperty("Operation") as Text;
            if (aFirstInput == null || (aFirstInput != null && double.IsNaN(aFirstInput.Value))) {
                if (aDefaultFirst != null) {
                    aFirstInput = aDefaultFirst;
                }
            }

            if (aFirstInput != null && !double.IsNaN(aFirstInput.Value)) {
                aResult = aFirstInput.Value;
            }

            if (aOperator == null) {
                this.SetOutput(theCust, "Result", null);
                return null;
            }

            string aOperation = aOperator.Value;
            if (aOperation.Equals("+", StringComparison.CurrentCultureIgnoreCase)) {
                double aSubTotal = double.NaN;
                bool aFirst = true;

                if (aInputs != null) {
                    int aInputCount = aInputs.Values.Count;
                    if (aInputCount == 0) {
                        if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                            aSubTotal = aDefaultSecond.Value;
                            aResult = aResult + aSubTotal;
                        }
                    } else {
                        foreach (Numeric aEachInput in aInputs.Values) {
                            if (aFirst) {
                                aSubTotal = aEachInput.Value;
                                aFirst = false;
                            } else {
                                aSubTotal += aEachInput.Value;
                            }
                        }
                        aResult = aResult + aSubTotal;
                    }
                } else if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                    aSubTotal = aDefaultSecond.Value;
                    aResult = aResult + aSubTotal;
                }
            } else if (aOperation.Equals("-", StringComparison.CurrentCultureIgnoreCase)) {
                if (double.IsNaN(aResult)) {
                    this.SetOutput(theCust, "Result", null);
                    return null;
                }

                if (aInputs != null) {
                    int aInputCount = aInputs.Values.Count;
                    if (aInputCount == 0) {
                        if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                            aResult -= aDefaultSecond.Value;
                        }
                    } else {
                        bool aIsAllNull = true;
                        foreach (Numeric aEachInput in aInputs.Values) {
                            double aEachInputValue = aEachInput.Value;
                            if (double.IsNaN(aEachInputValue)) {
                                aInputCount--;
                                aEachInputValue = 0;
                            } else {
                                aIsAllNull = false;
                            }
                            aResult -= aEachInputValue;
                        }

                        if (true == aIsAllNull) {
                            aResult = double.NaN;
                        }
                    }
                } else if (aDefaultSecond != null) {
                    if (double.IsNaN(aDefaultSecond.Value)) {
                        aDefaultSecond.Value = 0;
                    }
                    aResult -= aDefaultSecond.Value;
                }
            } else if (aOperation.Equals("*", StringComparison.CurrentCultureIgnoreCase)) {
                double aSubTotal = double.NaN;
                bool aFirst = true;

                if (aInputs != null) {
                    int aInputCount = aInputs.Values.Count;
                    if (aInputCount == 0) {
                        if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                            aSubTotal = aDefaultSecond.Value;
                            aResult = aResult * aSubTotal;
                        }
                    } else {
                        foreach (Numeric aEachInput in aInputs.Values) {
                            if (aFirst) {
                                aSubTotal = aEachInput.Value;
                                aFirst = false;
                            } else {
                                aSubTotal *= aEachInput.Value;
                            }
                        }
                        aResult = aResult * aSubTotal;
                    }
                } else if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                    aSubTotal = aDefaultSecond.Value;
                    aResult = aResult * aSubTotal;
                }
            } else if (aOperation.Equals("/", StringComparison.CurrentCultureIgnoreCase)) {
                if (double.IsNaN(aResult)) {
                    this.SetOutput(theCust, "Result", null);
                    return null;
                }

                if (aInputs != null) {
                    int aInputCount = aInputs.Values.Count;
                    if (aInputCount == 0) {
                        if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                            aResult /= aDefaultSecond.Value;
                        }
                    } else {
                        bool aIsAllNull = true;
                        foreach (Numeric aEachInput in aInputs.Values) {
                            double aEachInputValue = aEachInput.Value;
                            if (double.IsNaN(aEachInputValue)) {
                                aInputCount--;
                                aEachInputValue = 1;
                            } else {
                                aIsAllNull = false;
                            }
                            aResult /= aEachInputValue;
                        }

                        if (true == aIsAllNull) {
                            aResult = double.NaN;
                        }
                    }
                } else if (aDefaultSecond != null) {
                    if (double.IsNaN(aDefaultSecond.Value)) {
                        aDefaultSecond.Value = 1;
                    }
                    aResult /= aDefaultSecond.Value;
                }
            } else if (aOperation.Equals("%", StringComparison.CurrentCultureIgnoreCase)) {
                List<double> aInputList = new List<double>();
                int aCount = 0;
                if (!double.IsNaN(aResult)) {
                    aInputList.Add(aResult);
                    aCount++;
                }

                if (aInputs != null) {
                    int aInputCount = aInputs.Values.Count;

                    foreach (Numeric aEachInput in aInputs.Values) {
                        if (double.IsNaN(aEachInput.Value)) {
                            aInputCount--;
                        }

                        if (!double.IsNaN(aEachInput.Value)) {
                            aInputList.Add(aEachInput.Value);
                            aCount++;
                        }
                    }

                    if (aInputCount == 0) {
                        if (aDefaultSecond != null && !double.IsNaN(aDefaultSecond.Value)) {
                            aInputList.Add(aDefaultSecond.Value);
                            aCount++;
                        }
                    }
                } else if (aDefaultSecond != null) {
                    if (!double.IsNaN(aDefaultSecond.Value))
                    {
                        aInputList.Add(aDefaultSecond.Value);
                        aCount++;
                    }
                }

                if (aInputList.Count != 0) {
                    List<double> aGL = this.GetModeList(aInputList);
                    GenericList aOutputList = new GenericList(this);

                    if (aGL.Count != 0) {
                        for (int i = 0; i < aGL.Count; i++) {
                            aOutputList.AddRow(new string[] { aGL.ElementAt(i).ToString() });
                        }
                        this.SetOutput(theCust, "ModeResult", aOutputList);
                    }
                }
                aResult = double.NaN;
            }

            this.SetOutput(theCust, "Result", new Numeric(this, aResult));
            return null;
        }

        protected override void InitInputInfo() {
            this.AddInputInfo("FirstInput", Numeric.NumericInfoOptional);
            this.AddInputInfo("SubsequentInputs", IOTypeList.IOTypeListInfoOptional);
            this.AddInputInitData("SubsequentInputs", typeof(Numeric));
        }

        protected override void InitOutputInfo() {
            this.AddOutputInfo("Result", Numeric.NumericInfoOptional);

            this.AddOutputInfo("ModeResult", GenericList.GenericListInfoOptional);
            GLInitInfo aGL = new GLInitInfo();
            aGL.AddColumnHeaderAndType("ModeResult", typeof(string), true, false);
            this.AddOutputInitData("ModeResult", aGL);
        }

        protected override void InitPropInfo() {
            this.AddPropInfo("DefaultFirstInput", Numeric.NumericInfoOptional);
            this.AddPropInfo("DefaultSecondInput", Numeric.NumericInfoOptional);
            this.AddPropInfo("Operation", Text.TextDropDownInfo);
            this.AddPropInitData("Operation", OPERATOR);
        }

        public override string GetDesignHelp() => "CustomArithmetic";

        public override string GetDesignName() => "CustomArithmetic";

        private List<double> GetModeList(List<double> theList) {
            List<double> aList = new List<double>();

            theList.Sort();

            double[] aArrayTick = new double[theList.Count];
            for (int i = 0; i < theList.Count; i++) {
                aArrayTick[i] = 0;
            }

            int aDiffDataCount = 0;
            List<double> aDataList = new List<double>();
            for (int i = 0; i < theList.Count; i++) {
                double aTemp = theList.ElementAt(i);
                int aIndex = aDataList.IndexOf(aTemp);

                if (aIndex == -1) {
                    aDataList.Add(aTemp);
                    aArrayTick[aDiffDataCount++]++;
                } else {
                    aArrayTick[aIndex]++;
                }
            }

            double aMaxCount = aArrayTick[0];
            for (int i = 0; i < aDataList.Count; i++) {
                aMaxCount = (aMaxCount >= aArrayTick[i]) ? aMaxCount : aArrayTick[i];
            }

            for (int i = 0; i < aDataList.Count; i++) {
                if (aMaxCount == aArrayTick[i]) {
                    aList.Add(aDataList.ElementAt(i));
                }
            }
            return aList;
        }
    }
}