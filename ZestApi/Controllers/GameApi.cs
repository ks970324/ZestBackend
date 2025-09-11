using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace ZestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static string CurrentRound = "";
        private static string Attacker = "";
        private static string GameResult = "";
        
        private static Point RedPosition = new Point { X = 1, Y = 1 };
        private static Point BluePosition = new Point { X = 1, Y = 1 };

        private static int RedHP = 10;
        private static int BlueHP = 10;

        [HttpPost("newgame")]
        public IActionResult CreateGame()
        {
                var random = new Random();
                CurrentRound = random.Next(0, 2) == 0 ? "blue" : "red";
                RedPosition = new Point { X = 1, Y = 1 };
                BluePosition = new Point { X = 1, Y = 1 };
                RedHP = 10;
                BlueHP = 10;
                Attacker = CurrentRound;
                GameResult = "";
                
            

            return Ok(new NewGameResponse
            {
                CurrentRound = CurrentRound,
                RedPosition = RedPosition,
                BluePosition = BluePosition,
                Attacker = Attacker
            });

        }

        [HttpPost("getnextround")]
        public IActionResult NextRound()
        {
            if (!string.IsNullOrEmpty(CurrentRound))
            {
                CurrentRound = CurrentRound == "blue" ? "red" : "blue";
            }

            return Ok(new CurrentRoundResponse { CurrentRound = CurrentRound });
        }

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

        [HttpPost("blueattack")]
        public IActionResult BlueAttack([FromBody] AttackRequest request)
        {
            bool Hit = false;
            var random = new Random();
            RedPosition = new Point { X = random.Next(0, 3), Y = random.Next(0, 3) };
            CurrentRound = "red";
            Attacker = "blue";
            
            if (request.CurrentRound == "blue" && RedPosition.Y == request.BluePosition.Y)
            {
                Hit = true;
                int damage = Math.Abs(RedPosition.X+3 - request.BluePosition.X);
                RedHP -= damage switch
                {
                    4 or 5 => 1, 
                    3 => 2,
                    1 or 2 => 3,
                    _ => 0        
                };
            }

            if (RedHP <= 0)
            {
                GameResult = "blue";
            }
            
            
            return Ok(new AttackResponse
            {
                CurrentRound = CurrentRound,
                BluePosition = BluePosition,
                RedPosition = RedPosition,
                Hit = Hit,
                BlueHP = BlueHP,
                RedHP = RedHP,
                GameResult = GameResult,
                Attacker = Attacker
            });
            
            
        }

        [HttpPost("redattack")]
        public IActionResult RedAttack([FromBody] AttackRequest request)
        {
            bool Hit = false;
            var random = new Random();
            RedPosition = new Point { X = random.Next(0, 3), Y = random.Next(0, 3) };
            CurrentRound = "blue";
            Attacker = "red";

            if (request.CurrentRound == "red" && RedPosition.Y == request.BluePosition.Y)
            {
                Hit = true;
                int damage = Math.Abs(RedPosition.X+3 - request.BluePosition.X);
                BlueHP -= damage switch
                {
                    4 or 5 => 1, 
                    3 => 2,
                    1 or 2 => 3,
                    _ => 0        
                };
                
            }
            
            if (BlueHP <= 0)
            {
                GameResult = "red";
            }
            
            
            return Ok(new AttackResponse
            {
                CurrentRound = CurrentRound,
                BluePosition = BluePosition,
                RedPosition = RedPosition,
                Hit = Hit,
                BlueHP = BlueHP,
                RedHP = RedHP,
                GameResult = GameResult,
                Attacker = Attacker
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
    
    public class NewGameResponse
    {
        public string CurrentRound { get; set; }
        public string Attacker { get; set; }
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
        public string Attacker { get; set; }
        public Point BluePosition { get; set; }
        public Point RedPosition { get; set; }
        public bool Hit { get; set; }
        
        public int BlueHP { get; set; }
        public int RedHP { get; set; }
        public string GameResult { get; set; }
    }

}