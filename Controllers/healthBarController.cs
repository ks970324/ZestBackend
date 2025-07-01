using Microsoft.AspNetCore.Mvc;

namespace Zest.Controllers;


public class healthBarController : ControllerBase
{
    
    
    private static int maxHealth = 10;
    private static int currentHealth = 10;
    
    
    [HttpGet("current")]
    public IActionResult GetHealth()
    {
        return Ok(new { currentHealth, maxHealth });
    }
    
    
    [HttpPost("lost")]
    public IActionResult TakeDamage([FromBody] int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        return Ok(new { currentHealth, maxHealth });
    }
    
}