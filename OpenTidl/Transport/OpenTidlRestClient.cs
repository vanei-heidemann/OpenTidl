﻿/*
    Copyright (C) 2015  Jack Fagner

    This file is part of OpenTidl.

    OpenTidl is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    OpenTidl is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with OpenTidl.  If not, see <http://www.gnu.org/licenses/>.

    --- 

    Modified 2019 J. Westlake
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTidl.Models.Base;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.IO;
using System.Runtime.Serialization.Json;
using OpenTidl.Models;

namespace OpenTidl.Transport
{
    public class OpenTidlRestClient : IRestClient
    {
        #region properties

        public const string TIMEOUT_HEADER = "X-Timeout";

        internal String ApiEndpoint { get; }
        internal List<(string, string)> Headers { get; }

        private INetworkClient _client { get; }

        private IOpenTidlSerializer _serializer { get; set; }

        #endregion


        #region methods


        public async Task<T> HandleAsync<T>(String path, Object query, Object request, String method, bool canCache = true, params (String, String)[] extraHeaders) where T : class
        {
            var response = await GetResponseAsync<T>(path, query, request, method, canCache, extraHeaders).ConfigureAwait(false);

            if (response.Exception != null)
                throw response.Exception;

            return response.Model;
        }

        public async Task<RestResponse<T>> GetResponseAsync<T>(String path, Object query, Object request, String method, bool canCache, params (String, String)[] extraHeaders) where T : class
        {
            var queryString = RestUtility.GetFormEncodedString(query);
            var url = String.IsNullOrEmpty(queryString) ? $"{ApiEndpoint}{path}" : 
                (path.Contains("?") ? $"{ApiEndpoint}{path}&{queryString}" : $"{ApiEndpoint}{path}?{queryString}");
            byte[] content = null;

            if (RestUtility.GetFormEncodedList(request) is var data && data != null)
            {
                using (var form = new FormUrlEncodedContent(data))
                {
                    content = await form.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
            }

            var headers = Headers.ToList();
            headers.AddRange(extraHeaders);

            using (var response = await _client.GetResponseStreamAsync(url, method, content, canCache, headers).ConfigureAwait(false))
            {
                if (response.IsSuccess)
                {
                    T model = default;
                    Exception exc = default;
                    if (response.StatusCode < 300)
                        model = _serializer.DeserializeObject<T>(response.Stream);

                    if (response.StatusCode >= 400)
                        exc = new OpenTidlException(_serializer.DeserializeObject<ErrorModel>(response.Stream));

                    return new RestResponse<T>(model, exc, response.StatusCode, response.ETag);
                }

                return new RestResponse<T>(response.Exception);
            }
        }

        public async Task<WebStreamModel> GetWebStreamModelAsync(String url)
        {
            try
            {
                var resp = await _client.GetResponseStreamAsync(url, "GET", null, false, new List<(string, string)> { (TIMEOUT_HEADER, "none") });
                return WebStreamModel.Create(resp);
            }
            catch
            {
                return null;
            }
        }

        public void SetSerializer(IOpenTidlSerializer serializer)
        {
            _serializer = serializer;
        }

        #endregion


        #region construction

        public OpenTidlRestClient(
            String apiEndpoint, 
            String userAgent, 
            INetworkClient clientOverride = null, 
            params KeyValuePair<String, String>[] headers)
        {
            this.ApiEndpoint = apiEndpoint ?? "";

            _client = clientOverride ?? new OpenTidlNetworkClient();
            Headers = new List<(string, string)>
            {
                ("User-Agent", userAgent),
                ("Accept-Encoding", "gzip, deflate")
            };

            if (headers != null)
            {
                foreach (var h in headers)
                    Headers.Add((h.Key, h.Value));
            }

            _serializer = new NewtonsoftSerializer();
        }

        #endregion
    }
}
