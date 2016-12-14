using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Models.Common;

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

        private static T GetEntry<T> (Dictionary<Guid, T> dict, Guid id) where T : class
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
                _countryDict.Add(row.Id, row);
            }
        }

        private static void LoadStates()
        {
            _stateDict = new Dictionary<Guid, State>();
            foreach (var row in CommonContext.States)
            {
                _stateDict.Add(row.Id, row);
            }
        }

        private static void LoadCounties()
        {
            _countyDict = new Dictionary<Guid, County>();
            foreach (var row in CommonContext.Counties)
            {
                _countyDict.Add(row.Id, row);
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

        public static State GetState (Guid? id)
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

        public static County GetCounty (Guid? id)
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
