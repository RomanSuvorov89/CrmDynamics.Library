using CrmDynamics.Library.Extensions;
using CrmDynamics.Library.Models.Abstractions;
using CrmDynamics.Library.Models.Extensions;
using CrmDynamics.Library.Models.Query.Requests;
using CrmDynamics.Library.Models.Query.Requests.Abstractions;
using CrmDynamics.Library.Models.Query.Responses;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Utf8Json;

namespace CrmDynamics.Library.Workers.Web
{
    public class WebProxy
    {
        private readonly string _endpoint;
        private readonly NetworkCredential _credential;
        private readonly ICrmCache _crmCache;

        public WebProxy(string endpoint, NetworkCredential credential, ICrmCache crmCache)
        {
            _endpoint = endpoint;
            _credential = credential;
            _crmCache = crmCache;
        }

        public T GetResponse<T>(string requestUrl, string requestMethod, object data = null)
        {
            var response = GetResponse(requestUrl, requestMethod, data);
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(json);
        }

        public HttpResponseMessage GetResponse(string requestUrl, string requestMethod, object data = null)
        {
            var request = new HttpRequestMessage(new HttpMethod(requestMethod), _endpoint + requestUrl);

            if (data != null)
            {
                request.Content = new StringContent(JsonSerializer.ToJsonString(data), Encoding.UTF8, "application/json");
            }

            var httpClient = new HttpClient(new HttpClientHandler { Credentials = _credential ?? CredentialCache.DefaultNetworkCredentials });
            var response = httpClient.SendAsync(request)?.Result;

            if (response == null)
                throw new Exception("Сервер вернул пустой ответ");

            if (!response.IsSuccessStatusCode)
            {
                var exception = JsonSerializer.Deserialize<CrmException>(response.Content.ReadAsStringAsync().Result);
                throw new CrmException(exception.Error.Message, exception);
            }

            return response;
        }

        public ExecuteTransactionResponse GetTransactionResponse(ExecuteTransactionRequest request)
        {
            var requestDictionary = new Dictionary<int, string>();

            var batchid = "batch_" + Guid.NewGuid().ToString();
            var batchContent = new MultipartContent("mixed", batchid);
            var changesetID = "changeset_" + Guid.NewGuid().ToString();
            var changeSetContent = new MultipartContent("mixed", changesetID);

            for (int contentId = 1; contentId <= request.Requests.Count; contentId++)
            {
                HttpMessageContent content = new HttpMessageContent(GetRequestMessage((OrganizationRequest)request.Requests[contentId - 1]));
                content.Headers.Remove("Content-Type");
                content.Headers.TryAddWithoutValidation("Content-Type", "application/http");
                content.Headers.TryAddWithoutValidation("Content-Transfer-Encoding", "binary");
                content.Headers.TryAddWithoutValidation("Content-ID", contentId.ToString());
                changeSetContent.Add(content);
                requestDictionary.Add(contentId, ((OrganizationRequest)request.Requests[contentId - 1]).RequestName);
            }

            batchContent.Add(changeSetContent);

            var batchRequest = new HttpRequestMessage(HttpMethod.Post, _endpoint + "$batch")
            {
                Content = batchContent
            };

            var batchstring = batchRequest.Content.ReadAsStringAsync();

            var httpClient = new HttpClient(new HttpClientHandler { Credentials = _credential ?? CredentialCache.DefaultNetworkCredentials });
            var response = httpClient.SendAsync(batchRequest)?.Result;

            if (response == null)
                throw new Exception("Сервер вернул пустой ответ");

            if (!response.IsSuccessStatusCode)
            {
                var exception = JsonSerializer.Deserialize<CrmException>(response.Content.ReadAsStringAsync().Result);
                throw new CrmException(exception.Error.Message, exception);
            }

            var responseString = response.Content.ReadAsStringAsync();
            MultipartMemoryStreamProvider batchStream = response.Content.ReadAsMultipartAsync().Result;
            var batchStreamContent = batchStream.Contents.FirstOrDefault();

            MultipartMemoryStreamProvider changesetStream = batchStreamContent.ReadAsMultipartAsync().Result;

            var transactionResponse = new ExecuteTransactionResponse();

            foreach (var changesetContent in changesetStream.Contents)
            {
                changesetContent.Headers.Remove("Content-Type");
                changesetContent.Headers.Add("Content-Type", "application/http; msgtype=response");

                var indivdualResponse = changesetContent.ReadAsHttpResponseMessageAsync().Result;

                if (!indivdualResponse.IsSuccessStatusCode)
                {
                    var exception = JsonSerializer.Deserialize<CrmException>(response.Content.ReadAsStringAsync().Result);
                    throw new CrmException(exception.Error.Message, exception);
                }

                var operationName = requestDictionary.FirstOrDefault(dic => dic.Key == int.Parse(changesetContent.Headers.GetValues("Content-ID").FirstOrDefault())).Value;

                if (operationName == Constants.CREATE)
                {
                    var idString = indivdualResponse.Headers.GetValues("OData-EntityId").FirstOrDefault();
                    idString = idString.Replace(_endpoint, "").Replace("(", "").Replace(")", "");
                    idString = idString.Substring(idString.Length - 36);

                    var createResponse = new CreateResponse { Id = Guid.Parse(idString), ResponseName = operationName };
                    transactionResponse.Responses.Add(createResponse);
                }

                if (operationName == Constants.UPDATE)
                {
                    var updateResponse = new UpdateResponse { ResponseName = operationName };
                    transactionResponse.Responses.Add(updateResponse);
                }

                if (operationName == Constants.DELETE)
                {
                    var deleteResponse = new DeleteResponse { ResponseName = operationName };
                    transactionResponse.Responses.Add(deleteResponse);
                }

                if (operationName == Constants.DISASSOCIATE)
                {
                    var deleteResponse = new DissacociateResponse { ResponseName = operationName };
                    transactionResponse.Responses.Add(deleteResponse);
                }

                if (operationName == Constants.ASSOCIATE)
                {
                    var deleteResponse = new AssociateResponse { ResponseName = operationName };
                    transactionResponse.Responses.Add(deleteResponse);
                }
            }

            return transactionResponse;
        }

