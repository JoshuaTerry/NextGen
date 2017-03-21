using DDI.Shared.Attributes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.GL
{
    /// <summary>
    /// An EF complex type consisting of a set of 14 amounts, one for each fiscal period.
    /// </summary>
    [ComplexType]
    public class PeriodAmountList
    {
        private const int MAX_PERIODS = 14;
        // JLT - I understand now that these are ending balances for a period
        // When we look these up for some previous year, wouldn't we also need 
        // to cross reference the periods that were assigned to that year as its
        // possible that they have changed since then??
        [DecimalPrecision(14, 2)]
        public decimal Amount01 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount02 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount03 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount04 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount05 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount06 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount07 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount08 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount09 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount10 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount11 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount12 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount13 { get; set; }
        [DecimalPrecision(14, 2)]
        public decimal Amount14 { get; set; }

        // JLT - I'm not sure I understand what all this is for or how it fits into 
        // EF, but it looks like the following is to establish an Indexer?  
        // If thats the case can we just do the following?

        //private decimal[] amounts = new decimal[14];
        //public decimal this[int index]
        //{
        //    get { return amounts[index]; }
        //    set { amounts[index] = value; }
        //}

        [NotMapped]
        private IEnumerable<decimal> Enumerable
        {
            get
            {
                yield return Amount01;
                yield return Amount02;
                yield return Amount03;
                yield return Amount04;
                yield return Amount05;
                yield return Amount06;
                yield return Amount07;
                yield return Amount08;
                yield return Amount09;
                yield return Amount10;
                yield return Amount11;
                yield return Amount12;
                yield return Amount13;
                yield return Amount14;
            }
        }

        private decimal GetPeriodAmount(int index)
        {
            switch (index)
            {
                case 0: return Amount01;
                case 1: return Amount02;
                case 2: return Amount03;
                case 3: return Amount04;
                case 4: return Amount05;
                case 5: return Amount06;
                case 6: return Amount07;
                case 7: return Amount08;
                case 8: return Amount09;
                case 9: return Amount10;
                case 10: return Amount11;
                case 11: return Amount12;
                case 12: return Amount13;
                case 13: return Amount14;
            }
            throw new InvalidOperationException($"The index must be between 0 and {MAX_PERIODS - 1}.");
        }

        private void SetPeriodAmount(int index, decimal value)
        {
            switch (index)
            {
                case 0: Amount01 = value; break;
                case 1: Amount02 = value; break;
                case 2: Amount03 = value; break;
                case 3: Amount04 = value; break;
                case 4: Amount05 = value; break;
                case 5: Amount06 = value; break;
                case 6: Amount07 = value; break;
                case 7: Amount08 = value; break;
                case 8: Amount09 = value; break;
                case 9: Amount10 = value; break;
                case 10: Amount11 = value; break;
                case 11: Amount12 = value; break;
                case 12: Amount13 = value; break;
                case 13: Amount14 = value; break;
                default: throw new InvalidOperationException($"The index must be between 0 and {MAX_PERIODS - 1}.");
            }
        }

        private FixedSizeList<decimal> _periodAmounts = null;
        [NotMapped]
        public FixedSizeList<decimal> Amounts
        {
            get
            {
                if (_periodAmounts == null)
                {
                    _periodAmounts = new FixedSizeList<decimal>(this.Enumerable, this.GetPeriodAmount, this.SetPeriodAmount, MAX_PERIODS);
                }
                return _periodAmounts;
            }

        }
    }
}
