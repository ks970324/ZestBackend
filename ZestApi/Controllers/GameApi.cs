using Microsoft.AspNetCore.Mvc;

namespace ZestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static string CurrentRound = "";

        [HttpPost("newgame")]
        public IActionResult CreateGame()
        {
            if (string.IsNullOrEmpty(CurrentRound))
            {
                var random = new Random();
                CurrentRound = random.Next(0, 2) == 0 ? "blue" : "red";
            }

            return Ok(new CurrentRoundResponse { CurrentRound = CurrentRound });

        }

        [HttpPost("rounds/next")]
        public IActionResult NextRound()
        {
            if (!string.IsNullOrEmpty(CurrentRound))
            {
                CurrentRound = CurrentRound == "blue" ? "red" : "blue";
            }

            return Ok(new CurrentRoundResponse { CurrentRound = CurrentRound });
        }

        private static Point RedPosition = new Point { X = 1, Y = 1 };
        private static Point BluePosition = new Point { X = 1, Y = 1 };

        [HttpPost("getbluepos")]
        public IActionResult UpdateBluePosition([FromBody] PositionRequest request)
        {
            BluePosition = request.BluePosition;
            RedPosition = request.RedPosition;

            return Ok(new PositionResponse
            {
                RedPosition = RedPosition,
                BluePosition = BluePosition
            });
        }

        [HttpPost("getredpos")]
        public IActionResult GetRedPosition([FromBody] PositionRequest request)
        {
            BluePosition = request.BluePosition;
            var random = new Random();
            RedPosition = new Point { X = random.Next(0, 3), Y = random.Next(0, 3) };

            return Ok(new PositionResponse
            {
                RedPosition = RedPosition,
                BluePosition = BluePosition
            });

        }

        [HttpPost("attack")]
        public IActionResult Attack([FromBody] AttackRequest request)
        {
            bool Hit = false;
            
            if (request.CurrentRound == "red" && request.RedPosition.Y == request.BluePosition.Y)
            {
                Hit = true;
                CurrentRound = "blue";
            }
            
            else if (request.CurrentRound == "blue" && request.BluePosition.Y == request.RedPosition.Y)
            {
                Hit = true;
                CurrentRound = "red";
            }
            
            return Ok(new AttackResponse
            {
                CurrentRound = CurrentRound,
                BluePosition = BluePosition,
                RedPosition = RedPosition,
                Hit = Hit
            });
            
            
        }
    }


    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    
        
    public class PositionResponse
    {
        public Point RedPosition { get; set; }
        public Point BluePosition { get; set; }
    }

    public class PositionRequest
    {
        public Point RedPosition { get; set; }
        public Point BluePosition { get; set; } 
    }

    public class CurrentRoundResponse
    {
        public string CurrentRound { get; set; }

    }
    
    public class AttackRequest
    {
        public Point RedPosition { get; set; }
        public Point BluePosition { get; set; }
        public string CurrentRound { get; set; }
    }

    public class AttackResponse
    {
        public string CurrentRound { get; set; }
        public Point BluePosition { get; set; }
        public Point RedPosition { get; set; }
        public bool Hit { get; set; }
    }

}