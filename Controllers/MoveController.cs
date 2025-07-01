using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Zest.Controllers;

[ApiController]
[Route("[controller]")]
public class ZestGameController : ControllerBase
{
    private static Position redPlayerPos = new Position { x = 4, y = 1 };
    private static Position bluePlayerPos = new Position { x = 1, y = 1 };

    private static readonly (int startX, int startY) redGridStart = (3, 0);
    private static readonly (int width, int height) redGridSize = (3, 3);

    private static readonly (int startX, int startY) blueGridStart = (0, 0);
    private static readonly (int width, int height) blueGridSize = (3, 3);

    [HttpGet("position")]
    public IActionResult GetPositions()
    {
        return Ok(new
        {
            redPlayer = new { x = redPlayerPos.x, y = redPlayerPos.y },
            bluePlayer = new { x = bluePlayerPos.x, y = bluePlayerPos.y }
        });
    }

    [HttpPost("move")]
    public IActionResult MovePlayer([FromBody] MoveRequest request)
    {
        if (request.color == "red")
        {
            redPlayerPos = Move(redPlayerPos, request.direction, redGridStart.startX, redGridStart.startY,
                redGridSize.width, redGridSize.height);
        }
        else if (request.color == "blue")
        {
            bluePlayerPos = Move(bluePlayerPos, request.direction, blueGridStart.startX, blueGridStart.startY,
                blueGridSize.width, blueGridSize.height);
        }
        else
        {
            // 錯誤的 color
            return BadRequest(new { message = "Unknown player color." });
        }

        // 無論如何，都回傳最新的位置
        return Ok(new
        {
            redPlayer = new { x = redPlayerPos.x, y = redPlayerPos.y },
            bluePlayer = new { x = bluePlayerPos.x, y = bluePlayerPos.y }
        });


    }

    private Position Move(Position currentPos, string direction, int startX, int startY, int width, int height)
    {
        int newX = currentPos.x;
        int newY = currentPos.y;

        switch (direction.ToLower())
        {
            case "up":
                newY -= 1;
                break;
            case "down":
                newY += 1;
                break;
            case "left":
                newX -= 1;
                break;
            case "right":
                newX += 1;
                break;
        }

        if (newX < startX || newX >= startX + width || newY < startY || newY >= startY + height)
            return currentPos;

        return new Position { x = newX, y = newY };
    }

    private static Random random = new Random();

    [HttpGet("AIrandom")]
    public IActionResult AIRandomPosition()
    {
        int NewX = random.Next(redGridStart.startX, redGridStart.startX + redGridSize.width);
        int NewY = random.Next(redGridStart.startY, redGridStart.startY + redGridSize.height);

        redPlayerPos = new Position { x = NewX, y = NewY };

        return Ok(new { redPlayer = redPlayerPos });
    }

    private static TurnStatus currentStatus = TurnStatus.none;

    [HttpGet("round")]
    public IActionResult GetCurrentStatus()
    {

        if (currentStatus == TurnStatus.none)
        {
            GameStatus.FirstStatus = GameStatus.FirstRound();
            currentStatus = GameStatus.FirstStatus;
        }

        else if (currentStatus == TurnStatus.blueplayer)
            {
                currentStatus = TurnStatus.redplayer;
            }

            else if (currentStatus == TurnStatus.redplayer)
            {
                currentStatus = TurnStatus.blueplayer;
            }

            return Ok(new GameRound
            {

                currentStatus = currentStatus.ToString()
            });
    }



    private static int MaxHealth = 10;
    private static int RedcurrentHealth = 10;
    private static int BluecurrentHealth = 10;
    
    private static string gameResult = "none";


