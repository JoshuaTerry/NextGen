﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DDI.Data;
using DDI.Data.Models.Client;

namespace DDI.Business.Domain
{
    /// <summary>
    /// Business domain class for constituents
    /// </summary>
    public class ConstituentDomain : BaseEntityDomain<Constituent>
    {
        /// <summary>
        /// Get the formatted name for a constituent.
        /// </summary>
        /// <param name="constituent"></param>
        /// <returns></returns>
        public string GetFormattedName(Constituent constituent)
        {
            return string.Join(" ", (new string[]
            {
                constituent.FirstName,
                constituent.MiddleName,
                constituent.LastName
            }).Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        /// <summary>
        /// Get the sort name for a constituent (e.g. Last First Middle)
        /// </summary>
        /// <param name="constituent"></param>
        /// <returns></returns>
        public string GetSortName(Constituent constituent)
        {
            return string.Join(" ", (new string[]
            {
                constituent.LastName,
                constituent.FirstName,
                constituent.MiddleName
            }).Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        public override void Validate(Constituent constituent)
        {
            base.Validate(constituent);

            // Get list of modified properties
            List<string> modifiedProperties = Repository.GetModifiedProperties(constituent);

            // Update constituent formatted name and sort name only if name fields were updated.
            if (modifiedProperties.Intersect(new string[]
            {
                nameof(Constituent.FirstName),
                nameof(Constituent.MiddleName),
                nameof(Constituent.LastName),
                nameof(Constituent.Suffix),
                nameof(Constituent.Nickname),
                nameof(Constituent.Prefix),
                nameof(Constituent.NameFormat)
            }).Count() > 0)
            {
                constituent.FormattedName = GetFormattedName(constituent);
                constituent.Name = GetSortName(constituent);
            }
        }

        public int GetNextConstituentNumber()
        {
            int nextNum = Repository.Utilities.GetNextSequenceValue(DomainContext.ConstituentNumberSequence);
            return nextNum;
        }

        public void SetNextConstituentNumber(int newValue)
        {
            Repository.Utilities.SetNextSequenceValue(DomainContext.ConstituentNumberSequence, newValue);
        }
    }
}