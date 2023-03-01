using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Physics.Particles;

namespace Physics.Particles.Generators
{
    public class BungeeGenerator : ForceGenerator
    {
        public Data Other;
        public float SpringConstant { get; set; }
        public float RestLength { get; set; }

        public BungeeGenerator(Data other, float springConstant, float restLength)
        {
            Other = other;
            SpringConstant = springConstant;
            RestLength = restLength;
        }

        public Vector3 Update(Data data, float duration)
        {
            Vector3 force = data.Position;
            force -= Other.Position;

            float magnitude = force.Length();
            if (magnitude <= RestLength) return Vector3.Zero;
            magnitude = magnitude - RestLength;
            magnitude *= SpringConstant;
            force = Vector3.Normalize(force);
            force *= -magnitude;

            return force;
        }
    }
}