        private HttpRequestMessage GetRequestMessage(OrganizationRequest orgRequest)
        {
            if (orgRequest.RequestName == Constants.CREATE)
            {
                var entity = (orgRequest as CreateRequest).Target;
                var url = _endpoint + _crmCache.GetEntityDefinitionSchemaName(entity.LogicalName);
                var request = new HttpRequestMessage(new HttpMethod("POST"), url);
                request.Content = new StringContent(JsonSerializer.ToJsonString(entity.ConvertToExpandoObject(_crmCache)), Encoding.UTF8, "application/json");

                return request;
            }

            if (orgRequest.RequestName == Constants.UPDATE)
            {
                var entity = (orgRequest as UpdateRequest).Target;
                var url = _endpoint + _crmCache.GetEntityDefinitionSchemaName(entity.LogicalName);
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url + $"({entity.Id})");
                request.Content = new StringContent(JsonSerializer.ToJsonString(entity.ConvertToExpandoObject(_crmCache)), Encoding.UTF8, "application/json");

                return request;
            }

            if (orgRequest.RequestName == Constants.DELETE)
            {
                var entity = (orgRequest as DeleteRequest).Target;
                var url = _endpoint + _crmCache.GetEntityDefinitionSchemaName(entity.LogicalName);
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), url + $"({entity.Id})");

                return request;
            }

            if (orgRequest.RequestName == Constants.DISASSOCIATE)
            {
                var disReq = orgRequest as DisassociateRequest;
                var url = _endpoint + _crmCache.GetEntityDefinitionSchemaName(disReq.Target.LogicalName);

                if (disReq.IsCollectValueReference)
                {
                    return new HttpRequestMessage(new HttpMethod("DELETE"), url + $"({disReq.Target.Id})/{disReq.NavigationProperty}({disReq.OtherEntityId})/$ref");
                }

                return new HttpRequestMessage(new HttpMethod("DELETE"), url + $"({disReq.Target.Id})/{disReq.NavigationProperty}/$ref");
            }

            if (orgRequest.RequestName == Constants.ASSOCIATE)
            {
                //var realationshopNavigationPropertyName = _crmCache.GetReferencedEntityNavigationPropertyName(disReq.Target.LogicalName, disReq.AttributeKey); 
                var assReq = (orgRequest as AssociateRequest);
                var url = _endpoint + _crmCache.GetEntityDefinitionSchemaName(assReq.Target.LogicalName);
                var request = new HttpRequestMessage(new HttpMethod("PUT"), url + $"({assReq.Target.Id})/{assReq.NavigationProperty}/$ref");

                IDictionary<string, object> expandoObject = new ExpandoObject();
                expandoObject.Add("@odata.id", _endpoint + _crmCache.GetEntityDefinitionSchemaName(assReq.OtherEntity.LogicalName) + $"({assReq.OtherEntity.Id})");
                request.Content = new StringContent(JsonSerializer.ToJsonString(expandoObject), Encoding.UTF8, "application/json");

                return request;
            }

            throw new Exception("Unknown request type");
        }
    }
}
