using System;
using System.Collections.Generic;
using System.Linq;
using MementoSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApplication.Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly Example _ab;

        public TestController(Example ab)
        {
            _ab = ab;
        }
        
        [HttpGet]
        public void Test()
        {
            Console.WriteLine(MementoExtension.SavedObjectsCount()); 

            
            var x = new Random().Next(100, 1000);
            _ab.Keypress = "i am changed from request -- " + x.ToString();
            _ab.SaveState();

            
            if (_ab.StatesCount() > 0)
            {
                var k = _ab.RestoreState(0);
                Console.WriteLine(k.Keypress);
            }

        
            Console.WriteLine(_ab.StatesCount());
        }
    }

    
    public class Example
    {
        public string Keypress { get; set; }
    }
}