﻿using DDI.Shared.Models.Common;
using System;
using System.Collections.Generic;

namespace DDI.Data
{
    /// <summary>
    /// Provides cached country, state, and county values.
    /// </summary>
    public static class CommonDataCache
    {
        #region Private fields and properties

        private static Dictionary<Guid, Country> _countryDict = null;
        private static Dictionary<Guid, State> _stateDict = null;
        private static Dictionary<Guid, County> _countyDict = null;

        private static CommonContext _context = null;
        private static CommonContext CommonContext
        {
            get
            {
                if (_context == null)
                {
                    _context = new CommonContext();
                }
                return _context;
            }
        }

        #endregion

        #region Private Methods

        private static T GetEntry<T>(Dictionary<Guid, T> dict, Guid id) where T : class
        {
            T obj;
            if (dict.TryGetValue(id, out obj))
            {
                return obj;
            }

            return null;

        }

        private static void LoadCountries()
        {
            _countryDict = new Dictionary<Guid, Country>();
            foreach (var row in CommonContext.Countries)
            {
                Country country = new Models.Common.Country()
                {
                    AddressFormat = row.AddressFormat,
                    CallingCode = row.CallingCode,
                    CountryCode = row.CountryCode,
                    Description = row.Description,
                    Id = row.Id,
                    InternationalPrefix = row.InternationalPrefix,
                    ISOCode = row.ISOCode,
                    PhoneFormat = row.PhoneFormat,
                    PostalCodeFormat = row.PostalCodeFormat,
                    StateAbbreviation = row.StateAbbreviation,
                    StateName = row.StateName,
                    TrunkPrefix = row.TrunkPrefix
                };
                _countryDict.Add(row.Id, country);
            }
        }

        private static void LoadStates()
        {
            _stateDict = new Dictionary<Guid, State>();
            foreach (var row in CommonContext.States)
            {
                State state = new Models.Common.State()
                {
                    CountryId = row.CountryId,
                    Description = row.Description,
                    FIPSCode = row.FIPSCode,
                    Id = row.Id,
                    StateCode = row.StateCode
                };
                _stateDict.Add(row.Id, state);
            }
        }

        private static void LoadCounties()
        {
            _countyDict = new Dictionary<Guid, County>();
            foreach (var row in CommonContext.Counties)
            {
                County county = new County()
                {
                    Description = row.Description,
                    FIPSCode = row.FIPSCode,
                    Id = row.Id,
                    Population = row.Population,
                    PopulationPercentageChange = row.PopulationPercentageChange,
                    PopulationPerSqaureMile = row.PopulationPerSqaureMile,
                    StateId = row.StateId
                };

                _countyDict.Add(row.Id, county);
            }
        }

        #endregion

        #region Public Methods

        public static Country GetCountry(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            if (_countryDict == null)
            {
                LoadCountries();
            }

            return GetEntry(_countryDict, id.Value);
        }

        public static State GetState(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            if (_stateDict == null)
            {
                LoadStates();
            }
            return GetEntry(_stateDict, id.Value);
        }

        public static County GetCounty(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            if (_countyDict == null)
            {
                LoadCounties();
            }
            return GetEntry(_countyDict, id.Value);
        }

        #endregion

    }
}
