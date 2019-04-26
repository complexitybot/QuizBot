using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace QuizRequestService
{
    public class Requester : IQuizService
    {
        private readonly string serverUri;

        public Requester(string serverUri)
        {
            this.serverUri = serverUri;
        }

        public IEnumerable<TopicDTO> GetTopics()
        {
            var client = new RestClient(serverUri + "/api/topics");
            var content = SendGetRequest(client, Method.GET);
            var topics = JsonConvert.DeserializeObject<List<TopicDTO>>(content.Content);
            return topics;
        }

        public IEnumerable<LevelDTO> GetLevels(Guid topicId)
        {
            var client = new RestClient(serverUri + $"/api/{topicId}/levels");
            var content = SendGetRequest(client, Method.GET);
            var levels = JsonConvert.DeserializeObject<List<LevelDTO>>(content.Content);
            return levels;
        }

        public IEnumerable<LevelDTO> GetAvailableLevels(Guid userId, Guid topicId)
        {
            var client = new RestClient(serverUri + $"/api/{userId}/{topicId}/availableLevels");
            var request = SendGetRequest(client, Method.GET);
            if (request.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<LevelDTO>>(request.Content);
            }
            return null;
        }

        public string GetCurrentProgress(Guid userId, Guid topicId, Guid levelId)
        {
            var client = new RestClient(serverUri + $"/api/{userId}/{topicId}/{levelId}/currentProgress");
            return SendGetRequest(client, Method.GET).Content;
        }

        public TaskDTO GetTaskInfo(Guid userId, Guid topicId, Guid levelId)
        {
            var client = new RestClient(serverUri + $"/api/{userId}/{topicId}/{levelId}/task");
            var content = SendGetRequest(client, Method.GET);
            if (content.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<TaskDTO>(content.Content);
            return null;
        }

        public TaskDTO GetNextTaskInfo(Guid userId)
        {
            var client = new RestClient(serverUri + $"/api/{userId}/nextTask");
            var content = SendGetRequest(client, Method.GET);
            if (content.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<TaskDTO>(content.Content);
            return null;
        }

        public string GetHint(Guid userId)
        {
            var client = new RestClient(serverUri + $"/api/{userId}/hint");
            var request = SendGetRequest(client, Method.GET);
            if (request.StatusCode == HttpStatusCode.OK)
            {
                return request.Content;
            }
            return null;
        }

        public bool? SendAnswer(Guid userId, string answer)
        {
            var client = new RestClient(serverUri + $"/api/{userId}/sendAnswer");
            var parameter = new Parameter("application/json", $"\"{answer}\"", ParameterType.RequestBody);
            var content = SendGetRequest(client, Method.POST, parameter).Content;
            var result = bool.TryParse(content, out var isParsed);
            if (isParsed)
                return result;
            return null;
        }

        private IRestResponse SendGetRequest(IRestClient client, Method method, Parameter parameter = null)
        {
            var request = new RestRequest(method);
            if (parameter != null)
                request.AddParameter(parameter);
            var response = client.Execute(request);
            return response;
        }
    }
}