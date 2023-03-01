using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Physics.Particles;

namespace Physics.Particles.Generators
{
    public class DragGenerator : ForceGenerator
    {
        private float k1;
        private float k2;

        public DragGenerator(float k1, float k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        public Vector3 Update(Data data, float duration)
        {
            Vector3 force = data.Velocity;
            float dragCoeff = force.Length();
            dragCoeff = k1 * dragCoeff * k2 * dragCoeff * dragCoeff;

            if (force == Vector3.Zero)
                return force;
            force = Vector3.Normalize(force);
            force *= -dragCoeff;

            return force;
        }
    }
}
