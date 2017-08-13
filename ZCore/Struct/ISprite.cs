using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface ISprite
    {
        Enum Sprite { get; set; }
        Vector2d Position {get;}
        Vector2d Direction { get; }
    }
}
