using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Physics.Particles;

namespace Physics.Particles.Generators
{
    public class FakeSpringGenerator : ForceGenerator
    {

        public Vector3 Anchor { get; set; }
        public float SpringConstant { get; set; }

        public float Damping { get; set; }

        public FakeSpringGenerator(Vector3 anchor, float springConstant, float damping)
        {
            Anchor = anchor;
            SpringConstant = springConstant;
            Damping = damping;
        }

        public Vector3 Update(Data data, float duration)
        {
            if (data.InfiniteMass)
                return Vector3.Zero;
            Vector3 position = data.Position;
            position -= Anchor;

            float gamma = 0.5f * MathF.Sqrt(4 * SpringConstant - Damping * Damping);
            if (gamma == 0.0f) return Vector3.Zero;

            Vector3 c = position * (Damping / (2.0f * gamma)) + data.Velocity * (1.0f / gamma);

            Vector3 target = position * MathF.Cos(gamma * duration) + c * MathF.Sin(gamma * duration);
            target *= MathF.Exp(-0.5f * duration * Damping);
            Vector3 accel = (target - position) * (1.0f / (duration * duration)) - data.Velocity * (1.0f / duration);
            return accel * data.Mass;
        }
    }
}
