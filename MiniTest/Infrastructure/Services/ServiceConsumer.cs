using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiniProject.Infrastructure.Services
{
    public class ServiceConsumer<T> : IServiceConsumer<T>
        where T : class, IModel
    {
        private readonly string _endpointAddress;

        public ServiceConsumer(string endpointAddress)
        {
            if (string.IsNullOrEmpty(endpointAddress))
                throw new Exception($"Endpoint address for {nameof(ServiceConsumer<T>)} was not defined");

            _endpointAddress = endpointAddress;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (Uri.TryCreate(_endpointAddress, UriKind.Absolute, out var endpointAddress))
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(endpointAddress);
                string apiResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(apiResponse);
            }
            throw new Exception($"Endpoint {_endpointAddress} is not valid");
        }
    }
}
