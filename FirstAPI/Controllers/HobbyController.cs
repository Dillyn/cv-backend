using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("/hobby")] //set route
    [ApiController] // Bring in apicontroller
    public class HobbyController : ControllerBase //1 inherit controllerbase
    {
        private readonly ApplicationDBcontext _context;
        public HobbyController(ApplicationDBcontext context) { _context = context; }

        //private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "mock_db.json");

        [HttpGet]
        public IActionResult GetAll()
        {
            var hobby = _context.hobby.ToList();
            //var users = LoadUsersFromFile();
            return Ok(hobby);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var hobby = _context.hobby.Find(id);

            if (hobby == null) { return NotFound(); }
            return Ok(hobby);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Hobby hobby)
        {
            _context.hobby.Add(hobby);
            _context.SaveChanges();
            return Ok(hobby);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Hobby hobby)
        {
            Console.WriteLine("update called");
            var existingHobby = _context.hobby.Find(id);
            if (existingHobby == null)
            {
                return NotFound("Hobby not found"); // Hobby with the given ID doesn't exist
            }

            // Update properties dynamically

            // Update properties if provided
            existingHobby.Title = UpdateHobbyProperty(existingHobby.Title, hobby.Title);
            existingHobby.Content = UpdateHobbyProperty(existingHobby.Content, hobby.Content);
            existingHobby.Image = UpdateHobbyProperty(existingHobby.Image, hobby.Image);

            _context.SaveChanges();

            return Ok(hobby);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            Hobby hobby = _context.hobby.Find(id);
            if (hobby == null)
            {
                return NotFound("Record was not found"); // Hobby with the given ID doesn't exist
            }

            _context.Remove(hobby); // Marks the hobby for deletion in the in-memory context
            _context.SaveChanges(); // Commits the changes, including the deletion, to the actual database (localdb)


            return Ok(new { Message = $"Record with ID {id} was successfully deleted.", Hobby = hobby });
        }
        //Dynamic function for mapiing over properties of put
        private string UpdateHobbyProperty(string existingProperty, string newProperty)
        {
            //if new property is not null or empty return the new value
            if (!string.IsNullOrEmpty(newProperty))
            {
                return newProperty;
            }
            return existingProperty;
        }

    }



}
