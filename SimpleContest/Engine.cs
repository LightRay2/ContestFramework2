using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleContest
{
    public static class Engine
    {
        public static Tuple<Vector2d, Vector2d> ProcessManCollision(Vector2d one, Vector2d two,  Vector2d oneSpeed,  Vector2d twoSpeed)
        {
            //http://vobarian.com/collisions/2dcollisions2.pdf
            var normal = (two - one).Normalized();
            var tangent = normal.PerpendicularLeft;

            var oneNormalProjection = Vector2d.Dot(normal, oneSpeed);
            var twoNormalProjection = Vector2d.Dot(normal, twoSpeed);
            var oneTangentProjection = Vector2d.Dot(tangent, oneSpeed);
            var twoTangentProjection = Vector2d.Dot(tangent, twoSpeed);

            //происходит обмен проекций на нормаль
            return Tuple.Create(
                (oneTangentProjection * tangent + twoNormalProjection * normal).Normalized(),
                (twoTangentProjection * tangent + oneNormalProjection * normal).Normalized()
                );
        }
    }
}
