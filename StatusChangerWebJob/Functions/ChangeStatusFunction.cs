using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MVCProject2.Entities;
using MVCProject2.Models;
using System.Text.Json;

namespace StatusChangerWebJob.Functions
{
    public class ChangeStatusFunction
    {
        public static async Task ProcessQueueMessageAsync([QueueTrigger("status-queue")] User user, ILogger logger)
        {
            logger.LogInformation(JsonSerializer.Serialize(user));
            user.Status = StatusCodes.Completed;
            user.ModDate = DateTime.Now;
            user.Code = Guid.NewGuid().ToString();

            using (HttpClient httpClient = new HttpClient())
            {
                string baseUrl = "https://localhost:7039/Users/Update";
                var content = new StringContent(JsonSerializer.Serialize(user));
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = await httpClient.PutAsync(baseUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("PUT request successful");
                }
                else
                {
                    Console.WriteLine("PUT request failed with status code: " + response.StatusCode);
                    Console.WriteLine("good lord");
                }
            }

        }
    }
}
