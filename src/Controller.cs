using Microsoft.AspNetCore.Mvc;

namespace AfterOnResourceExecutionEventBug;
[ApiController]
public class Controller : ControllerBase
{
    [AsyncFilter]
    [HttpGet]
    [Route("/shortcircuit/action")]
    public ActionResult ShortCircuitAction() => Ok();

    [AsyncFilter]
    [HttpGet]
    [Route("/shortcircuit/resource")]
    public ActionResult ShortCircuitResource() => Ok();

    [AsyncFilter]
    [HttpGet]
    [Route("/shortcircuit/none")]
    public ActionResult ShortCircuitNone() => Ok();
}
