using System;

namespace JumpMan.ECS.Components
{
    class XboxController : Controller
    {
        public override string Name => "Xbox Controller";

        public override void TakeInput()
        {
            throw new NotImplementedException();
        }
    }
}
