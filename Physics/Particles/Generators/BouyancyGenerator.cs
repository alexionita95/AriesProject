using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Physics.Particles;

namespace Physics.Particles.Generators
{
    public class BouyancyGenerator : ForceGenerator
    {
        public float MaxDepth { get; set; }
        public float Volume { get; set; }
        public float LiquidHeight { get; set; }
        public float LiquidDensity { get; set; } = 1000;

        public BouyancyGenerator(float maxDepth, float volume, float liquidHeight, float liquidDensity = 1000)
        {
            MaxDepth = maxDepth;
            Volume = volume;
            LiquidHeight = liquidHeight;
            LiquidDensity = liquidDensity;
        }

        public Vector3 Update(Data data, float duration)
        {
            float depth = data.Position.Y;

            if (depth >= LiquidHeight + MaxDepth) return Vector3.Zero;

            Vector3 force = Vector3.Zero;

            if (depth <= LiquidHeight - MaxDepth)
            {
                force.Y = LiquidDensity * Volume;
                return force;
            }

            force.Y = LiquidDensity * Volume * (depth - MaxDepth - LiquidHeight) / 2 * MaxDepth;
            return force;

            throw new NotImplementedException();
        }
    }
}
