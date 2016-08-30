//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using OpsLogix.WAP.RunPowerShell.ApiClient.DataContracts;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace OpsLogix.WAP.RunPowerShell.Api
{
    /// <summary>
    /// Generic Utitlities
    /// </summary>
    internal class Utility
    {
        /// <summary>
        /// This is getting used to return error response. This can be used for methods not returning any specific object
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="errorResourceCode"></param>
        /// <returns></returns>
        internal static HttpResponseMessage SendResponseException(HttpRequestMessage request, HttpStatusCode statusCode, string message, string errorResourceCode = null)
        {
            return request.CreateResponse<RunPowerShellErrorResource>(statusCode, new RunPowerShellErrorResource()
            {
                Code = errorResourceCode,
                Message = message
            });  
        }

        /// <summary>
        /// This method is used to throw exceptions
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="errorResourceCode"></param>
        /// <returns></returns>
        internal static HttpResponseException ThrowResponseException(HttpRequestMessage request, HttpStatusCode statusCode, string message, string errorResourceCode = null)
        {
            return new HttpResponseException(
             new HttpResponseMessage(statusCode)
             {
                 Content = new ObjectContent<RunPowerShellErrorResource>(
                             new RunPowerShellErrorResource()
                             {
                                 Code = errorResourceCode,
                                 Message = message,
                             },
                             new XmlMediaTypeFormatter())
             });                   
        }
       
    }
}