    [HttpPost("attack")]
    public IActionResult Attack([FromBody] AttackRequest request)
    {
        int NewX = random.Next(redGridStart.startX, redGridStart.startX + redGridSize.width);
        int NewY = random.Next(redGridStart.startY, redGridStart.startY + redGridSize.height);

        var target = new Position { x = NewX, y = NewY };
        
        redPlayerPos = target;
        
        bool isHit = (request.Attacker.y == target.y);

        Position bulletEnd = isHit
            ? target
            : new Position { x = 6, y = request.Attacker.y };
        
        var losthealth = Math.Abs(request.Attacker.x - target.x)-1;

        if (isHit)
        {
            switch (losthealth)
            {
                case 0:
                case 1:
                    RedcurrentHealth -= 3;
                    break;
                case 2:
                    RedcurrentHealth -= 2;
                    break;
                case 3:
                case 4:
                    RedcurrentHealth -= 1;
                    break;
            } 
        }

        if (RedcurrentHealth <= 0)
        {
            gameResult = "blue";
        }

        

        return Ok(new AttackResult
        {
            IsHit = isHit,
            BulletEnd = bulletEnd,
            BluePlayer = request.Attacker,
            RedPlayer = target,
            MaxHealth = MaxHealth,
            RedCurrentHealth = RedcurrentHealth,
            BlueCurrentHealth = BluecurrentHealth,
            gameResult = gameResult
        });
    }

    [HttpPost("AIattack")]
    public IActionResult AIAttack([FromBody] AttackRequest request)
    {
        int NewX = random.Next(redGridStart.startX, redGridStart.startX + redGridSize.width);
        int NewY = random.Next(redGridStart.startY, redGridStart.startY + redGridSize.height);

        var attacker = new Position { x = NewX, y = NewY };
        
        
        bool isHit = (attacker.y == request.Target.y);
        
        redPlayerPos = attacker;
        
        Position bulletEnd = isHit
            ? request.Target
            : new Position { x = -1, y = attacker.y };
        
        var losthealth = Math.Abs(request.Target.x - attacker.x)-1;
        
        if (isHit)
        {
            switch (losthealth)
            {
                case 0:
                case 1:
                    BluecurrentHealth -= 3;
                    break;
                case 2:
                    BluecurrentHealth -= 2;
                    break;
                case 3:
                case 4:
                    BluecurrentHealth -= 1;
                    break;
            } 
        }

        if (BluecurrentHealth <= 0)
        {
            gameResult = "red";
        }
        
        
        return Ok(new AttackResult
        {
            IsHit = isHit,
            BulletEnd = bulletEnd,
            RedPlayer = attacker,
            BluePlayer = request.Target,
            MaxHealth = MaxHealth,
            RedCurrentHealth = RedcurrentHealth,
            BlueCurrentHealth = BluecurrentHealth,
            gameResult = gameResult
        });
    }
    
    [HttpPost("reset")]
    public IActionResult Reset()
    {
        RedcurrentHealth = 10;
        BluecurrentHealth = 10;
        currentStatus = TurnStatus.none;
        gameResult = "none";
        redPlayerPos = new Position { x = 4, y = 1 } ;
        bluePlayerPos = new Position { x = 1, y = 1 } ;
        return Ok("Game Reset");
            

    }
    
}






public class Position
{
    public int x { get; set; }
    public int y { get; set; }
}

public class MoveRequest
{
    public string color { get; set; }
    public string direction { get; set; }
}


public class AttackRequest
{
    public Position Attacker { get; set; }
    public Position Target { get; set; }
}

public class AttackResult
{
    public bool IsHit { get; set; }
    public Position BulletEnd { get; set; }
    
    public Position RedPlayer { get; set; }
    
    public Position BluePlayer { get; set; }
    
    public int MaxHealth { get; set; }
    
    public int RedCurrentHealth { get; set; }
    
    public int BlueCurrentHealth { get; set; }
    
    public string gameResult { get; set; }
    
}


public class GameStatus
{
    private static Random round = new Random();

    public static TurnStatus FirstRound()
    {
        int firstround = round.Next(1, 3);
        return (TurnStatus)firstround;
    }
        
    public static TurnStatus FirstStatus = FirstRound();
}

public class GameRound
{
    public string currentStatus { get; set; }
    
}

public class ResetData
{
    public int RedCurrentHealth { get; set; }
    
    public int BlueCurrentHealth { get; set; }
    
    
}
public enum TurnStatus
{
    none,
    blueplayer,
    redplayer
}
