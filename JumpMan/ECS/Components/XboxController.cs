using System;
using System.Collections.Generic;
using System.Text;

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
