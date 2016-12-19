﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Data.Enums;
using DDI.Shared;
using DDI.Shared.Logger;

namespace DDI.Business.Services
{
    public class ServiceBase
    {
        private static readonly Logger _logger = Logger.GetLogger(typeof(ServiceBase));

        public IDataResponse<T> GetIDataResponse<T>(Func<T> funcToExecute)
        {
            return GetDataResponse(funcToExecute);
        }

        public DataResponse<T> GetDataResponse<T>(Func<T> funcToExecute)
        {
            try
            {
                var result = funcToExecute();
                return new DataResponse<T>
                {
                    Data = result,
                    IsSuccessful = true
                };
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                var response = new DataResponse<T> { IsSuccessful = false };
                response.ErrorMessages.Add(e.Message);
                response.VerboseErrorMessages.Add(e.ToString());
                return response;
            }
        }

        public DataResponse GetDataResponse(Action actionToExecute)
        {
            DataResponse response;

            try
            {
                actionToExecute();
                response = new DataResponse
                {
                    IsSuccessful = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                response = new DataResponse { IsSuccessful = false };
                response.ErrorMessages.Add(ex.Message);
                response.VerboseErrorMessages.Add(ex.ToString());
            }

            return response;
        }

        public DataResponse<T> GetErrorResponse<T>(string errorMessage, string verboseErrorMessage = null)
        {
            _logger.Error($"Message: {errorMessage} | Verbose Message: {verboseErrorMessage}");

            return (verboseErrorMessage == null)
                ? GetErrorResponse<T>(new List<string> { errorMessage })
                : GetErrorResponse<T>(new List<string> { errorMessage }, new List<string> { verboseErrorMessage });
        }

        public DataResponse<T> GetErrorResponse<T>(IEnumerable<string> errorMessages, IEnumerable<string> verboseErrorMessages = null)
        {
            return new DataResponse<T>
            {
                IsSuccessful = false,
                Data = default(T),
                ErrorMessages = errorMessages?.ToList(),
                VerboseErrorMessages = verboseErrorMessages?.ToList()
            };
        }
    }
}