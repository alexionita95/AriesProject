using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Physics.Particles;

namespace Physics.Particles.Generators
{
    public class AnchoredSpringGenerator : ForceGenerator
    {

        public Vector3 Anchor { get; set; }
        public float SpringConstant { get; set; }
        public float RestLength { get; set; }

        public AnchoredSpringGenerator(Vector3 anchor, float springConstant, float restLength)
        {
            Anchor = anchor;
            SpringConstant = springConstant;
            RestLength = restLength;
        }

        public Vector3 Update(Data data, float duration)
        {
            Vector3 force = data.Position;
            force -= Anchor;
            float magnitude = force.Length();
            magnitude = magnitude - RestLength;
            magnitude *= SpringConstant;
            force = Vector3.Normalize(force);
            force *= -magnitude;
            return force;
        }
    }
}
