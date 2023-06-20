using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProject2.Entities;
using MVCProject2.Models;
using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MVCProject2.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDBContext _context;

        public UsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Users'  is null.");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View("UsersView");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]User user)
        {
           
            user ??= new User();
            if(string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)) return BadRequest();
            user.Created = DateTime.Now;
            _context.Add(user);
            await _context.SaveChangesAsync();


            InsertMessage("status-queue", user);

            return Ok(user);
        }
        private async void InsertMessage(string queueName, User user)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string connectionString = configuration.GetSection("ConnectionStrings:AzureConnection").Value;
            QueueClient queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64});

            user.Status = Models.StatusCodes.Processing;
            await _context.SaveChangesAsync();
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(JsonSerializer.Serialize(user));
            }

            Console.WriteLine($"Inserted: {user}");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {

            user ??= new User();
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)) return BadRequest();

            user.Created = DateTime.Now;
            User dbUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.ModDate = DateTime.Now;
            dbUser.Code = user.Code;
            dbUser.Status = user.Status;

            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
